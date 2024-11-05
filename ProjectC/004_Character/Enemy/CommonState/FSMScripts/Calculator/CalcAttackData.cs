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
    [SerializeField]
    [SlotType(typeof(EnemyParameters))]
    private FlexibleComponent m_enemyParameters;

    [SerializeField] private OutputSlotFloat m_outputAttackDist;
    [SerializeField] private OutputSlotFloat m_outputMaxApproachTime;

    private EnemyParameters _enemyParameters;

    public override void OnCalculate()
    {
        if (_enemyParameters == null)
        {
            _enemyParameters = m_enemyParameters.value as EnemyParameters;
            if (!_enemyParameters)
            {
                Debug.LogError("EnemyParametersがセットされていません" + gameObject.name);
                return;
            }
        }
        if (m_attackType.value <= _enemyParameters.NowAttackData.LotteryData.Count && m_attackType.value > 0)
        {
            // 接近必須攻撃かどうか
            if (_enemyParameters.NowAttackData.LotteryData[m_attackType.value - 1].m_isApproachBeforeAttack)
            {
                // 指定距離or指定秒数追いかける
                m_outputAttackDist.SetValue(_enemyParameters.NowAttackData.LotteryData[m_attackType.value - 1].m_bestAttackDist);
                m_outputMaxApproachTime.SetValue(_enemyParameters.NowAttackData.MaxApproachTime);
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
