/**
* @file BaseEnemyData.cs
* @brief 敵のステータス
*/

using Arbor;
using FoodInfo;
using IngredientInfo;
using ItemInfo;
using PocketItemDataInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* @brief 敵のステータス
* @details データベースに登録するやつ   イハ
*/
[CreateAssetMenu]
[System.Serializable]
public class EnemyData : ScriptableObject
{
    [Tooltip("敵の種類")]
    [SerializeField] private EnemyID m_enemyID = EnemyID.chicken;
    public EnemyID EnemyID => m_enemyID;

    [Tooltip("ボス個体か(BGM変化等対応するか)")]
    [SerializeField] private bool m_isBoss = false;
    public bool IsBoss => m_isBoss;


    [Header("基礎ステータス")]
    [SerializeField]
    private CharacterStatus m_characterStatus;
    public CharacterStatus CharacterStatus => m_characterStatus;


    [Header("Idle用データ")]
    [SerializeField, Tooltip("Idle時に再移動するまでの時間")]
    private float m_idleSpan;
    public float IdleSpan => m_idleSpan;

    [SerializeField, Tooltip("Idle時の移動範囲")]
    private float m_idleMoveRadius;
    public float IdleMoveRadius => m_idleMoveRadius;



    [Header("チェイス用データ")]
    [Tooltip("チェイス用パラメータ")]
    [SerializeField] private ChaseData m_chaseParameter;
    public ChaseData EnemyChaseData => m_chaseParameter;



    [Header("ドロップ関連")]
    [SerializeField] public List<DropItemInfo> m_dropItemInfo;


    [Header("各攻撃の攻撃力など")]
    [Tooltip("各攻撃の詳細")]
    [SerializeField] private List<AttackDamageData> m_attackData;
    public List<AttackDamageData> AttackData { get { return m_attackData; } }


    [Header("各攻撃の抽選用データ")]
    [Tooltip("第一段階の抽選パラメータ")]
    [SerializeField] private EnemyAttackData m_firstAttackData;
    public EnemyAttackData FirstAttackData => m_firstAttackData;

    [Tooltip("第二段階の抽選パラメータ")]
    [SerializeField] private EnemyAttackData m_secondAttackData;
    public EnemyAttackData SecondAttackData => m_secondAttackData;
}


public enum EnemyID
{
    chicken,
    Deer,
    Hog,
    Snake,
    BossChicken,
    BossWolf
}

[Serializable]
public struct DropItemInfo
{
    public IngredientInfo.IngredientID m_ingredientID;
    public int m_weightLottery;
}