using Arbor;
using CriWare;
using KinematicCharacterController;
using StorySkillInfo;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

// キャラクターの基盤クラス
public partial class CharacterCore : MonoBehaviour, IDamageable
{
    // 止まってる時と歩き(走り)時のステート(倉田　仕様自体は伊波)
    [System.Serializable]
    [AddTypeMenu("Common/IdleAndMove")]
    public class ActionState_IdleAndMove : ActionState_Base
    {
        private float m_speedStick = 0.0f;
        private bool m_isRun = false;

        public override void OnEnter()
        {
            base.OnEnter();
            Core.m_isNoKnockBack = false;
            switch (Core.GroupNo)
            {
                case CharacterGroupNumber.player:
                    {
                        if (Core.m_inputType == InputTypes.Player)
                        {
                            Core.PlayerParameters.StartVanishWeapon();
                        }
                    }
                    break;
            }
        }

        public override void OnExit()
        {
            Core.m_isRun = false;
            if (Core.m_dashEffect != null)
            {
                Core.m_dashEffect.Stop();
            }
        }

        void PlayerUpdate(Vector3 input)
        {
            if (Core.PlayerParameters == null)
            {
                Debug.LogError("PlayerParametersが設定されていません");
            }

            // ダッシュ時の判定処理
            if (Core.m_inputProvider.DoDush &&
            (Core.Status.m_stamina.Value >= Core.Status.m_dashStaminaCost) &&
            (input.magnitude != 0.0f))
            {
                if (Core.m_isNoStamina)
                {
                    Core.m_isRun = false;
                    Core.m_isPush = true;
                    Core.m_isNoStamina = false;
                }

                if (!Core.m_isPush && !Core.m_isRun)
                {
                    Core.m_isRun = true;
                    //ダッシュエフェクト再生
                    Core.m_dashEffect.Play();
                    Core.m_isPush = true;
                }
                else if (Core.m_inputProvider.OnPressedDush)//走り中に押しで走り解除
                {
                    Core.m_isRun = false;
                    //ダッシュエフェクト停止
                    Core.m_dashEffect.Stop();
                    Core.m_isPush = true;
                }

            }
            else if (!Core.m_inputProvider.DoDush)//ダッシュキーを離している時
            {
                Core.m_isPush = false;

                if (Core.m_isNoStamina)
                {
                    Core.m_isNoStamina = false;
                }

            }

            // 移動そのものに対する判定処理
            if (input.magnitude == 0.0f)
            {
                // 走り中かつ移動入力がない場合は走り状態解除
                if (Core.m_isRun)
                {
                    Core.m_isRun = false;
                    //ダッシュエフェクト停止
                    Core.m_dashEffect.Stop();

                }
            }

            //武器出現エフェクトのカウント
            Core.PlayerParameters.UpdateVanishWeapon(Core.m_animator);

            // 取得状態へ（吉田）
            // 採取可能状態にあるアイテムを探す→あったらState変更可
            if (SearchGatheringItem())
            {
                if (Core.m_inputProvider.Gathering)
                {
                    Core.m_animator.SetTrigger("Gathering");

                    //武器出現時にイベントを再生し武器を消失する
                    Core.PlayerParameters.HideWeapon(Core.m_animator);
                }

                // UI表示
                float distance = Vector3.Distance(Core.PlayerParameters.m_ableGatheringItem.transform.position, Core.transform.position);
                Core.PlayerParameters.AddAnyActionUIState(ActionUIController.ActionUIState.AbleGatherings, distance);
                Core.PlayerParameters.AddActionUIState(ActionUIController.ActionUIState.AbleGatherings);

                return;
            }
            else
            {
                // UI非表示
                Core.PlayerParameters.RemoveAnyActionUIState(ActionUIController.ActionUIState.AbleGatherings);
                Core.PlayerParameters.RemoveActionUIState(ActionUIController.ActionUIState.AbleGatherings);
            }


            // 攻撃ボタン押したら、攻撃に遷移
            if (Core.m_inputProvider.AttackType != 0)
            {
                foreach (var obj in IMetaAI<ProximityCreateWindow>.Instance.ObjectList)
                {
                    if (obj.IsCreate == true)
                    {
                        Core.m_animator.SetTrigger("ProximityCreateWindow");
                        return;
                    }
                }

                //走り状態でなければ通常攻撃へ、走り状態ならダッシュ攻撃へ（山本）
                if (Core.m_isRun == false)
                {
                    Core.m_animator.SetTrigger("Attack");
                }
                else
                {
                    Core.m_animator.SetTrigger("DashAttack");
                }

                //武器消失時にイベントを再生し武器を出現する
                Core.PlayerParameters.AppearWeapon(Core.m_animator);
            }

            //回避ボタン押したら、回避に遷移（山本）
            if (Core.m_inputProvider.DoRolling)
            {
                // スタミナが消費できるか確認
                if (Core.m_characterStatus.m_stamina.Value >= Core.m_characterStatus.m_rollingStaminaCost)
                {
                    Core.m_animator.SetTrigger("Rolling");
                }
            }

            //童話スキルの消費BPが足りているときに詠唱状態へと移行（山本）
            if (
                (Core.m_inputProvider.UseStorySkill_1 && Core.PlayerParameters.UseSkill1Flg) ||
                (Core.m_inputProvider.UseStorySkill_2 && Core.PlayerParameters.UseSkill2Flg)
                )
            {


                Core.m_animator.SetTrigger("StorySkill");
                //武器出現時にイベントを再生し武器を消失する
                Core.PlayerParameters.HideWeapon(Core.m_animator);

                //スキル1、スキル2どちらが使用されたか確認する
                if (Core.m_inputProvider.UseStorySkill_1 && Core.PlayerParameters.UseSkill1Flg)
                {

                    Core.PlayerParameters.TriggerStorySkill_1 = true;
                }
                else if (Core.m_inputProvider.UseStorySkill_2 && Core.PlayerParameters.UseSkill2Flg)
                {

                    Core.PlayerParameters.TriggerStorySkill_2 = true;
                }

                return;

            }



            // 経営シーン更新(上甲)
            ManagementScenePlayerUpdate();
        }

