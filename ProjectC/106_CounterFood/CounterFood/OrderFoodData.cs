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
        //==============================
        // カウンターで管理する範囲
        //==============================

        WaitChefStaff = 1,      // シェフスタッフ待ち
        Cooking = 2,            // 作成中
        FindCounterPoint = 3,   // カウンターポイントを探す
        WaitHallStaff = 4,      // ホールスタッフ待ち
        WaitCarry = 5,          // 受け取り待ち状態


        //==============================
        // その他
        //==============================

        Carry = 11,            // 運ばれている状態
        Set = 12,              // テーブルにセットされた状態
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

    private OrderFoodState m_currentOrderFoodState = OrderFoodState.WaitChefStaff;

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


    // 作成にかかる時間
    private float m_createDelay = 999.0f;

    // 現在の作成度合
    private float m_currentCreateDelayCount = 0.0f;


    //========================================
    //              実行処理
    //========================================


    /// <summary>
    /// 作成時間をカウントする
    /// 作成できたらステートを変更し、trueを返す
    /// </summary>
    public bool CreatCount(float _ratio)
    {
        // ポイントを探す状態でなければ動作しない
        if (m_currentOrderFoodState != OrderFoodState.Cooking) return false;

        // 作成カウントを更新する
        m_currentCreateDelayCount += (Time.deltaTime * _ratio);

        // 作成中の料理を更新
        if (m_createDelay <= m_currentCreateDelayCount)
        {
            // ホールスタッフを待つ状態に変更
            m_currentOrderFoodState = OrderFoodState.FindCounterPoint;

            return true;
        }

        return false;
    }


    /// <summary>
    /// カウンターを探す
    /// </summary>
    public void SetCounterPoint()
    {
        // ポイントを探す状態でなければ動作しない
        if (m_currentOrderFoodState != OrderFoodState.FindCounterPoint) return;

        if (m_counterPointData != null) return;

        // ポイントを取得
        CounterPointData point = CounterManager.instance.GetRandomPoint();
        if (point == null) return;

        // ポイントをセット・ステート更新・座標更新・動作開始
        m_counterPointData = point;
        m_currentOrderFoodState = OrderFoodState.WaitHallStaff;

        gameObject.transform.position = m_counterPointData.SetPoint.position;
        gameObject.transform.rotation = m_counterPointData.SetPoint.rotation;
        gameObject.SetActive(true);

        // ポイントに自身のデータをセット
        m_counterPointData.SetOrderFoodData = this;

        // 音声・エフェクトを再生
        if (0 < m_seName.Length) SoundManager.Instance.StartPlayback(m_seName);
        if (m_effectPrefab != null) Instantiate(m_effectPrefab, point.SetPoint.position, point.SetPoint.rotation);

    }



}
