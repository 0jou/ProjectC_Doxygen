using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Float/CalcMoveSpeed")]
[BehaviourTitle("CalcMoveSpeed")]
[AddComponentMenu("")]
public class CalcMoveSpeed : Calculator {

    [SerializeField]
    [SlotType(typeof(EnemyParameters))]
    private FlexibleComponent m_enemyParameters;

    [SerializeField] private OutputSlotFloat m_outputTime;

    public override void OnCalculate() {
        EnemyParameters parameters = m_enemyParameters.value as EnemyParameters;
        if (parameters)
        {
            m_outputTime.SetValue(parameters.GetEnemyData().CharacterStatus.DushSpeed);
        }
	}
}
