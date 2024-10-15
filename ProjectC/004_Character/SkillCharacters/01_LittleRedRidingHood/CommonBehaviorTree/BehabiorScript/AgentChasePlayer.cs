using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;
using UnityEngine.Serialization;



namespace Arbor.BehaviourTree.Actions
{
    [AddComponentMenu("")]
    [AddBehaviourMenu("Agent/AgentChasePlayer")]
    public sealed class AgentChasePlayer : AgentMoveBase
    {

        //プレイヤーを追跡するためのアクション（山本）

        [SerializeField]
        private FlexibleTransform m_playerTransform = new FlexibleTransform();
        [SerializeField]
        private FlexibleFloat m_stoppingDistance = new FlexibleFloat();
        [SerializeField]
        private FlexibleFloat m_warpDistance = new FlexibleFloat();
        [SerializeField]
        private FlexibleChaseParameters m_chaseParameters = new FlexibleChaseParameters();
        [SerializeField]
        private OutputSlotBool m_outIsReach;
        

        private Transform m_myTransform;
        private Vector3 m_targetPos = new();
        private bool m_warpFlg = false;

        protected override void OnAwake()
        {
            base.OnAwake();
            m_myTransform = transform.root;

        }

        protected override void OnStart()
        {
            base.OnStart();
            m_warpFlg = false;

            //プレイヤーの位置情報を探して代入
            foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (chara.transform.tag == "Player")
                {
                    m_playerTransform.parameter.SetTransform(chara.transform);
                    m_targetPos = m_playerTransform.value.position;

                    var vec = m_targetPos - transform.position;
                    var length = vec.magnitude;

                    if (m_warpDistance.value <= length)
                    {
                        m_warpFlg = true;
                        return;
                    }

                    break;
                }
            }

        }

        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            m_warpFlg = false;

            //プレイヤーの位置情報を探して代入
            foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (chara.transform.tag == "Player")
                {
                    m_playerTransform.parameter.SetTransform(chara.transform);
                    m_targetPos = m_playerTransform.value.position;

                    var vec = m_targetPos - transform.position;
                    var length = vec.magnitude;

                    if (m_warpDistance.value <= length)
                    {
                        m_warpFlg = true;
                        m_outIsReach.SetValue(m_warpFlg);
                        return;
                    }

                    break;
                }
            }



        }

        protected override bool OnIntervalUpdate(AgentController agentController)
        {
           
            m_outIsReach.SetValue(m_warpFlg);
            return agentController.MoveTo(_Speed.value, m_stoppingDistance.value, m_targetPos);
        }


    }
}

