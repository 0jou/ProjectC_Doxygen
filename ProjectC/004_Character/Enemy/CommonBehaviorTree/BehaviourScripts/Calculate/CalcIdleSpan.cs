using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Float/CalcIdleSpan")]
[BehaviourTitle("CalcIdleSpan")]
[AddComponentMenu("")]
public class CalcIdleSpan : Calculator {

    [SerializeField]
    [SlotType(typeof(EnemyParameters))]
    private FlexibleComponent m_enemyParameters;

    [SerializeField] private OutputSlotFloat m_outputTime;

    public override void OnCalculate() {
        EnemyParameters parameters = m_enemyParameters.value as EnemyParameters;
        if (parameters)
        {
            m_outputTime.SetValue(parameters.GetEnemyData().IdleSpan);
        }
	}
}