        //! @brief 経営シーンのプレイヤーの更新処理(上甲)
        //! @todo 特定シーンでのみ実行される処理をデバッグ用に無効化しているため、追々修正
        private void ManagementScenePlayerUpdate()
        {
            //// 現在のシーン確認
            //if (SceneNameManager.instance.CurrentSceneName != "ManagementScene")
            //{
            //    return;
            //}

            if (IMetaAI<BaseAssignEventObject>.Instance == null)
            {
                return;
            }

            foreach (var eventObj in IMetaAI<BaseAssignEventObject>.Instance.ObjectList)
            {
                if (eventObj == null)
                {
                    continue;
                }
                // 接触していなければ処理は行わない
                if (!eventObj.Collided)
                {
                    continue;
                }
                if (eventObj.IsAccessed(ref Core.m_inputProvider))
                {
                    eventObj.OnCollisionAccesesEvent();
                    var triggerName = eventObj.AnimatorTriggerName;
                    if (triggerName != null)
                    {
                        Core.m_animator.SetTrigger(triggerName);
                        return;
                    }
                }
            }
        }

        //新規追加:Input情報をtriggerで管理するときはここに書く（山本）
        public override void OnUpdate()
        {
            base.OnUpdate();

            switch (Core.GroupNo)
            {
                case CharacterGroupNumber.player:
                    {
                        PlayerUpdate(Core.m_inputProvider.MoveVector);
                    }
                    break;
            }
        }


