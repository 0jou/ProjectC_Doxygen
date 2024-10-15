using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OrderFoodInfo;

using ItemInfo;
using FoodInfo;

public class CounterManager : BaseManager<CounterManager>
{

    // オーダーされたカウンター料理を管理するマネージャークラス
    // 客がオーダー料理を持たなければいけないので、Active=Falseで管理
    // 制作者　田内

    [Header("カウンターのポイントリスト")]
    [SerializeField]
    private List<CounterPointData> m_counterPointDataList = new();


    //========================================================
    // オーダー料理データリスト

    // 料理リスト
    private List<OrderFoodData> m_orderFoodDataList = new();

    public List<OrderFoodData> OrderFoodDataList
    {
        get { return m_orderFoodDataList; }
    }


    //======================================================================


    /// <summary>
    /// 注文料理を受け取る
    /// </summary>
    public OrderFoodData OrderFood(CustomerData _customerData)
    {
        // オーダーできなければ
        if (!IsOrder()) return null;

        if (_customerData == null)
        {
            Debug.LogError("引数客情報がnullです");
            return null;
        }

        // 作成する料理をランダムで取得(作成できなければnullが帰ってくる)
        var orderData = ManagementProvideFoodManager.instance.GetRandomFoodData();
        if (orderData == null) return null;


        // 料理のインスタンスを作成する
        var foodData = CreateOrderFood((FoodID)orderData.ItemID, _customerData);

        // 取り除く
        if (foodData != null)
        {
            ManagementProvideFoodManager.instance.RemoveProvideFood((FoodID)orderData.ItemID);
        }

        return foodData;

    }

    /// <summary>
    /// 料理を注文できるかどうか
    /// </summary>
    public bool IsOrder()
    {
        // 提供できる料理があるかを確認する
        if (ManagementProvideFoodManager.instance.IsCreate())
        {
            // order可能
            return true;
        }

        // order不可能
        return false;
    }


    // 現在料理が注文されているか確認するメソッド
    public bool IsOrdering()
    {

        if (m_orderFoodDataList.Count <= 0)
        {
            // 注文されている状態でなければ
            return false;
        }

        // 注文されている状態であれば
        return true;
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




    // 現在受け取り待ちの料理を取得する、
    public OrderFoodData GetCounterFoodData()
    {
        OrderFoodData data = null;

        foreach (var foodData in m_orderFoodDataList)
        {
            if (foodData == null) continue;

            // まだ受け取り手がいなければ
            if (foodData.IsFindStaff())
            {
                data = foodData;
                break;
            }
        }

        return data;

    }



    private void Update()
    {
        // 作成カウント
        CreatCount();

        // 取り除き判定
        CheckRemoveOrderFoodDataList();
    }


    // オーダーされた料理のインスタンスを作成する
    private OrderFoodData CreateOrderFood(FoodID _foodID, CustomerData _customerData)
    {
        // 料理データを取得
        var foodData = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)_foodID);
        if (foodData == null) return null;

        // プレハブ
        var prefab = foodData.ManagementFoodPrefab;
        if (prefab == null)
        {
            Debug.LogError("プレハブが存在しないため作成できませんでした");
            return null;
        }

        //====================
        // インスタンスを作成

        var obj = Instantiate(prefab);
        obj.SetActive(false);

        //  注文料理データ
        var orderFoodData = obj.GetComponentInChildren<OrderFoodData>();
        if (orderFoodData == null)
        {
            Debug.LogError("OrderFoodDataがアタッチされていません");
            Destroy(obj);
            return null;
        }
        // 料理IDをセット
        orderFoodData.FoodID = _foodID;

        // 目的地座標をセット
        if (_customerData)
        {
            orderFoodData.TargetTableSetData = _customerData.TargetTableSetData;
        }

        // リストに追加
        AddOrderFoodDataList(orderFoodData);


        return orderFoodData;

    }



    // 作成時間をカウントする
    private void CreatCount()
    {
        // 作成中の料理をカウント
        // アクティブがfalseの時のみ
        foreach (var food in m_orderFoodDataList)
        {
            if (food == null) continue;

            if (food.CreatCount())
            {
                // ポイントをセットする
                SetCounterPoint(food);
            }
        }
    }


    // ポイントをセット
    private void SetCounterPoint(OrderFoodData _data)
    {
        if (_data == null) return;

        // 既にポイントがあれば操作しない
        if (_data.CounterPointData != null) return;

        CounterPointData point = GetRandomPoint();
        if (point == null)
        {
            // カウンターポイントが空くまで動かさない
            return;
        }

        // ポイントをセット
        _data.CounterPointData = point;

        // 動作開始
        _data.gameObject.SetActive(true);

        //料理生成時の音を追加（山本）
        SoundManager.Instance.Start3DPlayback("DoneCook", point.SetPoint.position);

    }




    // 注文料理データを追加
    private void AddOrderFoodDataList(OrderFoodData _data)
    {
        if (_data == null) return;
        m_orderFoodDataList.Add(_data);
    }



    // 注文料理データを削除
    private void CheckRemoveOrderFoodDataList()
    {
        m_orderFoodDataList.RemoveAll(data =>
        {
            // 存在しなければ
            if (data == null) return true;

            if (!(data.CurrentOrderFoodState == OrderFoodState.FindStaff ||
            data.CurrentOrderFoodState == OrderFoodState.WaitCounter))
            {
                // ポイントを解放
                data.CounterPointData = null;

                return true;
            }

            return false;
        }
       );
    }


    // ランダムで空いているカウンターポイントを取得(空きがなければnull)
    private CounterPointData GetRandomPoint()
    {

        m_counterPointDataList.RandomList();
        foreach (var counterPoint in m_counterPointDataList)
        {
            if (counterPoint == null &&
                counterPoint.SetPoint == null &&
                counterPoint.DestinationPoint == null) continue;

            // 設置されていれば
            if (counterPoint.IsSet) continue;

            return counterPoint;
        }

        return null;
    }

}
