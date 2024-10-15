using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

// 敵の警戒状態の処理(伊波)

[AddComponentMenu("")]
[AddBehaviourMenu("Enemy/BewareTarget")]
public class BewareTarget : Decorator {

    [SerializeField] private FlexibleTransform m_target;
    [SerializeField] private FlexibleChaseParameters m_chaseParameters;

    private Collider m_myCollider;
    private EnemyInputProvider m_input;
    private Vector3 m_startPos;
    private readonly VisualFieldJudgment m_judgement = new();

    protected override void OnAwake()
    {
        transform.root.TryGetComponent(out m_myCollider);
        m_input = transform.root.GetComponentInChildren<EnemyInputProvider>();
        if (transform.root.TryGetComponent(out AgentController agent))
        {
            m_startPos = agent.StartPosition;
        }
    }

    protected override void OnStart()
    {
        if (transform.root.TryGetComponent(out AgentController agent))
        {
            //agent.Stop();
            Vector3 targetPos = m_target.value.position - transform.position;
            targetPos = targetPos.normalized * 0.5f;
            targetPos += transform.position;
            agent.MoveTo(0.1f, 1.0f, targetPos);
        }
    }

    protected override bool OnConditionCheck() {
        if (!m_target.value) return false;

        if (Vector3.Distance(transform.position, m_startPos) >= m_chaseParameters.value.DistAwayFromSpawnPos) return false;

        float searchDist = m_chaseParameters.value.SearchCharacterDist;
        if (m_judgement.SearchTargetNearSpawn(m_target.value.gameObject, m_chaseParameters.value, 
            m_myCollider, m_startPos, ref searchDist, m_chaseParameters.value.SearchCharacterDist))
        {
           m_input.LookVector = m_target.value.position - transform.position;
            return true;
        }

        return false;
	}

    protected override void OnEnd()
    {
        m_input.LookVector = Vector3.zero;
    }
}
