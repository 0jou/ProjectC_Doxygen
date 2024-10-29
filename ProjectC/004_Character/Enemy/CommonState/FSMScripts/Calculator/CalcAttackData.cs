/**
* @file CalcAttackData.cs
* @brief 攻撃に必要な情報を返すCalculator（伊波）
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
[AddBehaviourMenu("Float/CalcAttackDist")]
[BehaviourTitle("CalcAttackDist")]
/**
* @brief 攻撃に必要な情報を返すCalculator（伊波）
* @details 接近必須攻撃ならAttackDataの値を、そうでなければfloat.MaxValueを返す
*/
public class CalcAttackData : Calculator
{
    [SerializeField] private FlexibleInt m_attackType;
    [SerializeField] private FlexibleAttackData m_attackData;

    [SerializeField] private OutputSlotFloat m_outputAttackDist;
    [SerializeField] private OutputSlotFloat m_outputMaxApproachTime;

    public override void OnCalculate()
    {
        if (m_attackType.value <= m_attackData.value.LotteryData.value.Count && m_attackType.value > 0)
        {
            // 接近必須攻撃かどうか
            if (m_attackData.value.LotteryData.value[m_attackType.value - 1].m_isApproachBeforeAttack.value)
            {
                // 指定距離or指定秒数追いかける
                m_outputAttackDist.SetValue(m_attackData.value.LotteryData.value[m_attackType.value - 1].m_bestAttackDist.value);
                m_outputMaxApproachTime.SetValue(m_attackData.value.MaxApproachTime.value);
            }
            else
            {
                // 即攻撃
                m_outputAttackDist.SetValue(float.MaxValue);
                m_outputMaxApproachTime.SetValue(0.0f);
            }
        }
        else
        {
            // なるべく追いかける
            m_outputAttackDist.SetValue(0.0f);
            m_outputMaxApproachTime.SetValue(float.MaxValue);
        }
    }
}
