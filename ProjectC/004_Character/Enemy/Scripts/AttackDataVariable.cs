using UnityEngine;
using Arbor;
using System.Collections.Generic;

[System.Serializable]
public class AttackData
{
    [SerializeField, Tooltip("攻撃前の最大接近時間")] private FlexibleFloat m_maxApproachTime;
    public FlexibleFloat MaxApproachTime => m_maxApproachTime;

    [Header("攻撃抽選用データ")]
    [SerializeField]
    private FlexibleList<AttackLotteryData> m_lotteryData;
    public FlexibleList<AttackLotteryData> LotteryData => m_lotteryData;
}

[System.Serializable]
public class AttackLotteryData
{
    [Tooltip("最小の抽選値")]
    [SerializeField] public FlexibleInt m_defaultWeight;
    [Tooltip("抽選外したときの追加抽選値")]
    [SerializeField] public FlexibleInt m_weightInclude;
    [Tooltip("攻撃基準距離")]
    [SerializeField] public FlexibleFloat m_bestAttackDist;
    [Tooltip("攻撃前に接近するかどうか")]
    [SerializeField] public FlexibleBool m_isApproachBeforeAttack;

    private int m_loseCount;
    public int LoseCount { get { return m_loseCount; } set { m_loseCount = value; } }

    private float m_mostFarAttackDist;

    [HideInInspector]
    public float m_lotteryWeight;
}

[System.Serializable]
public class FlexibleAttackData : FlexibleField<AttackData>
{
    public FlexibleAttackData()
    {
    }

    public FlexibleAttackData(AttackData value) : base(value)
    {
    }

    public FlexibleAttackData(AnyParameterReference parameter) : base(parameter)
    {
    }

    public FlexibleAttackData(InputSlotAny slot) : base(slot)
    {
    }

    public static explicit operator AttackData(FlexibleAttackData flexible)
    {
        return flexible.value;
    }

    public static explicit operator FlexibleAttackData(AttackData value)
    {
        return new FlexibleAttackData(value);
    }
}

[System.Serializable]
public class InputSlotAttackData : InputSlot<AttackData>
{
}

[System.Serializable]
public class OutputSlotAttackData : OutputSlot<AttackData>
{
}

[AddComponentMenu("")]
public class AttackDataVariable : Variable<AttackData>
{
}
