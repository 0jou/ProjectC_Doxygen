using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Timeline;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using StorySkillInfo;
using UniRx;

public class PlayerParameters : MonoBehaviour
{
    //敵をサーチする範囲（山本）
    [Header("敵の方向を向く際のサーチ範囲")]
    [SerializeField] private float m_serchEnemyDist = 0.0f;
    public float SearchEnemyDist { get { return m_serchEnemyDist; } }

    //回避用変数（山本）
    [SerializeField] public float m_rollingPow = 2f;
    [SerializeField] public float m_rollingAnimeSpeed = 2f;

    //キー入力量の取得（山本）
    [SerializeField] private float m_speedStick = 0.0f;
    public float SpeedStick { get { return m_speedStick; } set { m_speedStick = value; } }

    // =====================
    // 設置アイテム用（吉田）
    // =====================
    public struct PutItemInfo
    {
        public ItemInfo.ItemTypeID m_itemTypeID;
        public uint m_itemID;
    }
    [HideInInspector]
    public PutItemInfo m_putItemInfo;

    public void SetPutItemInfo(ItemInfo.ItemTypeID _itemTypeID, uint _itemID)
    {
        m_putItemInfo.m_itemTypeID = _itemTypeID;
        m_putItemInfo.m_itemID = _itemID;
    }

    // =====================
    // アイテム採取用（吉田）
    // =====================

    // 取得可能状態にあるアイテム
    [HideInInspector]
    public AssignItemID m_ableGatheringItem = null;

    // ==========================
    // アイテム持ち状態用（山本）
    // ==========================
    [Header("アイテムを持つ場所")]
    [SerializeField] public Transform m_holdTrans = null;


    // =====================
    // アイテム投げる用（吉田）
    // =====================

    [Header("アイテム投げる用")]
    // 投げるときの初速度
    [HideInInspector]
    public Vector3 m_throwPower = new Vector3(0, 0, 0);

    // マウスの位置に表示するプレハブ
    [SerializeField]
    public GameObject m_mouseThrowAim;

    // 投げる位置
    [SerializeField]
    public Transform m_handTrans;

    [Header("カメラ切り替える用")]
    // AimCamera
    [SerializeField]
    public GameObject m_throwAimCamera = null;

    // プレイヤーの背後に追従するカメラ
    [SerializeField]
    public GameObject m_playerfollowCamera = null;


    //---------------------------------------------------------------------------------
    //========================
    //童話スキル関係（山本）
    //========================

    [Header("スキル関係")]
    [SerializeField] public GameObject m_skillPrefab;//緊急用
    [SerializeField] public GameObject m_spellEffect;//スキル詠唱用のエフェクト

    //IDで装備スキルを管理するように変更途中（山本）
    [Header("装備中の童話スキル1のID")]
    [SerializeField] private StorySkill_ID m_storySkill1_ID = StorySkill_ID.None;
    public StorySkill_ID StorySkill1_ID
    {
        get { return m_storySkill1_ID; }
        set { m_storySkill1_ID = value; }
    }

    [Header("装備中の童話スキル2のID")]
    [SerializeField] private StorySkill_ID m_storySkill2_ID = StorySkill_ID.None;
    public StorySkill_ID StorySkill2_ID
    {
        get { return m_storySkill2_ID; }
        set { m_storySkill2_ID = value; }
    }

    //スキル監視用
    //Rトリガー,第1スキル
    private GameObject m_obserbSkill1 = null;
    public GameObject ObserbSkill1 { set { m_obserbSkill1 = value; }get{ return m_obserbSkill1; } }
    //Lトリガー,第2スキル
    private GameObject m_obserbSkill2 = null;
    public GameObject ObserbSkill2 { set { m_obserbSkill2 = value; } get { return m_obserbSkill2; } }

    //スキル使用できるかのフラグ
    //Right(第1スキル)
    private bool m_useSkill1Flg = false;
    public bool UseSkill1Flg
    {
        get { return m_useSkill1Flg; }
        set { m_useSkill1Flg = value; }
    }
    //Left（第2スキル）
    private bool m_useSkill2Flg = false;
    public bool UseSkill2Flg
    {
        get { return m_useSkill2Flg; }
        set { m_useSkill2Flg = value; }
    }

    //第1スキル,第2スキルどちらが使用されたか確認用フラグ
    private bool m_triggerStorySkill_1 = false;
    private bool m_triggerStorySkill_2 = false;
    public bool TriggerStorySkill_1
    {
        get { return m_triggerStorySkill_1; }
        set { m_triggerStorySkill_1 = value; }
    }
    public bool TriggerStorySkill_2
    {
        get { return m_triggerStorySkill_2; }
        set { m_triggerStorySkill_2 = value; }
    }

