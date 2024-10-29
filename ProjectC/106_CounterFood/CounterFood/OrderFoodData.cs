using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrderFoodInfo;

using ItemInfo;
using FoodInfo;
using NaughtyAttributes;

namespace OrderFoodInfo
{

    public enum OrderFoodState
    {
        FindStaff,    // スタッフに探され待ち状態
        WaitCounter,  // 受け取り待ち状態
        Carry,        // 運ばれている状態
        Set,          // テーブルにセットされた状態
    }
}

public partial class OrderFoodData : MonoBehaviour
{
    // カウンターが作成した料理データ
    // 制作者　田内



    [BoxGroup("作成時")]
    [Header("再生するSE")]
    [SerializeField]
    private string m_seName = "DoneCook";

    [BoxGroup("作成時")]
    [Header("作成するエフェクト")]
    [SerializeField]
    private GameObject m_effectPrefab = null;


    //===========
    // ステート

    private OrderFoodState m_currentOrderFoodState = OrderFoodState.FindStaff;

    public OrderFoodState CurrentOrderFoodState
    {
        get { return m_currentOrderFoodState; }
        set { m_currentOrderFoodState = value; }
    }

    //============
    // 料理ID

    private FoodID m_foodID = FoodID.Omelette;

    public FoodID FoodID
    {
        get { return m_foodID; }
        set
        {
            m_foodID = value;

            // 作成時間をセット
            var data = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)m_foodID);
            if (data != null) m_createDelay = data.CreateDelay;
        }
    }

    //======================
    // 目標のテーブル情報

    private TableSetData m_targetTabelSetData = null;

    public TableSetData TargetTableSetData
    {
        get { return m_targetTabelSetData; }
        set { m_targetTabelSetData = value; }
    }

    //=================================
    // 設置されているカウンターポイント

    private CounterPointData m_counterPointData = null;

    public CounterPointData CounterPointData
    {
        get { return m_counterPointData; }
    }

    // 作成されたかどうか
    private bool m_isCreate = false;

    public bool IsCreate
    {
        get { return m_isCreate; }
    }


    // 作成にかかる時間
    private float m_createDelay = 999.0f;

    // 現在の作成度合
    private float m_currentCreateDelayCount = 0.0f;

    //========================================
    //              実行処理
    //========================================

    public void OnUpdate()
    {
        // 作成カウントを行う
        CreatCount();

        // カウンターポイントを探す
        FindCounterPointData(); ;
    }


    /// <summary>
    /// 作成時間をカウントする
    /// </summary>
    private void CreatCount()
    {
        if (IsCounterFood() == false) return;
        if (m_isCreate == true) return;

        // 更新する
        m_currentCreateDelayCount += Time.deltaTime;

        // 作成中の料理を更新
        if (m_createDelay <= m_currentCreateDelayCount)
        {
            m_isCreate = true;
        }
    }


    private void FindCounterPointData()
    {
        if (IsCounterFood() == false) return;

        if (m_isCreate == false) return;
        if (m_counterPointData != null) return;

        CounterPointData point = CounterManager.instance.GetRandomPoint();
        if (point == null) return;

        // ポイントをセット
        m_counterPointData = point;
        gameObject.transform.position = m_counterPointData.SetPoint.position;
        gameObject.transform.rotation = m_counterPointData.SetPoint.rotation;

        // ポイントに自身をセット
        m_counterPointData.SetOrderFoodData = this;

        // 動作開始
        gameObject.SetActive(true);

        // 音声を再生
        if (m_seName != "")
        {
            SoundManager.Instance.StartPlayback(m_seName);
        }

        // エフェクトを作成
        if (m_effectPrefab != null)
        {
            Instantiate(m_effectPrefab, point.SetPoint.position, point.SetPoint.rotation);
        }

    }


    /// <summary>
    /// 客を探している状態か
    /// </summary>
    public bool IsFindStaff()
    {
        if (gameObject.activeSelf &&
                 m_currentOrderFoodState == OrderFoodState.FindStaff)
        {
            return true;
        }

        return false;
    }


    /// <summary>
    /// カウンターに設置されている料理かどうか
    /// </summary>
    public bool IsCounterFood()
    {
        switch (m_currentOrderFoodState)
        {
            case OrderFoodState.FindStaff:
            case OrderFoodState.WaitCounter:
                {
                    return true;
                }
            // カウンターから離れた場合手放す
            default:
                {
                    return false;
                }
        }
    }

}
