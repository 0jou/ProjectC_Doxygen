using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using SelectUseItemInfo;

//Startをウィンドウ生成前に行いたかったので追加しました。（山本）
[DefaultExecutionOrder(-100)]
public class ItemDataBaseManager : BaseManager<ItemDataBaseManager>
{

    // 制作者(田内)

    [Header("アイテムのデータベース")]
    [SerializeField]
    private ItemDataBase m_dataBase = null;


    public ItemDataBase DataBase { get { return m_dataBase; } }


    //=================================================================
    //=================================================================
    //=================================================================

    private void Awake()
    {
        SetID();
        base.Awake();
    }


    private void SetID()
    {

        // IDをセットする
        foreach (var list in m_dataBase.ItemDataBaseList)
        {

            if (list is FoodData)
            {
                var data = list as FoodData;
                data.SetData();
            }
            else if (list is IngredientData)
            {
                var data = list as IngredientData;
                data.SetData();
            }
            else
            {
                list.SetData();
            }
        }

    }


    public BaseItemData GetItemData(ItemInfo.ItemTypeID _itemTypeID, uint _itemID)
    {

        foreach (var list in m_dataBase.ItemDataBaseList)
        {
            // アイテムの種類が一致
            if (list.ItemTypeID == _itemTypeID)
            {
                // 種類が一致
                if (list.ItemID == _itemID)
                {

                    return list;

                }
            }
        }

        Debug.LogError(_itemTypeID.ToString() + _itemID.ToString() + "このIDのアイテムは登録されていません");
        return null;

    }


    public T GetItemData<T>(ItemTypeID _itemTypeID, uint _itemID) where T : BaseItemData
    {

        foreach (var list in m_dataBase.ItemDataBaseList)
        {
            // アイテムの種類が一致
            if (list.ItemTypeID == _itemTypeID)
            {
                // 種類が一致
                if (list.ItemID == _itemID)
                {
                    if (list is T)
                    {
                        var data = list as T;

                        return data;
                    }
                    else
                    {
                        Debug.LogError("ジェネリックを間違えているか、ItemTypeのIDを間違えています。確認してください");
                    }
                }
            }
        }


        Debug.LogError(_itemTypeID + ":" + _itemID + ":" + "このIDのアイテムは登録されていません");
        return null;

    }


    // 引数種類IDのアイテムを全て取得
    public List<BaseItemData> GetItemList(ItemTypeID _id)
    {
        List<BaseItemData> itemList = new();

        foreach (var list in m_dataBase.ItemDataBaseList)
        {
            // アイテムの種類が一致
            if (list.ItemTypeID == _id)
            {

                itemList.Add(list);

            }
        }

        return itemList;

    }


    // 引数IDのアイテムをリストから全て取得
    public List<T> GetItemList<T>(ItemTypeID _id) where T : BaseItemData
    {
        List<T> itemList = new();

        foreach (var list in m_dataBase.ItemDataBaseList)
        {
            // アイテムの種類が一致
            if (list.ItemTypeID == _id)
            {
                if (list is T)
                {
                    var item = list as T;
                    itemList.Add(item);
                }

            }
        }

        return itemList;

    }


    // ジェネリッククラスのアイテムをリストから全て取得
    public List<T> GetItemList<T>() where T : BaseItemData
    {
        List<T> itemList = new();

        foreach (var list in m_dataBase.ItemDataBaseList)
        {
            // キャストできれば
            if (list is T)
            {
                var item = list as T;
                itemList.Add(item);
            }
        }

        return itemList;

    }


}
