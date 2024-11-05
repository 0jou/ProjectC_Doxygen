using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
[AddBehaviourMenu("Transform/GetEnemyTarget")]
[BehaviourTitle("GetEnemyTarget")]
public class GetEnemyTarget : Calculator
{
    [SerializeField]
    [SlotType(typeof(EnemyParameters))]
    private FlexibleComponent m_enemyParameters;

    [SerializeField] private OutputSlotTransform m_output;

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
        m_output.SetValue(_enemyParameters.Target);
    }
}