        public override void OnFixedUpdate()
        {
            switch (Core.m_inputType)
            {
                case InputTypes.Player:
                    {
                        var input = Core.m_inputProvider.MoveVector;

                        // 方向キーの入力があったら、その向きにキャラクターを回転させる
                        if (input.magnitude != 0)
                        {
                            Core.SetRotateToTarget(Core.m_inputProvider.MoveVector);
                            Core.SetMoveVec(Core.m_inputProvider.MoveVector);
                        }

                        // 移動速度の計算
                        float speedByStick = input.magnitude;
                        Core.PlayerParameters.SpeedStick = speedByStick;

                        float finalSpeed;
                        if (Core.m_isRun) finalSpeed = Core.m_dushSpeed;
                        else finalSpeed = Core.m_walkSpeed * speedByStick;
                        Core.Move(finalSpeed);

                        // 移動音速度調整(上甲) todo 仕様変更につき一時速度調整なし
                        // m_moveSound.SetPlaybackRatio(finalSpeed / Core.m_walkSpeed);

                        // スタミナ回復（吉田）
                        UpdateStamina();
                        //BP回復(山本)
                        UpdateBP();
                    }
                    break;

                case InputTypes.Enemy:

                case InputTypes.Management:
                    {
                        //リグ存在し、Weightが0でなければリグを働かせないようにする（山本）
                        if (Core.EnemyParameters?.m_rig)
                        {
                            if (Core.EnemyParameters.m_rig.weight != 0.0f)
                                Core.EnemyParameters.m_rig.weight = 0.0f;
                        }

                        if (!Core.m_inputProvider.IsArrive)
                        {
                            Core.SetMoveVec(Core.m_inputProvider.MoveVector);
                            if (Core.m_inputProvider.LookVector.magnitude != 0.0f)
                            {
                                Core.SetRotateToTarget(Core.m_inputProvider.LookVector);
                            }
                            else
                            {
                                Core.SetRotateToTarget(Core.m_inputProvider.MoveVector);
                            }

                            // 接敵中なら急ぐ　伊波
                            if (Core.EnemyParameters?.Arbor)
                            {
                                Transform target = Core.EnemyParameters.Arbor.parameterContainer.GetTransform("Target");
                                if (target)
                                {
                                    Core.Move(Core.m_dushSpeed);
                                }
                                else
                                {
                                    Core.Move(Core.m_walkSpeed);
                                }
                            }
                            else
                            {
                                Core.Move(Core.m_walkSpeed);
                            }
                        }
                        else
                        {
                            if (Core.m_inputProvider.LookVector.magnitude != 0.0f)
                            {
                                Core.SetRotateToTarget(Core.m_inputProvider.LookVector);
                            }
                            else
                            {
                                Core.SetRotateToTarget(Core.m_inputProvider.Destination - Core.transform.position);
                            }
                            Core.Move(0f);
                        }
                    }
                    break;

                case InputTypes.SkillChara:
                    {
                        if (!Core.m_inputProvider.IsArrive)
                        {

                            Core.SetMoveVec(Core.m_inputProvider.MoveVector);
                            if (Core.m_inputProvider.LookVector.magnitude != 0.0f)
                            {
                                Core.SetRotateToTarget(Core.m_inputProvider.LookVector);
                            }
                            else
                            {
                                Core.SetRotateToTarget(Core.m_inputProvider.MoveVector);
                            }

                            float playerDist = CheckPlayerDistance();

                            //童話スキルキャラが走る
                            if (playerDist >= Core.PlayerSkillsParameters.RunDist)
                            {
                                Core.Move(Core.m_dushSpeed);
                            }
                            else if (playerDist < Core.PlayerSkillsParameters.WalkDist)
                            {

                                float walkSpeed = Core.m_walkSpeed * m_speedStick;
                                Core.Move(walkSpeed);
                            }
                            else
                            {
                                if (!m_isRun)
                                {
                                    Core.Move(Core.m_walkSpeed);
                                }
                                else
                                {
                                    float dushSpeed = Core.m_walkSpeed * 2.0f;
                                    Core.Move(dushSpeed);
                                }
                            }

                        }
                        else
                        {
                            Core.SetRotateToTarget(Core.m_inputProvider.Destination - Core.transform.position);
                            Core.Move(0f);
                        }

                    }
                    break;

            }
        }

        // 採取可能状態にあるアイテムを探す（吉田）
        private bool SearchGatheringItem()
        {
            foreach (var item in IMetaAI<AssignItemID>.Instance.ObjectList)
            {
                GetItemObject getItemObject = item.GetComponent<GetItemObject>();
                if (getItemObject == null) { continue; }

                if (getItemObject.IsGet)
                {
                    Core.PlayerParameters.m_ableGatheringItem = item;
                    return true;
                }
            }
            return false;
        }

        //プレイヤーとの距離測定
        private float CheckPlayerDistance()
        {
            foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (chara.tag == "Player")
                {
                    var dist = (chara.transform.position - Core.transform.position).magnitude;

                    m_speedStick = chara.PlayerParameters.SpeedStick;
                    m_isRun = chara.m_isRun;

                    return dist;
                }
            }

