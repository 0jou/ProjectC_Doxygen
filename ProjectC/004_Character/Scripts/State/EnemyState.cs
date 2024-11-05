using Arbor;
using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.VFX;

// キャラクターの基盤クラス
public partial class CharacterCore : MonoBehaviour, IDamageable
{
    //===================================
    // 敵側ステート
    //===================================
    // 各基本State（伊波）

    //タックル攻撃用のオブジェクトを指定（山本）
    [Header("タックル攻撃用のエフェクト")]
    [SerializeField] private GameObject m_rushAttackEffect = null;

    [System.Serializable]
    [AddTypeMenu("Enemy/ReadyAttack")]
    public class ActionState_EnemyReadyAttack : ActionState_Base
    {
        [SerializeField] private bool m_isNotKnockBack;
        [SerializeField] private bool m_isShowDushEffect;

        public override void OnEnter()
        {
            base.OnEnter();
            Core.m_isNoKnockBack = m_isNotKnockBack;

            //突進選択時、ダッシュエフェクトを発生させる（山本）
            if (m_isShowDushEffect)
            {
                if (Core.m_dashEffect)
                {
                    //ダッシュエフェクトを再生
                    Core.m_dashEffect?.Play();
                }

                if (Core.m_rushAttackEffect)
                {
                    //ラッシュエフェクトをアクティブ化(山本)
                    Core.m_rushAttackEffect.SetActive(true);
                    //数値を0.0fに
                    Core.m_rushAttackEffect.GetComponent<VisualEffect>()?.SetFloat("UVAlphaClippingValue", 0.0f);
                }

            }

            Core.m_inputProvider.AttackType = 0;
            Core.m_animator.SetInteger("DoAttackType", 0);
        }

        public override void OnFixedUpdate()
        {
            Core.SetRotateToTarget(Core.m_inputProvider.Destination - Core.transform.position);
        }
    }


