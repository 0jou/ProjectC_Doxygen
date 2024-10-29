using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PocketItemDataInfo;
using ItemInfo;
using FoodInfo;

public class SelectManagementProvideFoodController : MonoBehaviour
{
    // 制作者 田内
    // 経営提供料理を操作するコントローラー


    //======================================
    // 提供する料理


    private List<FoodID> m_provideFoodIDList = new();

    public List<FoodID> ProvideFoodIDList
    {
        get { return m_provideFoodIDList; }
    }


    private bool m_isListChange = false;

    public bool IsListChange
    {
        get { return m_isListChange; }
    }


    //===============================================
    //                 実行処理
    //===============================================

    public void OnLateUpdate()
    {
        m_isListChange = false;
    }


    /// <summary>
    /// 提供予定料理を追加
    /// </summary>
    public void AddProvideFoodID(FoodID _id)
    {
        if (IsAddList() == false) return;
        if (IsAddedProvideFood(_id) == true) return;

        m_provideFoodIDList.Add(_id);

        m_isListChange = true;
    }


    /// <summary>
    /// 提供予定料理を取り除く
    /// </summary>
    public void RemoveProvideFoodID(FoodID _id)
    {
        if (m_provideFoodIDList.Remove(_id))
        {
            m_isListChange = true;
        }
    }


    /// <summary>
    /// 提供料理を追加できるか確認
    /// </summary>
    public bool IsAddList()
    {
        // 最大数を越えていたら追加しない
        if (ManagementProvideFoodManager.instance.MaxFoodListCount <= m_provideFoodIDList.Count) return false;
        return true;
    }


    /// <summary>
    /// 追加済みかどうか確認する
    /// </summary>
    public bool IsAddedProvideFood(FoodID _foodID)
    {
        foreach (var id in m_provideFoodIDList)
        {
            if (id == _foodID) return true;
        }
        return false;
    }


    /// <summary>
    /// 提供料理をマネージャーにセットする
    /// </summary>
    public void SetManagementProvideFoodData()
    {
        ManagementProvideFoodManager.instance.SetProvideFoodControllerData(this);
    }


}
