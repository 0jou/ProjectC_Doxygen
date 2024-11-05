using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using UnityEngine.AI;

[AddBehaviourMenu("Float/CalcAgentParameters")]
[BehaviourTitle("CalcAgentParameters")]
[AddComponentMenu("")]
public class CalcAgentParameters : Calculator
{
    [SerializeField, SlotType(typeof(EnemyParameters))]
    private FlexibleComponent m_enemyParameters;

    [SerializeField] private OutputSlotFloat m_outMoveSpeed;
    [SerializeField] private OutputSlotFloat m_outIdleMoveRadius;
    [SerializeField] private OutputSlotFloat m_outStoppingDistance;

    private EnemyParameters enemyParameters;
    private NavMeshAgent agent;

    public override void OnCalculate()
    {
        if (!enemyParameters)
        {
            enemyParameters = m_enemyParameters.value as EnemyParameters;
            if (!enemyParameters) return;
        }


        m_outMoveSpeed.SetValue(enemyParameters.GetEnemyData().CharacterStatus.WalkSpeed);
        m_outIdleMoveRadius.SetValue(enemyParameters.GetEnemyData().IdleMoveRadius);

        if (!agent)
        {
            if (!enemyParameters.TryGetComponent(out agent)) return;
        }
        if (enemyParameters.Target == null)
        {
            m_outStoppingDistance.SetValue(agent.radius);
            return;
        }
        if (enemyParameters.Target.gameObject.TryGetComponent(out CapsuleCollider m_targetCollider))
        {
            float radius = transform.root.localScale.x * agent.radius;
            // REVIEW:敵同士で殴り合うようにしたい場合や、細長いコライダーに対応出来ないコードなので修正が必要
            m_outStoppingDistance.SetValue(m_targetCollider.radius * (float)System.Math.Sqrt(2f) + radius);
        }
        else
        {
            m_outStoppingDistance.SetValue(agent.radius);
        }
    }
}