    //童話スキルのキャストタイムの進捗度(山本)------------------------------------------------
    private FloatReactiveProperty m_castTimeProgress = new(0.0f);
    public FloatReactiveProperty CastTimeProgress { get { return m_castTimeProgress; } set { m_castTimeProgress = value; } }
    

    //----------------------------------------------------------------------------------------
    //===================================
    //武器出現・消失エフェクト関係（山本）
    //===================================

    // 武器が消失しているかのフラグ（山本）
    [HideInInspector]
    public bool m_isVanishWeapon = false;
    [Header("武器出現関連")]        //武器の消失イベント（山本）
    [Header("武器消失のタイムラインイベント")]
    [SerializeField] private TimelineAsset m_vanishWeaponEvent;
    [Header("武器出現のタイムラインイベント")]
    [SerializeField] private TimelineAsset m_appearWeaponEvent;
    [Header("消失した武器が出現するまでの時間")]
    [SerializeField] private float m_appearEventTime = 5.0f;

    private float m_eventTime = 0.0f;

    //-------------------------------------------------------------------------------------

    // =====================
    // UI表示用（吉田）
    // =====================
    [Header("UI表示用")]
    // 関数でUIの表示設定する
    [SerializeField]
    private ActionUIController m_actionUIController;


    //====================================
    //ゲームオーバー時のリスタート（山本）
    //====================================
    private Vector3 m_playerRestartPosition = Vector3.zero;
    public Vector3 PlayerRestartPosition
    {
        get { return m_playerRestartPosition; }
        set { m_playerRestartPosition = value; }
    }

    private Vector3 m_playerRestartForward = Vector3.zero;
    public Vector3 PlayerRestartForward
    {
        get { return m_playerRestartForward; }
        set { m_playerRestartForward = value; }
    }


    public void StartVanishWeapon()
    {
        m_eventTime = m_appearEventTime;
    }

    public void UpdateVanishWeapon(Animator animator)
    {
        if (!m_isVanishWeapon) return;

        if (m_eventTime <= 0.0f)
        {
            m_isVanishWeapon = false;

            if (animator.TryGetComponent<PlayableDirector>(out PlayableDirector director))
            {
                director.Play(m_appearWeaponEvent);
                //アニメーター側に武器を所持していることを伝える
                animator.SetFloat("HasWeapon", 1.0f);
            }
        }
        else
        {
            m_eventTime -= Time.deltaTime;
        }
    }

    public void AppearWeapon(Animator animator)
    {
        if (!m_isVanishWeapon) return;
        if (animator.TryGetComponent<PlayableDirector>(out PlayableDirector director))
        {
            director.Play(m_appearWeaponEvent);
            m_isVanishWeapon = false;
            //アニメーター側に武器を所持していることを伝える
            animator.SetFloat("HasWeapon", 1.0f);
        }
    }

    public void HideWeapon(Animator animator)
    {
        if (m_isVanishWeapon) return;
        if (animator.TryGetComponent<PlayableDirector>(out PlayableDirector director))
        {
            director.Play(m_vanishWeaponEvent);
            m_isVanishWeapon = true;
            //アニメーター側に武器を所持してないことを伝える
            animator.SetFloat("HasWeapon", 0.0f);
        }
    }

    public void AddActionUIState(ActionUIController.ActionUIState _state)
    {
        if (m_actionUIController == null)
        {
            Debug.LogError("ActionUIControllerが設定されていません");
            return;
        }

        m_actionUIController.AddState(_state);
    }

    public void RemoveActionUIState(ActionUIController.ActionUIState _state)
    {
        if (m_actionUIController == null)
        {
            Debug.LogError("ActionUIControllerが設定されていません");
            return;
        }

        m_actionUIController.RemoveState(_state);
    }
    public void AddAnyActionUIState(ActionUIController.ActionUIState _state, float _distance = 1.0f)
    {
        if (m_actionUIController == null)
        {
            Debug.LogError("ActionUIControllerが設定されていません");
            return;
        }

        m_actionUIController.AddAnyActionState(_state, _distance);
    }
    public void RemoveAnyActionUIState(ActionUIController.ActionUIState _state)
    {
        if (m_actionUIController == null)
        {
            Debug.LogError("ActionUIControllerが設定されていません");
            return;
        }

        m_actionUIController.RemoveAnyActionState(_state);
    }

    //==================================================================
    //ActionItemWindow用（山本）
    //=================================================================
    [Header("ActionItemController登録")]
    [SerializeField]
    private GameObject m_actionItemWindowController = null;
    public GameObject ActionItemWindowController { get { return m_actionItemWindowController; } }
}
