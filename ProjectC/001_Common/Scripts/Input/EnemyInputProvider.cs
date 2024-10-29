using Arbor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NavMeshAgentやArborから敵制御に必要な値を取得する(伊波)

public class EnemyInputProvider : MonoBehaviour, IInputProvider
{
    [SerializeField] private PathFinding m_pathFinding;
    [SerializeField] private ParameterContainer m_parameterContainer;

    private void Awake()
    {
        if (!m_pathFinding)
        {
            Debug.LogError("PathFindingがセットされていません。");
        }
        if (!m_parameterContainer)
        {
            Debug.LogError("ParameterContainerがセットされていません。");
        }
    }

    public bool IsArrive
    {
        get { return m_pathFinding.IsArrived; }
    }

    private Vector3 m_lookVector;
    public Vector3 LookVector
    {
        get { return m_lookVector; }
        set { m_lookVector = value; }
    }

    public Vector3 MoveVector
    {
        get
        {
            Vector3 v = m_pathFinding.DesiredVelocity;
            v.y = 0;
            v.Normalize();
            return v;
        }
    }
    public Vector3 Destination { get { return m_pathFinding.Destination; } }

    public int AttackType
    {
        get
        {
            int type;
            m_parameterContainer.TryGetInt("DoAttackType", out type);
            return type;
        }
        set 
        {
            m_parameterContainer.SetInt("DoAttackType", value);
        }
    }

    //public bool DoAttack { get { return PlayerInputManager.Instance.PlayerAM["Attack"].triggered; } }
    public bool DoDush { get; } = false;
    public bool OnPressedDush { get; } = false;
    public bool OnReleasedDush { get; } = false;


    //追加（山本）
    public bool DoRolling { get; } = false;

    public bool SelectLeftItem { get; } = false;
    public bool SelectRightItem { get; } = false;
    public bool UseItem
    {
        get
        {
            bool use;
            m_parameterContainer.TryGetBool("UseItem", out use);
            m_parameterContainer.SetBool("UseItem", false);
            return use;
        }
    }

    //追加（山本）
    public bool UseStorySkill_1 { get; } = false;

    //追加（山本）
    public bool UseStorySkill_2 { get; } = false;

    public bool Gathering { get; } = false;
    public bool PutItem { get; } = false;
    public bool ReadyThrowItem { get; } = false;
    public bool ThrowItem { get; } = false;
    public bool Cancel { get; } = false;
    public bool Eat { get; } = false;
    public Vector2 ThrowAim { get; } = Vector2.zero;

}
