using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OrderFoodInfo;

using ItemInfo;
using FoodInfo;

using NaughtyAttributes;

public class CounterManager : BaseManager<CounterManager>
{

    // オーダーされたカウンター料理を管理するマネージャークラス
    // 客がオーダー料理を持たなければいけないので、Active=Falseで管理
    // 制作者　田内

    [Header("カウンターのポイントリスト")]
    [SerializeField]
    private List<CounterPointData> m_counterPointDataList = new();


    //============================
    // オーダー料理データリスト

    // 料理リスト
    private List<OrderFoodData> m_orderFoodDataList = new();

    public List<OrderFoodData> OrderFoodDataList
    {
        get { return m_orderFoodDataList; }
    }


    //================================================
    //                  実行処理
    //================================================


    private void Update()
    {
        // 注文された料理の処理
        OnUpdateOrderFood();

        // 取り除き判定
        CheckRemove();
    }


    /// <summary>
    /// 料理を注文する
    /// </summary>
    public OrderFoodData OrderFood(CustomerData _customerData)
    {
        if (_customerData == null) return null;
        if (IsOrder() == false) return null;

        // 作成する料理をランダムで取得
        var orderData = ManagementProvideFoodManager.instance.GetRandomFoodData();
        if (orderData == null) return null;

        // 料理のインスタンスを作成する
        var foodData = CreateOrderFood((FoodID)orderData.ItemID, _customerData);

        return foodData;

    }

    /// <summary>
    /// 料理を注文できるかどうか
    /// </summary>
    public bool IsOrder()
    {
        // なにかしら提供できる料理があるかを確認する
        if (ManagementProvideFoodManager.instance.IsCreateAll()) return true;
        else return false;
    }


    /// <summary>
    /// 現在料理が注文されているか確認する
    /// </summary>
    public bool IsOrdering()
    {
        if (m_orderFoodDataList.Count <= 0) return false;
        else return true;
    }


    /// <summary>
    /// 調理中(作成中)の料理があるかどうか
    /// </summary>
    public bool IsCooking()
    {
        foreach (var data in m_orderFoodDataList)
        {
            if (data == null) continue;
            if (data.gameObject.activeSelf == false)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 現在受け取り待ちの料理を取得する
    /// </summary>
    public OrderFoodData GetCounterFoodData()
    {
        foreach (var foodData in m_orderFoodDataList)
        {
            if (foodData == null) continue;
            if (foodData.IsFindStaff())
            {
                return foodData;
            }
        }

        return null;
    }

    /// <summary>
    /// ランダムで空いているカウンターポイントを取得(空きがなければnull)
    /// </summary>
    public CounterPointData GetRandomPoint()
    {
        m_counterPointDataList.RandomList();
        foreach (var counterPoint in m_counterPointDataList)
        {
            // 設置されていれば
            if (counterPoint.SetOrderFoodData == null)
            {
                return counterPoint;
            }
        }

        return null;
    }


    // オーダーされた料理のインスタンスを作成する
    private OrderFoodData CreateOrderFood(FoodID _foodID, CustomerData _customerData)
    {
        var foodData = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)_foodID);
        if (foodData == null || _customerData == null) return null;


        // 経営用プレハブを取得/作成
        var prefab = foodData.ManagementFoodPrefab;
        if (prefab == null) return null;
        var obj = Instantiate(prefab);
        obj.SetActive(false);


        // 注文料理データ
        if (obj.TryGetComponent<OrderFoodData>(out var orderFoodData))
        {
            // 料理IDをセット
            orderFoodData.FoodID = _foodID;

            // 目的地座標をセット
            orderFoodData.TargetTableSetData = _customerData.TargetTableSetData;

            // リストに追加
            m_orderFoodDataList.Add(orderFoodData);

            // 問題なく作成出来たので、使用した食材を取り除く
            ManagementProvideFoodManager.instance.RemoveProvideFood(_foodID);

            return orderFoodData;
        }
        else
        {
            Debug.LogError("OrderFoodDataがアタッチされていません");
            Destroy(obj);
            return null;
        }
    }

    // 用済みの注文料理データを削除
    private void CheckRemove()
    {
        m_orderFoodDataList.RemoveAll(data =>
        {
            if (data == null) return true;
            if (data.IsCounterFood() == false) return true;
            else return false;
        }
       );
    }

    // 作成した料理の処理
    private void OnUpdateOrderFood()
    {
        foreach (var data in m_orderFoodDataList)
        {
            if (data == null) continue;
            data.OnUpdate();
        }
    }
}
