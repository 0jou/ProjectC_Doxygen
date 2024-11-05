/**
* @file AgentChaseToTarget.cs
* @brief Arbor3をつかった特定のオブジェクトを追いかける処理
*/
using System.Collections;
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
    [AddComponentMenu("")]
    [AddBehaviourMenu("Agent/AgentChaseToTransform")]
    [BuiltInBehaviour]
    /**
    * @brief 敵のチェイス処理用クラス（伊波）
    * @details Arbor3を通して、指定のTransformを追いかけるようNavMeshAgentに指示を出す
    */
    public sealed class AgentChaseToTransform : AgentMoveBase
    {
        #region Serialize fields

        [SerializeField]
        private FlexibleTransform m_targetTransform = new();
        [SerializeField]
        private FlexibleFloat m_stoppingDistance = new();

        [SerializeField]
        [SlotType(typeof(EnemyParameters))]
        private FlexibleComponent m_enemyParameters;

        [SerializeField]
        private FlexibleFloat m_maxSearchInterval = new(0.5f);

        [SerializeField]
        private OutputSlotBool m_outIsWatch;

        #endregion

        private Collider m_myCollider;
        private Vector3 m_targetPos = new();
        private float m_chaseTime = new();
        private bool m_isWatch;
        private float m_searchInterval;
        private readonly VisualFieldJudgment m_judgement = new();
        private ChaseData m_chaseData;

        protected override void OnAwake()
        {
            base.OnAwake();
            if (!transform.root.TryGetComponent(out m_myCollider))
            {
                Debug.LogError("コライダーを取得出来ませんでした" + gameObject.name, gameObject);
            }

            EnemyParameters parameters = m_enemyParameters.value as EnemyParameters;
            m_chaseData = parameters.GetChaseData();
            if (m_chaseData == null)
            {
                Debug.Log("ChaseDataが取得できませんでした" + gameObject.name);
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            WatchTarget();
            m_searchInterval = m_maxSearchInterval.value;
        }

        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            m_searchInterval -= Time.fixedDeltaTime;

            if (m_targetTransform.value == null)
            {
                m_chaseTime = 0.0f;
                return;
            }

            if (m_searchInterval <= 0f)
            {
                if (m_judgement.ChaseTarget(m_targetTransform.value.gameObject, m_chaseData, m_myCollider))
                {
                    WatchTarget();
                }
                else
                {
                    LostSightTarget();
                }

                // HACK:直値修正
                // 遠くにいるなら低頻度で座標更新
                if (Vector3.Distance(transform.position, m_targetTransform.value.position) <= m_chaseData.SearchCharacterDist)
                    m_searchInterval = m_maxSearchInterval.value;
                else m_searchInterval = m_maxSearchInterval.value * 2f;

                m_chaseTime -= m_searchInterval;
            }
        }

        protected override bool OnIntervalUpdate(AgentController agentController)
        {
            m_outIsWatch.SetValue(m_isWatch);
            return agentController.MoveTo(_Speed.value, m_stoppingDistance.value, m_targetPos);
        }


        private void WatchTarget()
        {
            if (!m_isWatch)
            {
                m_isWatch = true;
                m_chaseTime = m_chaseData.MaxChaseTime;
            }

            m_targetPos = m_targetTransform.value.position;
        }

        private void LostSightTarget()
        {
            m_isWatch = false;
            if (m_chaseTime > 0f)
            {
                m_targetPos = m_targetTransform.value.position;
            }
        }
    }
}