            return 0.0f;
        }

        // スタミナ回復（吉田）
        private void UpdateStamina()
        {
            // ダッシュ時 ---------------
            if (Core.m_isRun)
            {
                // スタミナ消費
                Core.m_characterStatus.m_stamina.Value -= Core.m_characterStatus.m_dashStaminaCost;
                if (Core.m_characterStatus.m_stamina.Value < 0)
                {
                    //スタミナなくなった
                    Core.m_characterStatus.m_stamina.Value = 0;
                    if (Core.m_dashEffect != null)
                    {
                        Core.m_dashEffect.Stop();
                        Core.m_isNoStamina = true;
                    }
                    Core.m_isRun = false;
                }

                return;// ダッシュ時は回復しない
            }


            // 回復 ---------------
            if (Core.m_characterStatus.m_stamina.Value >= Core.m_characterStatus.MaxStamina) return;

            Core.m_characterStatus.m_stamina.Value += Core.m_characterStatus.m_staminaSpeed * Time.deltaTime;

            if (Core.m_characterStatus.m_stamina.Value >= Core.m_characterStatus.MaxStamina)
            {
                Core.m_characterStatus.m_stamina.Value = Core.m_characterStatus.MaxStamina;

            }
        }


        //BPを時間経過ごとに回復
        private void UpdateBP()
        {
            //童話スキル１が使用できるかチェック（山本）
            Core.PlayerParameters.UseSkill1Flg = CheckStorySkill
            (
                0,
               Core.PlayerParameters.StorySkill1_ID,
               Core.PlayerParameters.ObserbSkill1
             );

            //童話スキル2が使用できるかチェック（山本）
            Core.PlayerParameters.UseSkill2Flg = CheckStorySkill
           (
                1,
               Core.PlayerParameters.StorySkill2_ID,
               Core.PlayerParameters.ObserbSkill2

            );

            //それぞれのBPを回復する(吉田)
            {
                //スキルBPを回復する(吉田)
                if (Core.PlayerParameters.ObserbSkill1 == null)
                    Core.m_characterStatus.m_bpSkill_1.Value += Core.m_characterStatus.m_bpRecoverSpeed * Time.deltaTime;

                if (Core.PlayerParameters.ObserbSkill2 == null)
                    Core.m_characterStatus.m_bpSkill_2.Value += Core.m_characterStatus.m_bpRecoverSpeed * Time.deltaTime;

                //Max超えてたらMaxに戻す（吉田）
                if (Core.m_characterStatus.m_bpSkill_1.Value >= Core.m_characterStatus.MaxBPSkill_1)
                {
                    Core.m_characterStatus.m_bpSkill_1.Value = Core.m_characterStatus.MaxBPSkill_1;
                }
                if (Core.m_characterStatus.m_bpSkill_2.Value >= Core.m_characterStatus.MaxBPSkill_2)
                {
                    Core.m_characterStatus.m_bpSkill_2.Value = Core.m_characterStatus.MaxBPSkill_2;
                }

            }


            //if (Core.m_characterStatus.m_bp.Value >= Core.m_characterStatus.MaxBP)
            //{
            //    return;
            //}

            //Core.m_characterStatus.m_bp.Value += Core.m_characterStatus.m_bpRecoverSpeed * Time.deltaTime;


            //if (Core.m_characterStatus.m_bp.Value >= Core.m_characterStatus.MaxBP)
            //{
            //    Core.m_characterStatus.m_bp.Value = Core.m_characterStatus.MaxBP;
            //}

        }


