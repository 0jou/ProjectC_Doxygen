using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using UnityEngine.InputSystem;
using Arbor.Examples;


public class PlayerRolling : MonoBehaviour
{

    [SerializeField] float m_rollingPow;
    [SerializeField] float m_animationSpeed;
    [SerializeField] private PlayerInput m_playerInput;

    // カメラ回転関係
    [SerializeField] private Transform m_cameraTargetTransform;
    [SerializeField] private Camera m_mainCamera;

    private bool m_isRolling;
    private Animator m_animator;


    public bool IsRolling
    {
        get { return m_isRolling; }
    }

    // Start is called before the first frame update
    void Start()
    {

        m_isRolling = false;

        //Animatorコーポネントの参照を取得
        m_animator = GetComponent<Animator>();


        //AnimatorからObservableStateMachineTrigerの参照を取得
        ObservableStateMachineTrigger trigger = m_animator.GetBehaviour<ObservableStateMachineTrigger>();

        //回避アクションの開始イベント
        IDisposable enterState = trigger.OnStateEnterAsObservable()
            .Subscribe(onStateInfo =>
            {
                AnimatorStateInfo info = onStateInfo.StateInfo;
                if (info.IsName("Base Layer.Rolling"))
                {
                    Debug.Log("回避アクション開始");
                    m_isRolling = true;
                    m_animator.SetBool("RollingFlg", false);
                }

            }).AddTo(this);

        //回避アクションの終了イベント
        IDisposable exitState = trigger.OnStateExitAsObservable()
            .Subscribe(onStateInfo =>
            {
                AnimatorStateInfo info = onStateInfo.StateInfo;
                if (info.IsName("Base Layer.Rolling"))
                {
                    Debug.Log("回避アクション終了");
                    m_isRolling = false;

                }

            }).AddTo(this);


    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isRolling)
        {
            return;
        }

        var inputVector = m_playerInput.actions["Move"].ReadValue<Vector2>();

        if (inputVector.magnitude != 0.0f)
        {
            Vector3 playerDirection = Vector3.Normalize(new Vector3(inputVector.x, 0, inputVector.y));
            Vector3 cameraDirection = m_cameraTargetTransform.position - m_mainCamera.transform.position;
            cameraDirection.y = 0;
            cameraDirection.Normalize();

            //キャラクターをカメラを考慮し、ステック方向に回転
            Quaternion characterRotation = Quaternion.LookRotation(cameraDirection) 
                * Quaternion.LookRotation(playerDirection);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, characterRotation,60.0f);

        }

    }

    private void FixedUpdate()
    {
        if (m_isRolling)
        {

            AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);

            CharacterController characterController = GetComponent<CharacterController>();
            Vector3 velocity = transform.forward * m_rollingPow;

            velocity /= stateInfo.length;

            characterController.Move(velocity * Time.deltaTime);

        }
    }

    public void EnterRolling()
    {
        if (!m_isRolling)
        {
            //アニメーションスピードの変更
            m_animator.SetFloat("RollingSpeed", m_animationSpeed);
            m_animator.SetBool("RollingFlg", true);
        }
    }

}
