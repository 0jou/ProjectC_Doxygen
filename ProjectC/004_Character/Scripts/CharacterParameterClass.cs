using Arbor;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

// 追跡用データ
[Serializable]
public class ChaseData
{
    [Tooltip("平常時の索敵距離")]
    [SerializeField] private float m_searchCharacterDist = 5f;
    public float SearchCharacterDist { get { return m_searchCharacterDist; } }

    [Tooltip("料理の索敵距離")]
    [SerializeField] private float m_searchDishDist = 8f;
    public float SearchDishDist { get { return m_searchDishDist; } }

    [Tooltip("視界判定に関係なく強制で気付く距離")]
    [SerializeField] private float m_noticeDist = 2f;
    public float NoticeDist { get { return m_noticeDist; } }

    [Tooltip("視野角")]
    [SerializeField] private int m_viewAngle = 90;
    public int ViewAngle { get { return m_viewAngle; } }

    [Tooltip("ターゲットが視野にいなくても追いかけてくる時間")]
    [SerializeField] private float m_maxChaseTime = 1;
    public float MaxChaseTime { get { return m_maxChaseTime; } }

    [Tooltip("チェイスを続ける敵との最大距離")]
    [SerializeField] private float m_chaseDistFromTarget = 15.0f;
    public float ChaseDistFromTarget { get { return m_chaseDistFromTarget; } }

    [Tooltip("スポーン地点から移動する最大距離")]
    [SerializeField] private float m_distAwayFromSpawnPos = 15.0f;
    public float DistAwayFromSpawnPos { get { return m_distAwayFromSpawnPos; } }
}


// 攻撃抽選用データ
[Serializable]
public class EnemyAttackData
{
    [SerializeField, Tooltip("攻撃毎の間隔")]
    private float m_attackSpan;
    public float AttackSpan => m_attackSpan;

    [SerializeField, Tooltip("攻撃前の最大接近時間")]
    private float m_maxApproachTime;
    public float MaxApproachTime => m_maxApproachTime;

    [Header("攻撃抽選用データ")]
    [SerializeField]
    private List<AttackLotteryData> m_lotteryData;
    public List<AttackLotteryData> LotteryData => m_lotteryData;

    [Serializable]
    public class AttackLotteryData
    {
        [Tooltip("最小の抽選値")]
        [SerializeField] public int m_defaultWeight;
        [Tooltip("抽選外したときの追加抽選値")]
        [SerializeField] public int m_weightInclude;
        [Tooltip("攻撃基準距離")]
        [SerializeField] public float m_bestAttackDist;
        [Tooltip("攻撃前に接近するかどうか")]
        [SerializeField] public bool m_isApproachBeforeAttack;

        private int m_loseCount;
        public int LoseCount { get { return m_loseCount; } set { m_loseCount = value; } }

        [HideInInspector]
        public float m_lotteryWeight;
    }
}


// 攻撃コライダー用データ
[Serializable]
public class AttackDamageData
{
    [SerializeField] private float m_hitStopTime;   // ヒットストップの時間
    public float HitStopTime => m_hitStopTime;

    [SerializeField] private float m_attack;        // CharacterCoreのAtkに追加する攻撃力
    public float Attack => m_attack;

    [SerializeField] private float m_knockBackMultiplier = 1f;  // ノックバックの強さ
    public float KnockBackMultiplier => m_knockBackMultiplier;

    [SerializeField] private bool m_isStrongAttack = false;     // 強攻撃か否か
    public bool IsStrongAttack => m_isStrongAttack;

    [SerializeField] private GameObject m_assetHitEffect;
    public GameObject AssetHitEffect => m_assetHitEffect;

    [SerializeField] private SoundData m_soundData;
    public SoundData SoundData => m_soundData;

    [SerializeField] private bool m_doFriendlyFire = false;
    public bool DoFriendlyFire => m_doFriendlyFire;

    [SerializeField] private bool m_doMultiHit = false;
    public bool DoMultiHit => m_doMultiHit;

    [Header("回復するBPの倍率（ダメージにかける） playerにしか関係ない")]
    [SerializeField]
    private float m_recoverBPMagni = 0.1f;
    public float RecoverBPMagni => m_recoverBPMagni;

    [Header("相手に与える状態異常")]
    [SerializeField]
    private ConditionInfo.ConditionID m_condition = ConditionInfo.ConditionID.Normal;
    public ConditionInfo.ConditionID ConditionID => m_condition;
}


// キャラクター共通のステータス
[Serializable]
public class CharacterStatus
{
    [Header("移動系")]
    [SerializeField] private float m_walkSpeed;
    public float WalkSpeed
    {
        get { return m_walkSpeed; }
        set { m_walkSpeed = value; }
    }

    [SerializeField] private float m_dushSpeed;
    public float DushSpeed
    {
        get { return m_dushSpeed; }
        set { m_dushSpeed = value; }
    }

    [Header("戦闘系")]
    public FloatReactiveProperty m_hp;  // 体力
    private float m_maxHP;
    public float MaxHP { get { return m_maxHP; } set { m_maxHP = value; } }

    public float m_attack;              // 攻撃力
    public float m_knockBackDamage;      // ノックバックするダメージ

    public CharacterStatus(CharacterStatus status)
    {
        m_walkSpeed = status.m_walkSpeed;
        m_dushSpeed = status.m_dushSpeed;
        m_hp = new(status.m_hp.Value);
        m_maxHP = status.m_hp.Value;
        m_attack = status.m_attack;
        m_knockBackDamage = status.m_knockBackDamage;
    }
}

// プレイヤー用ステータス
[Serializable]
public class PlayerStatus
{
    public FloatReactiveProperty m_bp;  // バトルポイント
    private float m_maxBP;
    public float MaxBP { get { return m_maxBP; } set { m_maxBP = value; } }
    //バトルポイント自然回復量
    public float m_bpRecoverSpeed;

    // 装備スキルごとのBP
    public FloatReactiveProperty m_bpSkill_1;
    private float m_maxBPSkill_1;
    public float MaxBPSkill_1 { get { return m_maxBPSkill_1; } set { m_maxBPSkill_1 = value; } }

    public FloatReactiveProperty m_bpSkill_2;
    private float m_maxBPSkill_2;
    public float MaxBPSkill_2 { get { return m_maxBPSkill_2; } set { m_maxBPSkill_2 = value; } }

    public FloatReactiveProperty m_stamina;     // スタミナ
    private float m_maxStamina;
    public float MaxStamina { get { return m_maxStamina; } set { m_maxStamina = value; } }
    public float m_staminaSpeed;                // スタミナ回復速度

    public float m_rollingStaminaCost;          // 回避 スタミナ消費量
    public float m_dashStaminaCost;             // ダッシュ スタミナ消費量

    public void Init()
    {
        MaxStamina = m_stamina.Value;
    }
}