        //スキルが使用できるか確認する処理
        public bool CheckStorySkill(int _slot, StorySkill_ID _skillID, GameObject _obserbSkillObj)
        {
            bool useSkillFlg = false;

            //童話スキル装備してないならfalse
            if (_skillID == StorySkill_ID.None)
            {
                return false;
            }


            if (_obserbSkillObj == null)
            {

                var storySkillData = StorySkillDataBaseManager.instance.GetStorySkillData(_skillID);

                //プレイヤーのBPがデータに登録された消費BP以上ならスキル使用可能（吉田）
                if (_slot == 0 && storySkillData.PayBP <= Core.m_characterStatus.m_bpSkill_1.Value)
                {
                    useSkillFlg = true;
                }
                else if (_slot == 1 && storySkillData.PayBP <= Core.m_characterStatus.m_bpSkill_2.Value)
                {
                    useSkillFlg = true;
                }


            }

            return useSkillFlg;

        }


    }


    [System.Serializable]
    [AddTypeMenu("Common/Attack")]
    // 攻撃ステート中の処理(倉田)
    // 攻撃時プレイヤーが一歩前進する（吉田）
    // 攻撃時プレイヤーが敵の方向を向く（吉田）
    public class ActionState_Attack : ActionState_Base
    {
        private Transform m_nearestTargetTrans = null;

        [Header("ノックバックしないかどうか")]
        [SerializeField] private bool m_isNotKnockBack;

        [Header("プレイヤー用　敵の索敵角度")]
        [SerializeField] private float m_targettingAngle = 360.0f;

        public override void OnEnter()
        {
            base.OnEnter();
            /*
            // 前進処理の変数 初期化
            m_forwordStepTimer = 0.0f;
            m_forwordStepSpeed = m_forwordStepStartSpeed;
            */
            // 一番近い敵を探す→transformを保持

            //回転対象のターゲットを初期化(山本)
            m_nearestTargetTrans = null;

            Core.m_isNoKnockBack = m_isNotKnockBack;

            switch (Core.GroupNo)
            {
                case CharacterGroupNumber.player:
                    {
                        float nearestDist = -1.0f;
                        foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
                        {
                            //敵と迷惑客の時通る（山本）
                            if ((chara.GroupNo == CharacterGroupNumber.enemy) ||
                                (chara.GroupNo == CharacterGroupNumber.gangster))
                            {
                                //HP0の敵にはむかないようにする(山本)
                                if (chara.Status.m_hp.Value <= 0.0f) continue;

                                float dist = Vector3.Distance(
                                    Core.transform.position, chara.transform.position);

                                //設定範囲内に敵がいるかのサーチ(山本)
                                var charaContoroller = chara.GetComponent<KinematicCharacterMotor>();
                                if (charaContoroller == null) { continue; }
                                //敵のカプセルコライダーの半径も加味してサーチする
                                if (dist >= Core.PlayerParameters.SearchEnemyDist + charaContoroller.Capsule.radius) { continue; }

                                // 敵がいる角度を考慮する
                                float angle;
                                angle = Vector3.Angle(Core.transform.forward, chara.transform.position - Core.transform.position);
                                if (angle > m_targettingAngle * 0.5f) continue;


                                if (dist < nearestDist || nearestDist < 0)
                                {
                                    nearestDist = dist - charaContoroller.Capsule.radius;
                                    m_nearestTargetTrans = chara.transform;
                                }
                            }

                        }

                    }
                    break;
            }
        }

        //山本追加
        public override void OnExit()
        {
            switch (Core.GroupNo)
            {
                case CharacterGroupNumber.player:
                    {
                        //連打しまくるとトリガーが残るので、トリガーを消す
                        Core.m_animator.ResetTrigger("Attack");
                    }
                    break;
            }
        }


        public override void OnUpdate()
        {
            base.OnUpdate();

            switch (Core.GroupNo)
            {
                case CharacterGroupNumber.player:
                    {

                        //回避ボタン押したら、回避にすぐに遷移（山本）
                        if (Core.m_inputProvider.DoRolling)
                        {
                            // スタミナが消費できるか確認
                            if (Core.m_characterStatus.m_stamina.Value >= Core.m_characterStatus.m_rollingStaminaCost)
                            {
                                Core.m_animator.SetTrigger("Rolling");
                            }
                        }


                        // 攻撃ボタン押したら、連続攻撃
                        if (Core.m_inputProvider.AttackType != 0)
                        {
                            Core.m_animator.SetTrigger("Attack");
                        }


                    }
                    break;
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            // 前進処理
            //ForwordStep();
            Core.m_charaCtrl.MoveSpeed = 0.0f;

            switch (Core.m_inputType)
            {
                case InputTypes.Player:
                    {
                        // 敵の方向を向く
                        LookAtEnemy();
                    }
                    break;
            }
        }

        /*
        // 一歩全身する（吉田）
        private void ForwordStep()
        {
            // アニメーションの情報を取得
            var animeState = Core.m_animator.GetCurrentAnimatorStateInfo(0);
            if (animeState.IsName("Move")) return;// Moveアニメーション中（アニメーション補間中）は前進しない

            m_forwordStepTimer += Time.fixedDeltaTime;
            if (m_forwordStepTimer < m_forwordStepStartTime) return;
            if (m_forwordStepTimer > m_forwordStepStopTime) return;

            // キャラクターを一歩前進させる
            m_forwordStepSpeed *= m_forwordStepAccel;
            Core.Move(m_forwordStepSpeed);
            Core.m_charaCtrl.MoveSpeed = m_forwordStepSpeed;
        }
        */

        // 一番近い敵の方向を向く（吉田）
        private void LookAtEnemy()
        {
            if (m_nearestTargetTrans == null) return;
            Core.SetRotateToTarget(m_nearestTargetTrans.position - Core.transform.position);
        }
    }



    [System.Serializable]
    [AddTypeMenu("Common/Dead")]
    public class ActionState_Dead : ActionState_Base
    {
        private ArborFSM m_arborFSM;

        [Header("死亡時ノックバックの強さ")]
        [SerializeField] private float m_knockBackPower = 0.0f;

        public override void Initialize(AnimatorStateMachine stateMachine)
        {
            base.Initialize(stateMachine);
            switch (Core.m_inputType)
            {
                case InputTypes.Enemy:
                    {
                        m_arborFSM = Core.gameObject.GetComponent<ArborFSM>();
                    }
                    break;
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            switch (Core.m_inputType)
            {
                case InputTypes.Player:
                    {
                        CutSceneManager.instance.PlayCutScene(CutSceneNumber.gameOver);
                        Core.PlayerParameters.HideWeapon(Core.m_animator);
                    }
                    break;

                case InputTypes.Enemy:
                    {
                        // Arborで変な処理が動かないように止める
                        m_arborFSM.Stop();
                    }
                    break;

                default:
                    {
                    }
                    break;

            }
            Core.m_charaCtrl.AddVelocity(Core.m_knockBackVec * m_knockBackPower);
            Core.m_charaCtrl.Jump(m_knockBackPower);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Core.Move(0.0f);

        }
    }


    [System.Serializable]
    [AddTypeMenu("Common/KnockBack")]
    public class ActionState_KnockBack : ActionState_Base
    {
        [Header("ノックバックの強さ")]
        [SerializeField] private float m_knockBackPower = 5.0f;
        public override void OnExit()
        {
            base.OnExit();
            Core.Move(0.0f);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Core.m_charaCtrl.AddVelocity(Core.m_knockBackVec * m_knockBackPower * Core.m_knockBackMultiplier);
        }
    }

    [System.Serializable]
    [AddTypeMenu("Common/BlowAway")]
    public class ActionState_BlowAway : ActionState_Base
    {
        [Header("ノックバックの強さ")]
        [SerializeField] private float m_knockBackPower = 5.0f;
        public override void OnExit()
        {
            base.OnExit();
            Core.Move(0.0f);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Core.m_charaCtrl.AddVelocity(Core.m_knockBackVec * m_knockBackPower * Core.m_knockBackMultiplier);
            Core.SetRotateToTarget(-Core.m_knockBackVec);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Core.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f)
            {
                Core.SetRotateToTarget(-Core.m_knockBackVec);
            }
        }
    }


    //登場時のジャンピング処理(山本)
    [System.Serializable]
    [AddTypeMenu("Common/Jumping")]
    public class ActionState_Jumping : ActionState_Base
    {
        private Vector3 m_nockBackVec = new Vector3(1, 0, 0);
        public override void OnExit()
        {
            base.OnExit();
            Core.Move(0.0f);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if(Core.PlayerParameters)
            {
                Core.PlayerParameters.HideWeapon(Core.m_animator);
            }

            Core.SetRotateToTarget(Core.m_knockBackVec);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            var playerDir = m_nockBackVec;
            var v = Vector3.RotateTowards(Core.transform.forward, playerDir, 10000.0f, 0);
            Core.SetRotateToTarget(v);
        }


    }

}