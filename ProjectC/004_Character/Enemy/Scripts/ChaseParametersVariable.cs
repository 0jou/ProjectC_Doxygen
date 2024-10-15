using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[System.Serializable]
public class ChaseParameters
{
	[Tooltip("平常時の索敵距離")]
    [SerializeField] private float m_searchCharacterDist = 5f;
    public float SearchCharacterDist { get { return m_searchCharacterDist; } }

    [Tooltip("料理の索敵距離")]
    [SerializeField] private float m_searchDishDist = 8f;
    public float SearchDishDist { get { return m_searchDishDist; } }

    [Tooltip("視界判定に関係なく強制で気付く距離")]
    [SerializeField] private float m_noticeDist = 2f;
    public float NoticeDist { get { return m_noticeDist; } }

    [Tooltip("視野角")]
    [SerializeField] private int m_viewAngle = 90;
    public int ViewAngle { get { return m_viewAngle; } }

    [Tooltip("ターゲットが視野にいなくても追いかけてくる時間")]
    [SerializeField] private float m_maxChaseTime = 1;
	public float MaxChaseTime { get { return m_maxChaseTime; } }

    [Tooltip("チェイスを続ける敵との最大距離")]
    [SerializeField] private float m_chaseDistFromTarget = 15.0f;
    public float ChaseDistFromTarget { get { return m_chaseDistFromTarget; } }

    [Tooltip("スポーン地点から移動する最大距離")]
    [SerializeField] private float m_distAwayFromSpawnPos = 15.0f;
	public float DistAwayFromSpawnPos { get { return m_distAwayFromSpawnPos; } }
}

[System.Serializable]
public class FlexibleChaseParameters : FlexibleField<ChaseParameters>
{
	public FlexibleChaseParameters()
	{
	}

	public FlexibleChaseParameters(ChaseParameters value) : base(value)
	{
	}

	public FlexibleChaseParameters(AnyParameterReference parameter) : base(parameter)
	{
	}

	public FlexibleChaseParameters(InputSlotAny slot) : base(slot)
	{
	}

	public static explicit operator ChaseParameters(FlexibleChaseParameters flexible)
	{
		return flexible.value;
	}

	public static explicit operator FlexibleChaseParameters(ChaseParameters value)
	{
		return new FlexibleChaseParameters(value);
	}
}

[System.Serializable]
public class InputSlotChaseParameters : InputSlot<ChaseParameters>
{
}

[System.Serializable]
public class OutputSlotChaseParameters : OutputSlot<ChaseParameters>
{
}

[AddComponentMenu("")]
public class ChaseParametersVariable : Variable<ChaseParameters>
{
}
