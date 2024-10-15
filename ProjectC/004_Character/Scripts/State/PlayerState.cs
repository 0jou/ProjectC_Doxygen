using KinematicCharacterController;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

using Cysharp.Threading.Tasks;
using System;
using UnityEngine.UIElements;
using UnityEditor;


// キャラクターの基盤クラス
public partial class CharacterCore : MonoBehaviour, IDamageable
{

    // 回避用ステート(倉田　側だけ作った)
    //中身作成(山本)
    [System.Serializable]
    [AddTypeMenu("Player/Rolling")]
    public class ActionState_Rolling : ActionState_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Core.m_animator.SetFloat("RollingSpeed", Core.PlayerParameters.m_rollingAnimeSpeed);
            Core.m_isNoDamage = true;

            Core.m_characterStatus.m_stamina.Value -= Core.m_characterStatus.m_rollingStaminaCost;

            // 回避SE 上甲
            SoundManager.Instance.StartPlayback("Rolling");

        }

        public override void OnUpdate()
        {
            var input = Core.m_inputProvider.MoveVector;

            //方向キーの入力があったら、その向きにキャラクターを回転させる（回避時すぐにキーの方向に向く処理）
            if (input.magnitude != 0)
            {
                //var playerDir = new Vector3(input.x, 0, input.y);
                var v = Vector3.RotateTowards(Core.transform.forward, input, 10000.0f, 0);
                Core.SetRotateToTarget(v);
            }
        }

        public override void OnFixedUpdate()
        {
            AnimatorStateInfo stateInfo = Core.m_animator.GetCurrentAnimatorStateInfo(0);
            Core.Move(Core.PlayerParameters.m_rollingPow / stateInfo.length);
        }

        public override void OnExit()
        {
            Core.m_isNoDamage = false;
        }
    }


    //童話スキル詠唱状態(山本)
    [System.Serializable]
    [AddTypeMenu("Player/UseStorySkill")]
    public class ActionState_UseStorySkill : ActionState_Base
    {
        [SerializeField] float m_playerDist = -3.0f;
        [SerializeField] float m_rayStartHeisght = 20.0f;

        GameObject m_gameObject;

        //緊急用
        //詠唱中にキャンセルされるとフラグON
        bool m_isCanserFlg = false;

        //童話スキルのデータ
        StorySkillData m_storySkillData;

        //童話スキルの現在のキャストタイム(値が1秒ごとに1引かれていき、0になったらキャスト完了)
        private float m_castTime = 0.0f;

        //童話スキルの設定キャストタイム（データベース上で設定したキャストタイムを保存する変数）
        private float m_maxCastTime = 0.0f;

        public override void OnEnter()
        {
            base.OnEnter();

            //キャストタイム初期化
            m_castTime = 0.0f;
            m_maxCastTime = 0.0f;
            //スキルパラメーター側のキャストタイムを初期化
            Core.PlayerParameters.CastTimeProgress.Value = 0.0f;

            //falseで初期化
            m_isCanserFlg = false;

            //StorySkill初期化
            m_storySkillData = ScriptableObject.CreateInstance<StorySkillData>();

            GameObject spellEffect = null;

            //第1スキルが使用されたか第２スキルが使用されたかチェック
            if (Core.PlayerParameters.TriggerStorySkill_1)
            {
                //対応の詠唱エフェクト発動
                m_storySkillData = StorySkillDataBaseManager.instance.GetStorySkillData
                    (Core.PlayerParameters.StorySkill1_ID);


            }
            else if (Core.PlayerParameters.TriggerStorySkill_2)
            {
                m_storySkillData = StorySkillDataBaseManager.instance.GetStorySkillData
                    (Core.PlayerParameters.StorySkill2_ID);
            }

            if (m_storySkillData == null)
            {
                return;
            }

            //データベースからキャストタイムを取得しセット
            m_castTime = m_storySkillData.CastTime;
            m_maxCastTime = m_castTime;


            //UI側に詠唱状態だということを伝える処理



            //データベースから召喚エフェクトを取得
            spellEffect = m_storySkillData.SpellEffectPrefab;

            if (spellEffect)
            {
                //詠唱エフェクト出現と記憶
                m_gameObject = Instantiate(spellEffect, Core.transform);
                if(m_gameObject.TryGetComponent(out EffectController effectController))
                {
                    effectController.EndEffectTime = m_storySkillData.CastTime;
                }
            }


        }

        public override void OnUpdate()
        {
            //回避ボタン押したら、回避にすぐに遷移（山本)
            //詠唱中にローリング状態へと移行するように
            if (Core.m_inputProvider.DoRolling)
            {
                // スタミナが消費できるか確認
                if (Core.m_characterStatus.m_stamina.Value >= Core.m_characterStatus.m_rollingStaminaCost)
                {
                    Core.m_animator.SetTrigger("Rolling");
                    m_isCanserFlg = true;
                }
            }

            //キャストタイムが０以下になったら詠唱完了しアニメーション移動
            if (m_castTime <= 0.0f)
            {
                Core.m_animator.SetTrigger("CastingEnd");
            }
            else
            {
                m_castTime -= Time.deltaTime;

                //CharacterCoreのキャストタイムの進捗度に値をセット
                float progress = 1.0f - (m_castTime / m_maxCastTime);
                progress = Math.Clamp(progress, 0.0f, 1.0f);
                Core.PlayerParameters.CastTimeProgress.Value = progress;
            }

        }

        public override void OnFixedUpdate()
        {
            Core.Move(0.0f);
        }

        public override void OnExit()
        {
            //m_isCanserFlgがONでなければ=回避されなければ⇒詠唱完了!!（山本）
            if (m_isCanserFlg == false)
            {
                //童話スキル発動（プレハブ生成）

                //童話スキルのデータなければリターン
                if (m_storySkillData == null)
                {
                    Debug.LogError("童話スキルデータが空でした。");
                    return;
                }

                Vector3 rayHitPos = Vector3.zero;

                //レイの開始位置セット(童話スキルプレハブの場所設定)
                Quaternion quo = Quaternion.LookRotation(Core.transform.forward);
                Vector3 rayStartPos = Core.transform.position
                    + Core.transform.forward * m_storySkillData.Distance
                    + Vector3.up * m_rayStartHeisght;

                if (RayHitPosition(rayStartPos, out RaycastHit hit))
                {
                    rayHitPos = hit.point;
                }
                else
                {
                    Debug.LogError("設置場所が見つかりませんでした。");
                    return;
                }

               

                //データに対応した童話スキルのプレハブ生成
                var skill = Instantiate(m_storySkillData.StorySkillPrefab, rayHitPos, quo);


                //ChildrenMenberコンポーネントあればSetPosition呼ぶ
                //(子オブジェクトに位置座標を指定する際に使用するコンポーネント)
                if (skill.TryGetComponent(out ChildrenMember children))
                {
                    children.SetPosition();

                }

                //SetPositionCharacterControllerコンポーネントがあれば呼ぶ
                //（特定のCharacterContorllerに位置座標を設定するコンポーネント）
                if (skill.TryGetComponent(out SetPositionCharacterController setPosition))
                {
                    setPosition.SetCharacterCore();
                }


                //監視用変数に代入(Skill1、Skill2で分ける)
                if (Core.PlayerParameters.TriggerStorySkill_1)
                {
                    if (Core.PlayerParameters.ObserbSkill1 == null)
                    {
                        Core.PlayerParameters.ObserbSkill1 = skill;
                    }

                    // StorySkill_1のBP消費
                    Core.m_characterStatus.m_bpSkill_1.Value -= m_storySkillData.PayBP;
                }
                else if (Core.PlayerParameters.TriggerStorySkill_2)
                {
                    if (Core.PlayerParameters.ObserbSkill2 == null)
                    {
                        Core.PlayerParameters.ObserbSkill2 = skill;
                    }

                    // StorySkill_2のBP消費
                    Core.m_characterStatus.m_bpSkill_2.Value -= m_storySkillData.PayBP;
                }

                //詠唱用のエフェクトの破壊
                //Destroy(m_gameObject);

                Debug.Log("童話スキルプレハブ生成");

                //武器消失時にイベントを再生し武器を出現する
                Core.PlayerParameters.AppearWeapon(Core.m_animator);

                //タイムライン再生あれば再生
                if (skill.TryGetComponent(out CharacterCore characterCore))
                {
                    characterCore.PlayerSkillsParameters.StorySkillAppear();

                }


            }
            else
            {
                if (m_gameObject)
                {
                    //詠唱用のエフェクトの破壊
                    Destroy(m_gameObject);
                }
            }

            //どちらのスキルを使用したのかのフラグ（TriggerStorySkillをfalseにセット）
            if (Core.PlayerParameters.TriggerStorySkill_1)
            {
                Core.PlayerParameters.TriggerStorySkill_1 = false;
            }

            if (Core.PlayerParameters.TriggerStorySkill_2)
            {
                Core.PlayerParameters.TriggerStorySkill_2 = false;
            }


        }

        //レイを飛ばして場所を童話スキルを設置する場所特定する
        private bool RayHitPosition(Vector3 _startPos, out RaycastHit _hit)
        {
            // 方向
            Vector3 dir = Vector3.down;

            // レイ作成
            var ray = new Ray(_startPos, dir);

            int layermask = 1 << 32;

            if (Physics.Raycast(ray, out _hit, 100.0f, layermask))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    // 採取用ステート(吉田)
    [System.Serializable]
    [AddTypeMenu("Player/Gathering")]
    public class ActionState_Gathering : ActionState_Base
    {
        AssignItemID m_gatheringItem = null;

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("採取開始");
            m_gatheringItem = Core.PlayerParameters.m_ableGatheringItem;
        }

        public override void OnFixedUpdate()
        {
            Core.Move(0.0f);
        }

        public override void OnExit()
        {
            if (m_gatheringItem != null)
            {
                GetItemObject getItemObject = m_gatheringItem.GetComponent<GetItemObject>();
                if (getItemObject == null)
                {
                    Debug.LogError("GetItemObjectコンポーネントが存在しません");
                    return;
                }
                getItemObject.GetItem();
            }
            Debug.Log("採取終了");
        }
    }

    // 設置用ステート(吉田)
    [System.Serializable]
    [AddTypeMenu("Player/PutItem")]
    public class ActionState_PutItem : ActionState_Base
    {
        private PutItemInstance m_putItemInstance = null;

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("設置開始");

            // 設置設定（アニメーション中に変更されないように）
            m_putItemInstance = Core.AddComponent<PutItemInstance>();
            m_putItemInstance.SetItemID(Core.PlayerParameters.m_putItemInfo.m_itemTypeID, Core.PlayerParameters.m_putItemInfo.m_itemID);

            // 追加　上甲　効果音
            SoundManager.Instance.StartPlayback("PutItem");
        }

        public override void OnFixedUpdate()
        {
            //移動量を減らす
            Core.Move(0.0f);
        }

        public override void OnExit()
        {
            if (m_putItemInstance != null)
            {
                m_putItemInstance.Craft();
                Destroy(m_putItemInstance);
            }

            // 未設定状態にする
            Core.PlayerParameters.m_putItemInfo.m_itemID = 0;

            //フラグをfalseに（山本）
            Core.m_animator.SetBool("IsPutItem", false);

            // UIの表示
            Core.PlayerParameters.AddActionUIState(ActionUIController.ActionUIState.Attack);


            // Debug.Log("設置終了");
        }
    }

    // 投げる準備用ステート(吉田)
    [System.Serializable]
    [AddTypeMenu("Player/ReadyToThrow")]
    public class ActionState_ReadyToThrow : ActionState_Base
    {
        // 放物線描画用
        DrawThrowItemArc m_drawArc = null;

        [SerializeField]
        float m_playerMoveSpeed = 0.1f;

        [SerializeField, Header("到着時間　放物線の高さが決まる")]
        float m_arrivalTime = 1.0f;

        private bool m_cancelFlg = false;

        // 放物線計算用 ----------------------------------------------

        // カメラ
        Camera m_mainCamera = null;

        GameObject m_lunchObj = null;// 空のオブジェクト

        GameObject m_mousePosObj = null;// マウスの位置を示すオブジェクト

        AimCameraController m_aimCameraController = null;

        // ---------------------------------------------------------

        public override void OnEnter()
        {
            base.OnEnter();
            Core.m_animator.SetFloat("Speed", 1);

            // 放物線描画用の初期化
            m_drawArc = Core.AddComponent<DrawThrowItemArc>();

            // 放物線計算用の初期化
            m_mainCamera = Camera.main;
            m_lunchObj = new GameObject("LunchObj");

            // マウスの位置を示すオブジェクトの初期化
            m_mousePosObj = Instantiate(Core.PlayerParameters.m_mouseThrowAim);
            m_mousePosObj.name = "MousePos";
            var data = ItemDataBaseManager.instance.GetItemData<FoodData>(
                Core.PlayerParameters.m_putItemInfo.m_itemTypeID, Core.PlayerParameters.m_putItemInfo.m_itemID);
            if (data != null)
            {
                m_mousePosObj.transform.localScale =
                    new Vector3(data.ThrowRange, m_mousePosObj.transform.localScale.y, data.ThrowRange);
            }


            //カメラの切り替え
            SwitchCamera(true);

            if (Core.PlayerParameters.m_throwAimCamera != null)
            {
                m_aimCameraController =
                    Core.PlayerParameters.m_throwAimCamera.GetComponent<AimCameraController>();
            }

            // UIの表示
            Core.PlayerParameters.AddActionUIState(ActionUIController.ActionUIState.ReadyToThrow);
            Core.PlayerParameters.AddAnyActionUIState(ActionUIController.ActionUIState.ReadyToThrow,0.0f);

        }

        public override void OnUpdate()
        {
            //投げ状態を解除（山本）
            if (Core.m_inputProvider.Cancel)
            {
                Core.m_animator.SetTrigger("CancelThrow");
                m_cancelFlg = true;
                return;
            }

            if (PlayerInputManager.instance.IsInputActionWasReleased(InputActionMapTypes.UI, "ThrowItem"))
            {
                Core.m_animator.SetTrigger("ThrowItem");
                Core.m_animator.SetFloat("Speed", 1);

            }


            if (!HitRay(out RaycastHit raycastHit))
            {
                m_drawArc.ShowArc = false;
                m_mousePosObj.SetActive(false);
                return;
            }

            // レイが当たっている場合 --------------------------

            m_drawArc.ShowArc = true;
            m_mousePosObj.SetActive(true);

            CalculateThrowPower(
                _startpos: Core.PlayerParameters.m_handTrans.position,
                _endPos: raycastHit.point);
            m_drawArc.ThrowPower = Core.PlayerParameters.m_throwPower;
            m_drawArc.StartPos = Core.PlayerParameters.m_handTrans.position;
            m_mousePosObj.transform.position = raycastHit.point;
            m_lunchObj.transform.position = Core.PlayerParameters.m_handTrans.position;
        }

        public override void OnFixedUpdate()
        {
            // プレイヤーの移動
            MovePlayer();
            RotatePlayer();
        }

        public override void OnExit()
        {
            if (m_drawArc != null)
            {
                Destroy(m_drawArc);
            }

            Destroy(m_lunchObj);
            Destroy(m_mousePosObj);

            SwitchCamera(_aimCamera: false);

            // UI非表示
            Core.PlayerParameters.RemoveActionUIState(ActionUIController.ActionUIState.ReadyToThrow);
            Core.PlayerParameters.RemoveAnyActionUIState(ActionUIController.ActionUIState.ReadyToThrow);

            //キャンセルされたら
            if (m_cancelFlg)
            {
                Core.m_animator.SetBool("IsThrow", false);
                m_cancelFlg = false;
            }

        }

        private void MovePlayer()
        {
            var input = Core.m_inputProvider.MoveVector;

            if (input.magnitude != 0.0f)
            {
                Core.SetMoveVec(input);
            }
            Core.Move(input.magnitude * m_playerMoveSpeed);
        }

        private void RotatePlayer()
        {
            var horizontalRotation = Quaternion.AngleAxis(
                m_aimCameraController.Horizontal.Value, Vector3.up);

            Vector3 dir = horizontalRotation * Vector3.forward;
            Core.SetRotateToTarget(dir);
        }

        private void CalculateThrowPower(Vector3 _startpos, Vector3 _endPos)
        {
            //　開始位置と到着位置の長さを計算
            var adjacent = Vector3.Distance(
                    _startpos,
                    _endPos);

            //　落下距離を計算
            //　0.5 * 重力 * 到達時間の2乗
            var opposite = Mathf.Abs(
                0.5f * Physics.gravity.y * m_arrivalTime * m_arrivalTime);

            //　到達点＋上向きに落下距離 放物線の頂点
            var upPos = _endPos + Vector3.up * opposite;

            //　斜辺の長さを計算
            var hypotenuse = Vector3.Distance(
                _startpos, upPos);

            //　角度を計算
            //　余弦定理
            float theta = -Mathf.Acos(
                (Mathf.Pow(hypotenuse, 2) + Mathf.Pow(adjacent, 2) - Mathf.Pow(opposite, 2))
                / (2 * hypotenuse * adjacent));

            //　横軸のspeedから斜め方向の速さを計算
            float hypotenuseSpeed = hypotenuse / m_arrivalTime;

            //　一旦攻撃対象の方を見る
            m_lunchObj.transform.LookAt(_endPos);
            ////　砲台の高さ向きを変える
            m_lunchObj.transform.Rotate(
                Vector3.right, theta * Mathf.Rad2Deg, Space.Self);

            Core.PlayerParameters.m_throwPower =
                m_lunchObj.transform.forward * hypotenuseSpeed;
        }

        private bool HitRay(out RaycastHit _hit)
        {
            var mouse = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
            var ray = m_mainCamera.ScreenPointToRay(mouse);
            return Physics.Raycast(ray, out _hit, Mathf.Infinity);
        }

        private void SwitchCamera(bool _aimCamera)
        {
            //カメラの切り替え
            if (Core.PlayerParameters.m_throwAimCamera != null && Core.PlayerParameters.m_playerfollowCamera != null)
            {
                Core.PlayerParameters.m_throwAimCamera.SetActive(_aimCamera);
                Core.PlayerParameters.m_playerfollowCamera.SetActive(!_aimCamera);
            }
        }
    }

    // 投げる用ステート(吉田)
    [System.Serializable]
    [AddTypeMenu("Player/ThrowItem")]
    public class ActionState_ThrowItem : ActionState_Base
    {
        ThrowItemInstance m_throwItemInstance = null;
        public override void OnEnter()
        {
            base.OnEnter();
            m_throwItemInstance = Core.AddComponent<ThrowItemInstance>();
            m_throwItemInstance.SetHandTrans(Core.PlayerParameters.m_handTrans);
            m_throwItemInstance.SetItemID(Core.PlayerParameters.m_putItemInfo.m_itemTypeID, Core.PlayerParameters.m_putItemInfo.m_itemID);
            m_throwItemInstance.ThrowPower = Core.PlayerParameters.m_throwPower;

            if (m_throwItemInstance != null)
            {
                m_throwItemInstance.Throw();

                Destroy(m_throwItemInstance);
            }

        }

        public override void OnUpdate()
        {
            //if (Core.m_animator.GetBool("IsThrow") == true)
            //{
            //    if (m_throwItemInstance != null)
            //    {
            //        m_throwItemInstance.Throw();

            //        Destroy(m_throwItemInstance);
            //    }
            //}
        }


        public override void OnExit()
        {
            if (m_throwItemInstance != null)
            {
                m_throwItemInstance.Throw();
                Destroy(m_throwItemInstance);
            }

            // 未設定状態にする
            Core.PlayerParameters.m_putItemInfo.m_itemID = 0;

            //falseにする
            Core.m_animator.SetBool("IsThrow", false);

        }
    }


    //武器しまい状態(山本)
    [System.Serializable]
    [AddTypeMenu("Player/SheathWeapon")]
    public class ActionState_SheathWeapon : CharacterCore.ActionState_Base
    {
        public override void OnExit()
        {
            if (Core.m_animator.GetBool("IsSheath") == false)
            {
                Core.m_animator.SetBool("IsSheath", true);
            }
        }
    }

    //武器持ち状態(山本)
    [System.Serializable]
    [AddTypeMenu("Player/UnSheathWeapon")]
    public class ActionState_UnSheathWeapon : CharacterCore.ActionState_Base
    {
        public override void OnExit()
        {
            if (Core.m_animator.GetBool("IsSheath") == true)
            {
                Core.m_animator.SetBool("IsSheath", false);
            }
        }

        // 設置用ステート(吉田)
        [System.Serializable]
        public class ActionState_ProximityCreateWindow : ActionState_Base
        {
            public override void OnEnter()
            {
                base.OnEnter();
                foreach (var obj in IMetaAI<ProximityCreateWindow>.Instance.ObjectList)
                {
                    if (obj.IsCreate == true)
                    {
                        obj.CreateWindow();
                    }
                }
            }
            public override void OnExit()
            {
                //foreach (var obj in IMetaAI<ProximityCreateWindow>.Instance.ObjectList)
                //{
                //    if (obj.IsCreate == true)
                //    {
                //        obj.CreateWindow();
                //    }
                //}
            }
        }
    }

    // アイテム持ち状態(山本)
    [System.Serializable]
    [AddTypeMenu("Player/HoldItem")]
    public class ActionState_HoldItem : ActionState_Base
    {
        ////アニメーションのExit時にIsHoldItemeフラグをfalseにしないようするフラグ
        //private bool m_isSkipFlg = false;
        //選択したアイテムを出現させるクラス
        private HoldItemInstance m_holdItemInstance = null;
        //食べられるかどうか
        private bool m_isEat = false;

        public override void OnEnter()
        {
            base.OnEnter();
            Core.m_animator.SetBool("IsHoldItem", true);

            //武器が存在する場合は消す
            Core.PlayerParameters.HideWeapon(Core.m_animator);

            //選択アイテムを出現させる
            m_holdItemInstance = Core.AddComponent<HoldItemInstance>();
            m_holdItemInstance.SetHoldPoint(Core.PlayerParameters.m_holdTrans);
            m_holdItemInstance.SetItemID(Core.PlayerParameters.m_putItemInfo.m_itemTypeID,
                                            Core.PlayerParameters.m_putItemInfo.m_itemID);
            m_holdItemInstance.Craft();

            //料理データにアクセスし、食べられものかどうかをチェック
            var data = ItemDataBaseManager.instance.GetItemData<FoodData>
                (Core.PlayerParameters.m_putItemInfo.m_itemTypeID,
                 Core.PlayerParameters.m_putItemInfo.m_itemID);

            m_isEat = data.IsEat;

            //// UIの非表示
            //Core.PlayerParameters.RemoveActionUIState(ActionUIController.ActionUIState.Attack);
            //// UIの表示
            //Core.PlayerParameters.AddActionUIState(ActionUIController.ActionUIState.HoldItem);
        }

        public override void OnUpdate()
        {
            //選択アイテムが変更されたらコンポーネントを変更
            if (m_holdItemInstance.ItemID != Core.PlayerParameters.m_putItemInfo.m_itemID)
            {
                m_holdItemInstance.DestroyPrefab();
                m_holdItemInstance.SetHoldPoint(Core.PlayerParameters.m_holdTrans);
                m_holdItemInstance.SetItemID(Core.PlayerParameters.m_putItemInfo.m_itemTypeID,
                                                Core.PlayerParameters.m_putItemInfo.m_itemID);
                m_holdItemInstance.Craft();

                //料理データにアクセスし、食べられものかどうかをチェック
                var data = ItemDataBaseManager.instance.GetItemData<FoodData>
                    (Core.PlayerParameters.m_putItemInfo.m_itemTypeID,
                     Core.PlayerParameters.m_putItemInfo.m_itemID);

                m_isEat = data.IsEat;

            }

            // 指定のボタンの入力で設置状態へ（吉田）
            if (Core.m_inputProvider.PutItem)
            {
                // アイテムの設定されていたらState変更可
                if (Core.PlayerParameters.m_putItemInfo.m_itemID != 0)
                {
                    Core.m_animator.SetTrigger("PutItem");

                    //// UI非表示
                    //Core.PlayerParameters.RemoveActionUIState(ActionUIController.ActionUIState.HoldItem);
                }
            }

            //指定のボタン入力かつ食べられる料理の場合、食事状態に移行
            if (Core.m_inputProvider.Eat && m_isEat)
            {
                Core.m_animator.SetTrigger("IsEat");

                //// UI非表示
                //Core.PlayerParameters.RemoveActionUIState(ActionUIController.ActionUIState.HoldItem);
            }

            // 指定のボタンの入力で投げ準備状態へ（吉田）
            if (Core.m_inputProvider.ReadyThrowItem)
            {
                // アイテムの設定されていたらState変更可
                if (Core.PlayerParameters.m_putItemInfo.m_itemID != 0)
                {
                    Core.m_animator.SetTrigger("ReadyToThrow");
                }
            }

            //キャンセルボタンを押すことでMoveに戻る
            if (Core.m_inputProvider.Cancel)
            {
                Core.m_animator.SetBool("IsHoldItem", false);

                //// UI非表示
                //Core.PlayerParameters.RemoveActionUIState(ActionUIController.ActionUIState.HoldItem);
                // UIの表示
                Core.PlayerParameters.AddActionUIState(ActionUIController.ActionUIState.Attack);

            }
        }

        public override void OnFixedUpdate()
        {
            //移動処理
            var input = Core.m_inputProvider.MoveVector;

            // 方向キーの入力があったら、その向きにキャラクターを回転させる
            if (input.magnitude != 0)
            {
                Core.SetRotateToTarget(input);
            }

            // 移動速度の計算
            float speedByStick = input.magnitude;
            float finalSpeed;
            if (Core.m_isRun) finalSpeed = Core.m_dushSpeed * speedByStick;
            else finalSpeed = Core.m_walkSpeed * speedByStick;

            Core.Move(finalSpeed);

        }


        public override void OnExit()
        {

            Core.m_animator.SetBool("IsHoldItem", false);

            //コンポーネントを削除
            if (m_holdItemInstance != null)
            {
                Destroy(m_holdItemInstance);
            }

        }


    }

    //アイテム食事状態(山本)
    [System.Serializable]
    [AddTypeMenu("Player/UseItem")]
    public class ActionState_UseItem : ActionState_Base
    {
        //料理食事コンポーネント
        private UseItemInstance m_useItemInstance;

        public override void OnEnter()
        {
            base.OnEnter();

            //アイテム使用
            m_useItemInstance = Core.AddComponent<UseItemInstance>();
            m_useItemInstance.SetItemID(Core.PlayerParameters.m_putItemInfo.m_itemTypeID,
                                        Core.PlayerParameters.m_putItemInfo.m_itemID);

            m_useItemInstance.UseItem();
        }

        public override void OnExit()
        {
            base.OnExit();
            if (m_useItemInstance)
            {
                Destroy(m_useItemInstance);
            }

            //フラグをfalseに（山本）
            Core.m_animator.SetBool("IsEatItem", false);

            // UIの表示
            Core.PlayerParameters.AddActionUIState(ActionUIController.ActionUIState.Attack);

        }
    }

    //復活状態（山本）
    [System.Serializable]
    [AddTypeMenu("Player/Continue")]
    public class ActionState_PlayerContinue : ActionState_Base
    {

        public override void OnEnter()
        {
            base.OnEnter();
            //var position = Core.PlayerParameters.PlayerRestartPosition;
            //Core.CharaCtrl.SetPositionMotor(position);


            var forward = Core.PlayerParameters.PlayerRestartForward;
            Vector3 v = Vector3.RotateTowards(Core.transform.forward, forward, 10000.0f, 0);
            Core.SetRotateToTarget(v);


            //体力を満タンで回復
            Core.Status.m_hp.Value = Core.Status.MaxHP;

            //コンティニュー用カットシーン再生
            CutSceneManager.instance.PlayCutScene(CutSceneNumber.reStart);

            //ノーダメージフラグ立てる
            Core.m_isNoDamage = true;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (CutSceneManager.instance.IsCutScenePlay() == false)
            {
                Core.m_animator.SetTrigger("RePlay");
            }

        }

        public override void OnExit()
        {
            base.OnExit();

            Core.m_isNoDamage = false;
        }


    }
}