using Arbor;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.VFX;

// キャラクターの基盤クラス
public partial class CharacterCore : MonoBehaviour, IDamageable
{
    [Serializable]
    [AddTypeMenu("Enemy/Condition/Paralysis")]
    public class ActionState_Paralysis : CharacterCore.ActionState_Base
    {
        public override void OnEnter()
        {
            if (Core.m_dashEffect)
            {
                //ダッシュエフェクトを止める（山本）
                Core.m_dashEffect?.Stop();
            }

            if (Core.m_rushAttackEffect)
            {
                //ラッシュ攻撃エフェクトを非アクティブにする
                Core.m_rushAttackEffect?.SetActive(false);
            }
        }
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Core.Move(0.0f);
        }
    }

    [Serializable]
    [AddTypeMenu("Enemy/Condition/Sleep")]
    public class ActionState_Sleep : CharacterCore.ActionState_Base
    {
    }

    [Serializable]
    [AddTypeMenu("Enemy/Condition/Fire")]
    public class ActionState_Fire : CharacterCore.ActionState_Base
    {
        [SerializeField] private float m_rotateDegAngSpeed = 45.0f;
        [SerializeField] private float m_maxDelay = 0.3f;
        [SerializeField] private float m_maxAreaSize = 4.0f;
        private float m_delay;

        bool m_isSpin = true;
        Vector3 m_startPos;

        public override void OnEnter()
        {
            base.OnEnter();
            m_delay = m_maxDelay;
            m_isSpin = true;
            m_startPos = Core.transform.position;

            //リグ存在するならリグを働かせ,ターゲットの方向を向かせる（山本）
            if(Core.EnemyParameters.m_rig)
            {
                if (Core.EnemyParameters.m_rig.weight != 1.0f)
                    Core.EnemyParameters.m_rig.weight = 1.0f;
            }

        }


        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            //リグ存在するならリグを働かせ,ターゲットの方向を向かせる（山本）
            if (Core.EnemyParameters.m_rig)
            {
                if (Core.EnemyParameters.m_rig.weight != 1.0f)
                    Core.EnemyParameters.m_rig.weight = 1.0f;
            }

            m_delay -= Time.fixedDeltaTime;
            Core.Move(Core.Status.DushSpeed);
            if (m_isSpin)
            {
                var RotateTowards = Quaternion.Euler(0, m_rotateDegAngSpeed, 0) * Core.transform.root.forward;
                Core.SetRotateToTarget(RotateTowards);
            }
            if (m_delay > 0.0f) return;

            m_delay = m_maxDelay;

            m_isSpin = UnityEngine.Random.Range(0, 2) == 0;
            if (!m_isSpin)
            {
                if (Vector3.Distance(Core.transform.position, m_startPos) >= m_maxAreaSize)
                {
                    Core.SetRotateToTarget(m_startPos - Core.transform.position);
                }
                else
                {
                    Core.SetRotateToTarget(new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0.0f, UnityEngine.Random.Range(-1.0f, 1.0f)));
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            //リグ存在するならリグを働かせないようにする（山本）
            if (Core.EnemyParameters.m_rig)
            {
                if (Core.EnemyParameters.m_rig.weight != 0.0f)
                    Core.EnemyParameters.m_rig.weight = 0.0f;
            }

        }

        //[SerializeField]
        //private float m_rotateDegAngSpeed = 45.0f;

        //[SerializeField] private float m_minDelay = 0.3f;
        //[SerializeField] private float m_amaxDelay = 0.6f;
        //private float m_delay;

        //bool m_isSpin = true;

        //public override void OnEnter()
        //{
        //    base.OnEnter();
        //    m_delay = UnityEngine.Random.Range(m_minDelay, m_amaxDelay);
        //    m_isSpin = true;
        //}

        //public override void OnFixedUpdate()
        //{
        //    base.OnFixedUpdate();

        //    m_delay -= Time.fixedDeltaTime;
        //    Core.Move(Core.m_dushSpeed);
        //    if (m_isSpin)
        //    {
        //        var RotateTowards = Quaternion.Euler(0, m_rotateDegAngSpeed, 0) * Core.transform.root.forward;
        //        Core.RotateToTarget(RotateTowards);
        //    }
        //    if (m_delay > 0.0f) return;

        //    m_delay = UnityEngine.Random.Range(m_minDelay, m_amaxDelay); ;

        //    m_isSpin = !m_isSpin;
        //    if (!m_isSpin)
        //    {
        //        Core.RotateToTarget(new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0.0f, UnityEngine.Random.Range(-1.0f, 1.0f)));
        //    }
        //}
    }


    [Serializable]
    [AddTypeMenu("Enemy/Condition/Stun")]
    public class ActionState_Stun : ActionState_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();

            //ダッシュエフェクトを止める（山本）
            if (Core.m_dashEffect != null)
            {
                Core.m_dashEffect.Stop();
            }

            if (Core.m_rushAttackEffect)
            {
                //ラッシュ攻撃エフェクトを非アクティブにする
                Core.m_rushAttackEffect?.SetActive(false);
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Core.Move(0.0f);
        }
    }
}