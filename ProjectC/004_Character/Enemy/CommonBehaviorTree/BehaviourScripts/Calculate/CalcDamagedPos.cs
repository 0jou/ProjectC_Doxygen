using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using UnityEngine.AI;

[AddBehaviourMenu("Vector3/CalcDamagedPos")]
[BehaviourTitle("CalcDamagedPos")]
[AddComponentMenu("")]
public class CalcDamagedPos : Calculator
{
    [SerializeField, SlotType(typeof(EnemyParameters))]
    private FlexibleComponent m_enemyParameters;
    [SerializeField] private FlexibleVector3 m_attackedPos;

    [SerializeField] private OutputSlotVector3 m_out;

    private EnemyParameters enemyParameters;

    public override void OnCalculate()
    {
        if (!enemyParameters)
        {
            enemyParameters = m_enemyParameters.value as EnemyParameters;
            if (!enemyParameters) return;
        }

        Vector3 outVal;
        outVal = enemyParameters.AttackedVec * enemyParameters.GetEnemyData().CharacterStatus.WalkSpeed;
        outVal += m_attackedPos.value;
        m_out.SetValue(outVal);
    }
}
