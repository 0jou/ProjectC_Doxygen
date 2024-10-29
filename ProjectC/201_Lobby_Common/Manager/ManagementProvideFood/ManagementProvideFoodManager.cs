using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodInfo;
using ItemInfo;
using PocketItemDataInfo;

using UniRx;

/// <summary>
/// リスト変化時のイベント
/// </summary>
public class GlobalChangeProvideFoodEvent
{
}

/// <summary>
/// 料理売り切れ時のイベント
/// </summary>
public class GlobalSoldOutProvideFoodEvent
{
    public ProvideFoodData ProvideFoodData;
}

/// <summary>
/// 料理追加時のイベント
/// </summary>
public class GlobalAddProvideFoodListEvent
{
    public ProvideFoodData ProvideFoodData;
}

/// <summary>
/// 料理取り除く時のイベント
/// </summary>
public class GlobalRemoveProvideFoodListEvent
{
    public ProvideFoodData ProvideFoodData;
}



[System.Serializable]
public class ProvideFoodData
{
    //========================
    // 料理ID
    private FoodID m_foodID = FoodID.Omelette;

    public FoodID FoodID
    {
        get { return m_foodID; }
        set { m_foodID = value; }
    }

    //========================
    // 提供可能数

    private uint m_provideNum = 0;

    public uint ProvideNum
    {
        get { return m_provideNum; }
        set { m_provideNum = value; }
    }

    //========================
    // ボーナス数

    private uint m_bonusNum = 0;

    public uint BonusNum
    {
        get { return m_bonusNum; }
        set { m_bonusNum = value; }
    }

    //========================
    // 売れた数
    private uint m_soldNum = 0;

    public uint SoldNum
    {
        get { return m_soldNum; }
        set { m_soldNum = value; }
    }

    //========================
    // 取り除いた数
    private uint m_removeNum = 0;

    public uint RemoveNum
    {
        get { return m_removeNum; }
        set { m_removeNum = value; }
    }

}




public class ManagementProvideFoodManager : BaseManager<ManagementProvideFoodManager>
{

    // 制作者　田内
    // 提供する料理を操作・管理するマネージャー

    [Header("ポケットの種類")]
    [SerializeField]
    private PocketType m_pocketType = PocketType.ManagementStorage;

    public PocketType PocketType
    {
        get { return m_pocketType; }
    }

    [Header("最大で提供できる料理の種類")]
    [SerializeField]
    [Min(1)]
    private uint m_maxFoodListCount = 1;

    public uint MaxFoodListCount
    {
        get { return m_maxFoodListCount; }
    }

    //======================================
    // 提供する料理


    private List<ProvideFoodData> m_provideFoodDataList = new();

    public List<ProvideFoodData> ProvideFoodDataList
    {
        get { return m_provideFoodDataList; }
    }

    //======================================================================
    //                  実行処理
    //======================================================================


    /// <summary>
    /// コントローラーを基に提供料理をセットする
    /// </summary>
    public void SetProvideFoodControllerData(SelectManagementProvideFoodController _controller)
    {
        // リストを初期化
        m_provideFoodDataList.Clear();

        // 追加する
        foreach (var id in _controller.ProvideFoodIDList)
        {
            AddProvideFoodList(id);
        }
    }


    /// <summary>
    /// 提供料理を追加する
    /// </summary>
    public bool AddProvideFoodList(FoodID _foodID)
    {
        // 追加できるか確認する
        if (IsAddList() == false)
        {
            return false;
        }

        // 既に追加されていれば追加しない
        if (IsAddedProvideFood(_foodID))
        {
            return false;
        }

        // 作成
        ProvideFoodData provideFoodData = new();
        provideFoodData.FoodID = _foodID;
        provideFoodData.ProvideNum = (uint)FoodData.GetProvideNum(m_pocketType, _foodID);

        // リストに追加
        m_provideFoodDataList.Add(provideFoodData);

        // リスト変更イベントを送信
        PublishChangeProvideFoodEvent();

        // 追加イベントを送信
        PublishAddListEvent(provideFoodData);


        return true;
    }


    /// <summary>
    /// ボーナスを追加
    /// </summary>
    public void AddBonusNum(FoodID _foodID, uint _bonusNum = 1)
    {
        var data = GetProvideFood(_foodID);
        if (data == null) return;

        data.BonusNum += _bonusNum;
    }


