using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrderFoodInfo;

using ItemInfo;
using FoodInfo;

namespace OrderFoodInfo
{

    public enum OrderFoodState
    {
        FindStaff,     // スタッフに探され待ち状態
        WaitCounter,  // 受け取り待ち状態
        Carry,        // 運ばれている状態
        Set,          // テーブルにセットされた状態
    }
}

public partial class OrderFoodData : MonoBehaviour
{

    // カウンターが作成した料理データ
    // 制作者　田内

    //======================================================================
    // ステート

    private OrderFoodState m_currentOrderFoodState = OrderFoodState.FindStaff;

    public OrderFoodState CurrentOrderFoodState
    {
        get { return m_currentOrderFoodState; }
        set { m_currentOrderFoodState = value; }
    }

    //=================================================
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

    //============================================
    // 目標のテーブル情報

    private TableSetData m_targetTabelSetData = null;

    public TableSetData TargetTableSetData
    {
        get { return m_targetTabelSetData; }
        set { m_targetTabelSetData = value; }
    }

    //============================================
    // 設置されているカウンターポイント
    private CounterPointData m_counterPointData = null;
    public CounterPointData CounterPointData
    {
        get { return m_counterPointData; }
        set
        {
            if (m_counterPointData != null)
            {
                // セットを解除
                m_counterPointData.IsSet = false;
            }

            m_counterPointData = value;

            if (m_counterPointData?.SetPoint != null)
            {

                m_counterPointData.IsSet = true;

                // 座標更新
                gameObject.transform.position = m_counterPointData.SetPoint.position;
                gameObject.transform.rotation = m_counterPointData.SetPoint.rotation;
            }
        }
    }

    //==================================
    // 作成にかかる時間
    private float m_createDelay = 0.0f;

    // 現在の作成度合
    private float m_currentCreateDelayCount = 0.0f;



    // 削除後に通る処理
    private void OnDestroy()
    {
        // ポイントを手放す
        CounterPointData = null;
    }



   /// <summary>
   /// 作成時間をカウントする
   /// </summary>
    public bool CreatCount()
    {
        // 作成中の料理を更新
        if (m_createDelay < m_currentCreateDelayCount)
        {
            return true;
        }
        else
        {
            // 更新する
            m_currentCreateDelayCount += Time.deltaTime;
            return false;
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

}
