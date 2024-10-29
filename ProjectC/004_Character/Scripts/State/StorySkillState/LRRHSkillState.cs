using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;


// キャラクターの基盤クラス
public partial class CharacterCore : MonoBehaviour, IDamageable
{
    //====================================================
    //プレイヤーのスキルに関するステート
    //===================================================

    [System.Serializable]
    [AddTypeMenu("Player/SkillLRR/Fall")]
    public class ActionState_SkillLRR_Fall : CharacterCore.ActionState_Base
    {
        private UsingEffectByLRRH m_effect = null;
        private bool m_onceFlg = false;


        public override void OnUpdate()
        {

            //地面に着地した時
            if (Core.m_charaCtrl.Motor.GroundingStatus.IsStableOnGround && m_onceFlg == false)
            {
                if (m_effect.SmokeEffect == null)
                {
                    Debug.LogWarning("UsingEffectByLRRHのSmokeEffectが登録されていません");
                    return;
                }


                //スモークEffect発生
                GameObject.Instantiate(m_effect.SmokeEffect, Core.transform.position, Core.transform.rotation);

                Core.m_animator.SetTrigger("Idle");
                m_onceFlg = true;

                // 追加　上甲　着地音(3D)
                SoundManager.Instance.Start3DPlayback("Landing",Core.transform.position);
            }

        }

        public override void OnEnter()
        {
            base.OnEnter();


            m_effect = Core.m_animator.GetComponent<UsingEffectByLRRH>();
            if (m_effect == null)
            {
                Debug.LogWarning("UsingEffectByLRRHコンポーネントがついていません");
            }


            if (m_effect.TrailEffect == null)
            {
                Debug.LogWarning("UsingEffectByLRRHのTrailEffectが登録されていません");
                return;
            }



            //TrailEffect発生
            m_effect.m_trailEffect = GameObject.Instantiate(m_effect.TrailEffect, Core.transform.position,
                Quaternion.LookRotation(-Core.transform.up), Core.transform);


        }

        public override void OnExit()
        {
            m_effect.m_trailEffect.SetActive(false);
            Core.PlayerSkillsParameters.OffsetEfffect = m_effect.m_trailEffect;
        }
    }



    [System.Serializable]
    [AddTypeMenu("Player/SkillLRR/Appear")]
    public class ActionState_SkillLRR_Appear : CharacterCore.ActionState_Base
    {
        public override void OnExit()
        {
            base.OnExit();
           
            Core.PlayerSkillsParameters.SwitchPathfinding(true);

        }
    }


    [System.Serializable]
    [AddTypeMenu("Player/SkillLRR/Attack")]
    public class ActionState_SkillLRR_Attack : CharacterCore.ActionState_Base
    {
        public override void OnUpdate()
        {
            Core.Move(0.0f);
        }

        public override void OnEnter()
        {
            Core.PlayerSkillsParameters.AttackCount++;
            Core.Move(0.0f);
        }

        public override void OnExit()
        {
            Core.m_animator.SetInteger("AttackNum", Core.PlayerSkillsParameters.AttackCount);
        }

    }

    [System.Serializable]
    [AddTypeMenu("Player/SkillLRR/Idle")]
    public class ActionState_SkillLRR_Idle : CharacterCore.ActionState_Base
    {
        Vector3 m_targetPos = Vector3.zero;

        public override void OnUpdate()
        {

            if (Core.m_animator.GetComponent<SetRigInfo>().UseRig.weight == 0)
            {
                //リグが働くようにWeightを1に
                Core.m_animator.GetComponent<SetRigInfo>().UseRig.weight = 1.0f;
            }
        }

        public override void OnFixedUpdate()
        {
            Vector3 targetVec = GetNearestEnemyVector();
            RotateToTargetEnemy(targetVec);
        }

        //public override void OnEnter()
        //{
        //    base.OnEnter();
        //}

        //public override void OnExit()
        //{
        //}

        //一番近いエネミーの方向に向き、むき終えると攻撃ステートへと移動
        private void RotateToTargetEnemy(Vector3 targetVec)
        {
            if (targetVec == Vector3.zero)
            {
                return;
            }

            //見つめる先の位置取得
            Vector3 nowTargetPos = Core.m_animator.GetComponent<SetRigInfo>().TargetTransform.position;
            //目標の地点へ移動 
            nowTargetPos = Vector3.MoveTowards(nowTargetPos, m_targetPos, Core.PlayerSkillsParameters.MoveLookTargetSpeed * Time.fixedDeltaTime);
            //座標更新
            Core.m_animator.GetComponent<SetRigInfo>().TargetTransform.position = nowTargetPos;
            //距離取得
            float dist = Vector3.Distance(m_targetPos, nowTargetPos);

            if (Vector3.Angle(targetVec, Core.transform.forward) < 1f && dist == 0.0f)
            {
                Core.m_animator.SetTrigger("SkillAttack");

                return;
            }

            targetVec.Normalize();
            //Quaternion characterTargetRotation = Quaternion.LookRotation(targetVec);
            //Core.transform.rotation = Quaternion.RotateTowards(Core.transform.rotation, characterTargetRotation, Core.m_rotationSpeed * Time.fixedDeltaTime);

            Core.m_charaCtrl.LookVector = targetVec;
        }


