using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using UnityEngine.UIElements;
using UnityEngine.SocialPlatforms.Impl;

namespace Arbor.StateMachine.StateBehaviours
{
    [AddComponentMenu("")]
    [AddBehaviourMenu("Enemy/KeepDistance")]
    public class KeepDistance : AgentMoveBase
    {
        [SerializeField] private FlexibleTransform m_target = new FlexibleTransform();
        [SerializeField] private FlexibleFloat m_stoppingDistance = new FlexibleFloat(0f);
        [SerializeField] private FlexibleFloat m_maxKeepTime = new FlexibleFloat(0f);
        private float m_keepTime;

        [SerializeField] private float m_rotDegAng;

        private EnemyInputProvider m_input;
        private bool m_moveRight;
        private float m_dist;

        public override void OnStateAwake()
        {
            m_input = transform.root.GetComponentInChildren<EnemyInputProvider>();
        }

        public override void OnStateBegin()
        {
            m_keepTime = m_maxKeepTime.value;
            m_moveRight = Random.Range(0, 2) == 0;
            m_dist = Vector3.Distance(m_target.value.position, transform.position);
        }

        protected override bool OnIntervalUpdate(AgentController agentController)
        {
            Vector3 pos = m_target.value.position;
            Vector3 nowRot = transform.position - m_target.value.position;
            nowRot.y = 0.0f;
            nowRot.Normalize();
            if (m_moveRight) pos += Quaternion.Euler(0, m_rotDegAng, 0) * nowRot * m_dist;
            else pos += Quaternion.Euler(0, -m_rotDegAng, 0) * nowRot * m_dist;

            return agentController.MoveTo(_Speed.value, 0.0f, pos);
        }

        public override void OnStateFixedUpdate()
        {
            base.OnStateFixedUpdate();
            m_keepTime -= Time.fixedDeltaTime;
            if (m_keepTime <= 0.0f)
            {
                OnDone();
                return;
            }

            if (m_target.value == null)
            {
                OnCantMove();
                return;
            }
            m_input.LookVector = m_target.value.position - transform.position;
        }
    }
}