    [System.Serializable]
    [AddTypeMenu("Enemy/UseItem")]
    public class ActionState_EnemyUseItem : ActionState_Base
    {
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Core.Move(0.0f);
        }
    }

    [System.Serializable]
    [AddTypeMenu("Enemy/Rush")]
    public class ActionState_EnemyRush : ActionState_Base
    {
        [Header("突進時間")]
        [SerializeField] private float m_rushTime;
        [Header("追尾時間")]
        [SerializeField] private float m_maxHomingTime;
        [Header("追尾の強さ"), Range(0.0f, 1.0f)]
        [SerializeField] private float m_rateHoming;

        [Header("CharactorCoreのdushSpeed基準にしたspeed補正値")]
        [SerializeField] private float m_magnificationSpeed = 1f;
        private float m_remainingTime;
        private float m_homingTime;
        private Collider m_collider;

        ////ラッシュエフェクト関係（山本）
        //[Header("ラッシュエフェクトの消失スピード")]
        //[SerializeField] private float m_rushEffectAppearSpeed = 0.1f;
        //ラッシュエフェクトの現在の進捗状況
        private float m_rushEffectApearProgress = 0.0f;


        public override void OnEnter()
        {
            base.OnEnter();
            m_remainingTime = m_rushTime;
            m_homingTime = m_maxHomingTime;
            m_rushEffectApearProgress = 0.0f;

            Core.m_isNoKnockBack = true;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            m_remainingTime -= Time.deltaTime;
            m_homingTime -= Time.deltaTime;

            //ラッシュエフェクトの進捗度に合わせて表示（山本）
            if (Core.m_rushAttackEffect)
            {
                //ダッシュ時間に合わせて進捗度が0=>1=>0になるように調整
                m_rushEffectApearProgress = (float)Math.Sin((1.0f - (m_remainingTime / m_rushTime)) * Math.PI);
                //進捗度をセット
                Core.m_rushAttackEffect.GetComponent<VisualEffect>().SetFloat("UVAlphaClippingValue", m_rushEffectApearProgress);
            }


            if (m_remainingTime <= 0f)
            {
                Core.m_animator.SetTrigger("EndAttack");

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
        }


        public override void OnFixedUpdate()
        {
            if (m_homingTime > 0.0f)
            {
                Vector3 rotVec = Core.m_inputProvider.Destination - Core.transform.position;
                rotVec.y = 0.0f;
                rotVec.Normalize();
                rotVec = Core.transform.forward + rotVec * m_rateHoming;
                Core.SetRotateToTarget(rotVec);
            }
            Core.Move(Core.Status.DushSpeed * m_magnificationSpeed);
        }

        public override void OnExit()
        {
            base.OnExit();

            //ダッシュエフェクトを止める（山本）
            if (Core.m_dashEffect)
            {
                Core.m_dashEffect?.Stop();
            }


            if (Core.m_rushAttackEffect)
            {
                //ラッシュ攻撃エフェクトを非アクティブにする
                Core.m_rushAttackEffect?.SetActive(false);
            }
        }
    }

    [System.Serializable]
    [AddTypeMenu("Enemy/ReadyThrowRock")]
    public class ActionState_ReadyThrowRock : ActionState_Base
    {
        [Header("追尾開始タイミング"), Range(0.0f, 1.0f)]
        [SerializeField] private float m_startHomingProgress;
        [Header("追尾の強さ"), Range(0.0f, 1.0f)]
        [SerializeField] private float m_rateHoming;

        Transform m_target;

        public override void OnEnter()
        {
            base.OnEnter();
            Core.m_isNoKnockBack = true;

            if (Core.transform.TryGetComponent(out ArborFSM arbor))
            {
                m_target = arbor.parameterContainer.GetTransform("Target");
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (Core.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > m_startHomingProgress)
            {
                if (!m_target) return;
                Vector3 rotVec = m_target.transform.position - Core.transform.position;
                rotVec.y = 0.0f;
                rotVec.Normalize();
                rotVec = Core.transform.forward + rotVec * m_rateHoming;
                Core.SetRotateToTarget(rotVec);
            }
            Core.Move(0.0f);
        }
    }


    [Serializable]
    [AddTypeMenu("Enemy/Discover")]
    public class ActionState_EnemyDiscover : ActionState_Base
    {
        // 以下の処理はEnemyIconElementに移行（伊波）
        //[SerializeField] private float m_iconShowTime = 2f;

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Core.Move(0.0f);
        }


        //public override async void OnEnter()
        //{
        //    base.OnEnter();
        //    Transform canvas = Core.transform.Find("EnemyCanvas");
        //    if (!canvas) return;
        //    if (!canvas.TryGetComponent(out EnemyIconController iconController)) return;
        //    iconController.ShowIcon(EnemyIconController.EnemyIconType.DiscoverIcon);
        //    await UniTask.Delay(TimeSpan.FromSeconds(m_iconShowTime));
        //    if (iconController)
        //    {
        //        iconController.HideIcon(EnemyIconController.EnemyIconType.DiscoverIcon);
        //    }
        //}
    }

    [Serializable]
    [AddTypeMenu("Enemy/LostSight")]
    public class ActionState_EnemyLostSight : ActionState_Base
    {
        // 以下の処理はEnemyIconElementに移行（伊波）
        //[SerializeField] private float m_iconShowTime = 5f;
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Core.Move(0.0f);
        }

        //public override async void OnEnter()
        //{
        //    base.OnEnter();
        //    Transform canvas = Core.transform.Find("EnemyCanvas");
        //    if (!canvas) return;
        //    if (!canvas.TryGetComponent(out EnemyIconController iconController)) return;
        //    iconController.ShowIcon(EnemyIconController.EnemyIconType.LostSightIcon);
        //    await UniTask.Delay(TimeSpan.FromSeconds(m_iconShowTime));
        //    if (iconController)
        //    {
        //        iconController.HideIcon(EnemyIconController.EnemyIconType.LostSightIcon);
        //    }
        //}
    }
}