        //一番近い敵の位置情報から向く方向を取得する関数
        private Vector3 GetNearestEnemyVector()
        {
            Vector3 nearestEnemyPos = Vector3.zero;
            Vector3 targetVec = Vector3.zero;
            float nearestDist = float.MaxValue;

            foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (chara.GroupNo != CharacterGroupNumber.enemy) continue;
                if (chara.m_characterStatus.m_hp.Value <= 0.0f) continue;

                if (SearchTarget(chara.gameObject, ref nearestDist))
                {
                    nearestEnemyPos = chara.transform.position;
                    nearestEnemyPos.y += chara.GetComponent<CapsuleCollider>().center.y;
                }
            }

            m_targetPos = nearestEnemyPos;

            //Core.PlayerSkillsParameters.TargetPos = m_targetPos;

            targetVec = m_targetPos - Core.transform.position;
            targetVec.y = 0;
            targetVec.Normalize();
            return targetVec;
        }


        //メタAIから敵の情報を取ってきて、一番近いエネミーかどうかチェックする関数(壁とか考慮せず、距離だけ)
        private bool SearchTarget(GameObject _target, ref float _nearestDist)
        {
            float dist;

            dist = Vector3.Distance(Core.transform.position, _target.transform.position);

            if (dist > _nearestDist) return false;

            _nearestDist = dist;

            return true;
        }

    }

    //拡縮して消滅
    [System.Serializable]
    [AddTypeMenu("Player/SkillLRR/End")]
    public class ActionState_SkillLRR_End : CharacterCore.ActionState_Base
    {

        //public override void OnUpdate()
        //{
        //    //Core.transform.DOScale(0.0f, 0.5f);
        //}

        //public override void OnEnter()
        //{

        //}

        //public override void OnExit()
        //{
        //    //DOTween.Kill(this, true);
        //    //スキルプレハブ破棄
        //    //Destroy(Core.gameObject);
        //}

    }

    //ジャンプして消滅
    [System.Serializable]
    [AddTypeMenu("Player/SkillLRR/Jumping")]
    public class ActionState_SkillLRR_Jumping : CharacterCore.ActionState_Base
    {
        private Vector3 m_jumpPos = Vector3.zero;
        private float m_jumpPow = 0.0f;
        private float m_time = 0.0f;
        private UsingEffectByLRRH m_effect;
        private bool m_onceFlg = false;

        public override void OnUpdate()
        {

            Vector3 vec = Core.transform.position - m_jumpPos;
            float length = vec.magnitude;

            if (Core.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
            {
                if (m_onceFlg == false)
                {
                    //スモークEffect発生
                    var effect = GameObject.Instantiate
                        (m_effect.SmokeEffect, Core.transform.position, Core.transform.rotation);

                    if (effect == null)
                    {
                        Debug.LogWarning("UsingEffectByLRRHのSmokeEffectが登録されていません");
                    }


                    m_onceFlg = true;

                }

                m_time += Time.deltaTime;
                m_jumpPow += 0.5f;
                Core.m_charaCtrl.Jump(m_jumpPow * m_time);
            }

            if (length >= 10.0f)
            {
                Destroy(Core.gameObject);
            }
        }

        public override void OnEnter()
        {
            m_effect = Core.m_animator.GetComponent<UsingEffectByLRRH>();
            if (m_effect == null)
            {
                Debug.LogWarning("UsingEffectByLRRHコンポーネントがついていません");
            }


            Core.PlayerSkillsParameters.SwitchPathfinding(false);

            m_jumpPos = Core.transform.position;

            //TrailEffectの活性化
            Core.PlayerSkillsParameters.OffsetEfffect.SetActive(true);

            Core.m_charaCtrl.Gravity = Vector3.zero;
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }

    //プレイヤーから離れすぎるとワープする
    [System.Serializable]
    [AddTypeMenu("Player/SkillLRR/Warp")]
    public class ActionState_SkillLRR_Warp : CharacterCore.ActionState_Base
    {
        [SerializeField]
        private float m_playerDist = 2.0f;

        private GameObject m_player = null;


        public override void OnEnter()
        {
            base.OnEnter();

            if (m_player != null)
            {
                return;
            }

            //プレイヤーを探して代入
            foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (chara.transform.tag == "Player")
                {
                    m_player = chara.gameObject;
                }

            }



        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (m_player)
            {
                var nextVec = m_player.transform.position - Core.transform.position;
                nextVec.Normalize();

                //ターゲットの方を向く
                Core.SetRotateToTarget(nextVec);
            }

        }


        public override void OnExit()
        {
            base.OnExit();

            if (!m_player)
            {
                return;
            }

            //ターゲット位置へとワープする
            var appearPos = m_player.transform.position - m_player.transform.forward * m_playerDist;
            Core.m_charaCtrl.SetPositionMotor(appearPos, true);


            var nextVec = m_player.transform.position - Core.transform.position;
            nextVec.Normalize();

            //ターゲットの方を向く
            Core.SetRotateToTarget(nextVec);

        }

    }


    //消失
    [System.Serializable]
    [AddTypeMenu("Player/SkillLRR/Disappear")]
    public class ActionState_AkillLRR_Disappear : CharacterCore.ActionState_Base
    {



    }

}