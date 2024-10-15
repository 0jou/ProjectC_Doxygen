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

[System.Serializable]
public class ProvideFoodData
{
    // 料理ID
    private FoodID m_foodID = FoodID.Omelette;

    public FoodID FoodID
    {
        get { return m_foodID; }
        set { m_foodID = value; }
    }

    // 売れた数
    private uint m_soldNum = 0;

    public uint SoldNum
    {
        get { return m_soldNum; }
        set { m_soldNum = value; }
    }

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

    //======================================
    // 提供する料理


    private List<ProvideFoodData> m_provideFoodDataList = new();

    public List<ProvideFoodData> ProvideFoodDataList
    {
        get { return m_provideFoodDataList; }
    }


    private bool m_isListChange = false;

    public bool IsListChange
    {
        get { return m_isListChange; }
    }

    //======================================================================-

    private void LateUpdate()
    {
        m_isListChange = false;
    }



    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        m_provideFoodDataList.Clear();
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

        if (IsAddedProvideFood(_foodID))
        {
            // 既に追加されていれば追加しない
            return false;
        }

        ProvideFoodData provideFoodData = new();
        provideFoodData.FoodID = _foodID;

        // リストに追加
        m_provideFoodDataList.Add(provideFoodData);

        // 変更が加えられた
        m_isListChange = true;

        return true;
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

            // 取り除いた数
            data.RemoveNum++;

            // 必要な材料を取り除く
            FoodData.RemoveNeedIngredient(m_pocketType, data.FoodID);

            // 売り切れイベントの確認・送信
            PublishSoldOutProvideFoodEvent(data);

            // リスト変更イベントを送信
            PublishChangeProvideFoodEvent();

            break;
        }
    }


    /// <summary>
    /// (※注意)提供料理をリストから完全に取り除く
    /// </summary>
    public bool RemoveProvideFoodList(FoodID _foodID)
    {

        List<ProvideFoodData> removeList = new();

        foreach (var data in m_provideFoodDataList)
        {
            if (data == null) continue;

            if (data.FoodID == _foodID)
            {
                removeList.Add(data);
            }
        }

        // 取り除く
        bool flg = false;
        foreach (var data in removeList)
        {
            // リストから取り除く
            if (m_provideFoodDataList.Remove(data))
            {
                // 変更が加えられた
                m_isListChange = true;
                flg = true;
            }
        }

        return flg;
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

            if (data.FoodID == _foodID)
            {
                // リスト内に存在すれば
                return data;
            }
        }

        // 存在しなければ
        return null;

    }


    /// <summary>
    /// 提供可能数を取得する
    /// </summary>
    public int GetProvidePossibleNum(FoodID _foodID)
    {
        var data = GetProvideFood(_foodID);
        if (data == null) return 0;

        return FoodData.GetCreateNum(m_pocketType, data.FoodID);
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
            if (FoodData.IsCreate(m_pocketType, provideFood.FoodID))
            {
                // 料理データを返す
                return ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)provideFood.FoodID);
            }

        }

        // 作成できる料理が無くなればnullを返す
        return null;

    }


    /// <summary>
    /// 提供料理が何かしらセットされているかどうか
    /// </summary>
    public bool IsSetProvideFood()
    {
        if (m_provideFoodDataList.Count <= 0)
        {
            // セットされていなければ
            return false;
        }
        else
        {
            // セットされていれば
            return true;
        }
    }



    /// <summary>
    /// 提供料理が作成可能かどうか確認する
    /// </summary>
    public bool IsCreate()
    {
        foreach (var foodData in m_provideFoodDataList)
        {
            if (foodData == null) continue;

            // 作成可能な料理があれば
            if (FoodData.IsCreate(m_pocketType, foodData.FoodID))
            {
                return true;
            }

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

            if (data.FoodID == _foodID)
            {
                // リスト内に存在すれば
                return true;
            }
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
        if (FoodData.IsCreate(m_pocketType, _data.FoodID) == false)
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


    private void PublishChangeProvideFoodEvent()
    {
        // イベント送信
        GlobalChangeProvideFoodEvent eve = new();
        MessageBroker.Default.Publish<GlobalChangeProvideFoodEvent>(eve);
    }

}
