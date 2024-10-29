/*!
 * @file CustomerData.cs
 * @brief 客情報を管理するクラス
 * @author 田内
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;
using FoodInfo;
using CustomerStateInfo;

namespace CustomerStateInfo
{
    /// <summary>
    /// @brief 客のステート情報 現在のステート依存での処理等に使用
    /// @attention 一部ステートではステート依存でオブジェクトの破棄が行われたりしているため注意
    /// </summary>
    public enum CustomerState
    {
        Normal,
        MoveChair,   // 椅子まで移動する
        WaitFood,    // 料理を待つ状態
        AngryExit,   // 怒った状態
        Eat,         // 料理を食べている状態
        Exit,        // 帰る状態
        WaitQueue,   // 待ち列を待つ状態
        DineDashExit,    // 食い逃げ状態
    }

}

/// <summary>
/// @brief 客情報を管理するクラス
/// </summary>
public class CustomerData : MonoBehaviour
{
    // 客情報
    // 制作者　田内


    //=========================================
    // 怒る時間

    //! @brief 怒るまでの時間(sec)
    [SerializeField]
    private float m_angryTime = 10.0f;

    public float AngryTime
    {
        get { return m_angryTime; }
    }

    //! @brief 怒りゲージ経過時間(sec)
    private float m_angryCount = 0.0f;

    public float AngryCount
    {
        get { return m_angryCount; }
        set { m_angryCount = value; }
    }

    //=========================================
    // 食べる時間

    //! @brief 食べるきるまでの時間(sec)
    [SerializeField]
    private float m_eatTime = 10.0f;

    public float EatTime
    {
        get { return m_eatTime; }
    }


    //! @brief 食べ経過時間
    private float m_eatCount = 0.0f;

    public float EatCount
    {
        get { return m_eatCount; }
        set { m_eatCount = value; }
    }

    //=========================================
    // 客のステート

    //! @brief 現在の客のステート情報
    private CustomerState m_currentCustomerState = CustomerState.Normal;

    public CustomerState CurrentCustomerState
    {
        get { return m_currentCustomerState; }
        set { m_currentCustomerState = value; }
    }

    //=========================================
    // 要求している料理

    //! @brief ターゲットの料理情報
    private OrderFoodData m_targetOrderFoodData = null;

    public OrderFoodData TargetOrderFoodData
    {
        get { return m_targetOrderFoodData; }
        set { m_targetOrderFoodData = value; }
    }


    //=====================================
    // 出現座標

    //! @brief 出現/帰還 地点座標
    private Vector3 m_appearPos = new();

    public Vector3 AppearPos
    {
        get { return m_appearPos; }
    }


    //======================================
    // ターゲットのテーブル

    // 目的地のテーブル情報
    private TableSetData m_targetTableSetData = null;

    public TableSetData TargetTableSetData
    {
        get { return m_targetTableSetData; }
        set { m_targetTableSetData = value; }
    }


    //=======================================
    // 待ち列のデータ

    //! @brief 待ち行列のデータ
    private QueueData m_queueData = null;

    public QueueData QueueData
    {
        get { return m_queueData; }
        set { m_queueData = value; }
    }

    //===========================================================
    //                      実行処理
    //===========================================================

    private void Start()
    {
        // マネージャーに登録
        CustomerManager.instance.AddCustomerData(this);

        Initialize();
    }


    // 初期値をセット
    protected void Initialize()
    {
        // 出現した位置
        m_appearPos = transform.position;
    }

    /// <summary>
    /// @brief 怒り時間をカウントする
    /// </summary>
    /// <returns>true-怒る:false-まだ</returns>
    public bool CountAngry()
    {
        m_angryCount += Time.deltaTime;

        if (m_angryTime < m_angryCount)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 食べる時間をカウントする
    /// </summary>
    /// <returns>true-怒る:false-まだ</returns>
    public bool CountEat()
    {
        m_eatCount += Time.deltaTime;

        if (m_eatTime < m_eatCount)
        {
            return true;
        }

        return false;
    }

}