    /// <summary>
    /// 提供料理の売り上げ数を加算
    /// </summary>
    public void SoldFood(FoodID _id)
    {
        foreach (var data in m_provideFoodDataList)
        {
            if (data == null) continue;
            if (data.FoodID != _id) continue;

            // 売れた数
            data.SoldNum++;

            // リスト変更イベントを送信
            PublishChangeProvideFoodEvent();

            break;
        }
    }


    /// <summary>
    /// 必要素材を取り除く
    /// </summary>
    public void RemoveProvideFood(FoodID _id)
    {
        foreach (var data in m_provideFoodDataList)
        {
            if (data == null) continue;
            if (data.FoodID != _id) continue;

            // ボーナスがあれば食材を使用しない
            if (0 < data.BonusNum)
            {
                data.BonusNum--;
                return;
            }

            // 通常
            if (FoodData.RemoveProvideNeedIngredient(m_pocketType, _id))
            {
                // 作成可能数を減算
                data.ProvideNum--;

                // 取り除いた数を加算
                data.RemoveNum++;

                // 取り除きイベントの送信
                PublishRemoveListEvent(data);

                // 売り切れイベントの確認・送信
                PublishSoldOutProvideFoodEvent(data);

                // リスト変更イベントを送信
                PublishChangeProvideFoodEvent();
            }
            break;
        }
    }


    /// <summary>
    /// 引数提供料理を取得する
    /// 無ければNullを返す
    /// </summary>
    public ProvideFoodData GetProvideFood(FoodID _foodID)
    {
        foreach (var data in m_provideFoodDataList)
        {
            if (data == null) continue;
            if (data.FoodID == _foodID) return data;
        }
        return null;
    }


    /// <summary>
    /// リストからランダムで料理データを取得
    /// 提供できるものが無ければnullを返す
    /// </summary>
    public FoodData GetRandomFoodData()
    {
        // ランダムにする
        var randomList = m_provideFoodDataList.RandomList();

        foreach (var provideFood in randomList)
        {
            if (provideFood == null) continue;
            if (0 < provideFood.ProvideNum)
            {
                // 料理データを返す
                return ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)provideFood.FoodID);
            }
        }
        return null;
    }


    /// <summary>
    /// 提供料理が作成可能かどうか確認する
    /// </summary>
    public bool IsCreateAll()
    {
        foreach (var foodData in m_provideFoodDataList)
        {
            if (foodData == null) continue;
            if (0 < foodData.ProvideNum || 0 < foodData.BonusNum) return true;
        }
        return false;
    }


    /// <summary>
    /// 提供料理を追加できるか確認
    /// </summary>
    public bool IsAddList()
    {
        // 最大数を越えていたら追加しない
        if (m_maxFoodListCount <= m_provideFoodDataList.Count)
        {
            return false;
        }

        return true;
    }


    /// <summary>
    /// 追加済みかどうか確認する
    /// </summary>
    public bool IsAddedProvideFood(FoodID _foodID)
    {
        foreach (var data in m_provideFoodDataList)
        {
            if (data == null) continue;
            if (data.FoodID == _foodID) return true;
        }

        // 存在しなければ
        return false;
    }


    //====================================================
    //              イベントメッセージ
    //====================================================

    // 売り切れイベントを送信
    private void PublishSoldOutProvideFoodEvent(ProvideFoodData _data)
    {
        // 作成可能でなければ
        if (FoodData.IsProvide(m_pocketType, _data.FoodID) == false)
        {
            // イベント送信
            GlobalSoldOutProvideFoodEvent eve = new();
            eve.ProvideFoodData = _data;
            MessageBroker.Default.Publish<GlobalSoldOutProvideFoodEvent>(eve);
        }
    }


    private void PublishAddListEvent(ProvideFoodData _data)
    {
        // イベント送信
        GlobalAddProvideFoodListEvent eve = new();
        eve.ProvideFoodData = _data;
        MessageBroker.Default.Publish<GlobalAddProvideFoodListEvent>(eve);
    }


    private void PublishRemoveListEvent(ProvideFoodData _data)
    {
        // イベント送信
        GlobalRemoveProvideFoodListEvent eve = new();
        eve.ProvideFoodData = _data;
        MessageBroker.Default.Publish<GlobalRemoveProvideFoodListEvent>(eve);
    }


    private void PublishChangeProvideFoodEvent()
    {
        // イベント送信
        GlobalChangeProvideFoodEvent eve = new();
        MessageBroker.Default.Publish<GlobalChangeProvideFoodEvent>(eve);
    }

}
