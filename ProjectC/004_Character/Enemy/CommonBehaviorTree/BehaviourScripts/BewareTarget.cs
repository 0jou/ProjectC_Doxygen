using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

// 敵の警戒状態の処理(伊波)

[AddComponentMenu("")]
[AddBehaviourMenu("Enemy/BewareTarget")]
public class BewareTarget : Decorator
{
    [SerializeField]
    [SlotType(typeof(EnemyParameters))]
    private FlexibleComponent m_enemyParameters;

    private Collider m_myCollider;
    private EnemyInputProvider m_input;
    private Vector3 m_startPos;
    private readonly VisualFieldJudgment m_judgement = new();
    private CharacterCore m_characterCore;
    private ChaseData m_chaseData;
    private Transform m_target;

    protected override void OnAwake()
    {
        transform.root.TryGetComponent(out m_myCollider);
        m_input = transform.root.GetComponentInChildren<EnemyInputProvider>();
        if (transform.root.TryGetComponent(out AgentController agent))
        {
            m_startPos = agent.StartPosition;
        }

        EnemyParameters parameters = m_enemyParameters.value as EnemyParameters;
        m_chaseData = parameters.GetChaseData();
        if (m_chaseData == null)
        {
            Debug.LogError("ChaseDataが取得できませんでした" + gameObject.name);
        }
        if (!parameters.transform.TryGetComponent(out m_characterCore))
        {
            Debug.LogError("CharacterCoreが取得できませんでした" + gameObject.name);
        }
    }

    protected override void OnStart()
    {
        EnemyParameters parameters = m_enemyParameters.value as EnemyParameters;

        if (parameters.Target == false)
        {
            return;
        }
        m_target = parameters.Target;
        if (m_characterCore)
        {
            Vector3 targetPos = m_target.position - transform.position;
            m_characterCore.SetRotateToTarget(targetPos);
        }
    }

    protected override bool OnConditionCheck()
    {
        if (!m_target) return false;

        if (Vector3.Distance(transform.position, m_startPos) >= m_chaseData.DistAwayFromSpawnPos) return false;

        float searchDist = m_chaseData.SearchCharacterDist;
        if (m_judgement.SearchTargetNearSpawn(m_target.gameObject, m_chaseData,
            m_myCollider, m_startPos, ref searchDist, m_chaseData.SearchCharacterDist))
        {
            m_input.LookVector = m_target.position - transform.position;
            return true;
        }

        return false;
    }

    protected override void OnEnd()
    {
        m_input.LookVector = Vector3.zero;
    }
}
