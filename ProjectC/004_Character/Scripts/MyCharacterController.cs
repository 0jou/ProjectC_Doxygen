using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using System;
using UnityEngine.TextCore.Text;
using UniRx;
using UniRx.Triggers;
using TMPro;


public struct PlayerCharacterInputs
{
    public float MoveAxisForward;
    public float MoveAxisRight;
    public Quaternion CameraRotation;
}

public class MyCharacterController : MonoBehaviour, ICharacterController
{
    public KinematicCharacterMotor Motor;

    // 当たり判定を行わないタグ
    [SerializeField]
    [Tag]
    private List<string> m_tagList = new();

    [Header("Move")]
    public float MaxStableMoveSpeed = 10f;
    public float StableMovementSharpness = 15;
    public float OrientationSharpness = 10;

    [Header("Misc")]
    public bool RotationObstruction;
    public Vector3 Gravity = new Vector3(0, -30f, 0);
    public Transform MeshRoot;

    [Header("Animation Parameters")]
    public Animator CharacterAnimator;
    public float ForwardAxisSharpness = 10;
    public float TurnAxisSharpness = 5;

    private bool m_isRootMotion = true;
    public bool IsRootMotion { get { return m_isRootMotion; } set { m_isRootMotion = value; } }

    private float m_moveSpeed;
    public float MoveSpeed { set { m_moveSpeed = value; } get { return m_moveSpeed; } }

    public Vector3 m_moveVec = new Vector3(0, 0, 0);
    public Vector3 MoveVec { set { m_moveVec = value; } get { return m_moveVec; } }

    private Vector3 m_lookVector;
    public Vector3 LookVector { set { m_lookVector = value; } get { return m_lookVector; } }

    //private Vector3 _moveInputVector;
    //private Vector3 _lookInputVector;

    private Vector3 _internalVelocityAdd = Vector3.zero;

    private float m_speedRate;
    public float SpeedRate { get { return m_speedRate; } set { m_speedRate = value; } }

    // ルートモーション関連
    private Vector3 _rootMotionPositionDelta;
    private Quaternion _rootMotionRotationDelta;
    private float _targetForwardAxis;
    private float _targetRightAxis;
    private float _forwardAxis;
    private float _rightAxis;


    private void Start()
    {
        // Assign to motor
        Motor.CharacterController = this;

        CharacterAnimator.gameObject.OnAnimatorMoveAsObservable()
            .Where(_ => enabled)
            .Subscribe(_ =>
            {
                if (m_isRootMotion)
                {
                    _rootMotionPositionDelta += CharacterAnimator.deltaPosition;
                    _rootMotionRotationDelta = CharacterAnimator.deltaRotation * _rootMotionRotationDelta;
                }
            }
        );
    }

    private void Update()
    {
        if (m_isRootMotion)
        {
            // ルートモーション関連
            _forwardAxis = Mathf.Lerp(_forwardAxis, _targetForwardAxis, 1f - Mathf.Exp(-ForwardAxisSharpness * Time.deltaTime));
            _rightAxis = Mathf.Lerp(_rightAxis, _targetRightAxis, 1f - Mathf.Exp(-TurnAxisSharpness * Time.deltaTime));

            if (FindParameter("Forward")) CharacterAnimator.SetFloat("Forward", _forwardAxis);
            if (FindParameter("Turn")) CharacterAnimator.SetFloat("Turn", _rightAxis);
            if (FindParameter("OnGround")) CharacterAnimator.SetBool("OnGround", Motor.GroundingStatus.IsStableOnGround);
        }
    }

    private bool FindParameter(string name)
    {
        foreach (AnimatorControllerParameter parameter in CharacterAnimator.parameters)
        {
            if (parameter.name == name)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// (Called by KinematicCharacterMotor during its update cycle)
    /// This is called before the character begins its movement update
    /// </summary>
    public void BeforeCharacterUpdate(float deltaTime)
    {
    }

    /// <summary>
    /// (Called by KinematicCharacterMotor during its update cycle)
    /// This is where you tell your character what its rotation should be right now. 
    /// This is the ONLY place where you should set the character's rotation
    /// </summary>
    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (m_lookVector != Vector3.zero && OrientationSharpness > 0f)
        {
            m_lookVector.y = 0f;
            m_lookVector.Normalize();
            // Smoothly interpolate from current to target look direction
            Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, m_lookVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

            // Set the current rotation (which will be used by the KinematicCharacterMotor)
            currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);

            if (Vector3.Angle(transform.forward, m_lookVector) < 1.0f)
            {
                m_lookVector = Vector3.zero;
            }
        }

        // ルートモーション
        currentRotation = currentRotation * _rootMotionRotationDelta;
    }

    /// <summary>
    /// (Called by KinematicCharacterMotor during its update cycle)
    /// This is where you tell your character what its velocity should be right now. 
    /// This is the ONLY place where you can set the character's velocity
    /// </summary>
    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        if (Motor.GroundingStatus.IsStableOnGround)
        {
            // Reorient source velocity on current ground slope (this is because we don't want our smoothing to cause any velocity losses in slope changes)
            //currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

            // Calculate target velocity
            //Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
            //Vector3 reorientedInput = Vector3.Cross(Motor.GroundingStatus.GroundNormal, inputRight).normalized * _moveInputVector.magnitude;

            if (!m_isRootMotion)
            {
                Vector3 targetMovementVelocity;
                if (m_moveVec.magnitude == 0.0f) targetMovementVelocity = Motor.CharacterForward * m_moveSpeed * m_speedRate;
                else targetMovementVelocity = m_moveVec * m_moveSpeed * m_speedRate;

                // Smooth movement Velocity
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1 - Mathf.Exp(-StableMovementSharpness * deltaTime));
            }
            // ルートモーションの移動
            else if (deltaTime > 0)
            {
                // The final velocity is the velocity from root motion reoriented on the ground plane
                Vector3 Velocity = _rootMotionPositionDelta / deltaTime;
                currentVelocity = Motor.GetDirectionTangentToSurface(Velocity, Motor.GroundingStatus.GroundNormal) * Velocity.magnitude;
                //currentVelocity *= 3.0f;
            }
        }
        else
        {
            if (_forwardAxis > 0f)
            {
                // If we want to move, add an acceleration to the velocity
                Vector3 targetMovementVelocity = Motor.CharacterForward * _forwardAxis * m_moveSpeed * m_speedRate;
                Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, Gravity);
                currentVelocity += velocityDiff * deltaTime;
            }

            // Gravity
            currentVelocity += Gravity * deltaTime;
        }

        if (_internalVelocityAdd.sqrMagnitude > 0f)
        {
            currentVelocity += _internalVelocityAdd;
            _internalVelocityAdd = Vector3.zero;
        }

        m_moveVec = Vector3.zero;
        m_speedRate = 1.0f;
    }

    /// <summary>
    /// (Called by KinematicCharacterMotor during its update cycle)
    /// This is called after the character has finished its movement update
    /// </summary>
    public void AfterCharacterUpdate(float deltaTime)
    {
        // ルートモーション関連 Updateでためたルートモーションの移動量をリセット
        _rootMotionPositionDelta = Vector3.zero;
        _rootMotionRotationDelta = Quaternion.identity;
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        foreach (var tag in m_tagList)
        {
            if (coll.CompareTag(tag)) return false;
        }


        return true;
    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
    }

    public void PostGroundingUpdate(float deltaTime)
    {
    }

    public void AddVelocity(Vector3 velocity)
    {
        _internalVelocityAdd += velocity;
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {
    }


    public void Jump(float _jumpPower)
    {
        Motor.ForceUnground(0.1f);
        AddVelocity(new Vector3(0f, _jumpPower, 0f));
    }


    public void SetPositionMotor(Vector3 _pos, bool _bypassInterpolation = true)
    {
        if (Motor == null)
        {
            Debug.LogError("モーターがシリアライズされていません");
            return;
        }

        Motor.SetPosition(_pos, _bypassInterpolation);
    }

    public void AddNoHitTag(string _tagName)
    {
        if (_tagName.Length == 0)
        {
            return;
        }

        m_tagList.Add(_tagName);
    }


}