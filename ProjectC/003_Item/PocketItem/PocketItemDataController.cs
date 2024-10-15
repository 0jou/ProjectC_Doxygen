using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;

namespace PocketItemDataInfo
{
    public enum PocketType
    {
        Inventory = 0,              // インベントリ
        Storehouse = 1,             // 倉庫
        NewItem = 2,                // 新規アイテム
        ManagementStorage = 3,      // 経営用ストレージ
    }


    public static class PocketItemDataInfoExtensions
    {
        public static BasePocketItemDataController GetPocketItemDataManager(this PocketType _pocketType)
        {
            switch (_pocketType)
            {
                case PocketType.Inventory:
                    {

                        return InventoryManager.instance;
                    }
                case PocketType.Storehouse:
                    {
                        return StorehouseManager.instance;
                    }
                case PocketType.NewItem:
                    {
                        return NewItemManager.instance;
                    }
                case PocketType.ManagementStorage:
                    {
                        return ManagementStorageManager.instance;
                    }
            }

            Debug.LogError(_pocketType + "に対するマネージャーが存在しません");

            return null;
        }
    }

}

public partial class BasePocketItemDataController : MonoBehaviour
{
    // アイテムの管理を行うコントローラー
    // 制作者(田内)

    //========================
    // 合計で所持できる数


    [Header("材料や料理を含めた、合計所持数")]
    [SerializeField]
    protected int m_listMaxSize = 20;


    public int ListMaxSize { get { return m_listMaxSize; } }


    //=========================
    // 所持している料理

    protected List<PocketItemData> m_itemDataList = new();

    public List<PocketItemData> ItemDataList { get { return m_itemDataList; } }

    //==================================================================================


    private void Start()
    {
        // デフォルトのデータをリストに追加
        AddDefaultItemData();
    }


    //======================================================
    //                  実行処理
    //======================================================

    #region メソッド説明
    ///--------------------------------------
    /// <summary>
    /// アイテムリストに引数アイテムを追加する
    /// </summary>
    /// -------------------------------------
    /// <param name="_itemTypeID">
    /// 追加したいアイテムの種類
    /// </param>
    /// --------------------------------------
    /// <param name="_itemID">
    /// 追加したいアイテムのID
    /// </param>
    /// --------------------------------------
    /// /// <returns>
    /// true - 追加できた
    /// false - 追加できなかった
    /// </returns>
    /// --------------------------------------
    #endregion
    // アイテムリストに引数料理を追加する
    virtual public bool AddItem(ItemTypeID _itemTypeID, uint _itemID)
    {

        var data = ItemDataBaseManager.instance.GetItemData(_itemTypeID, _itemID);
        if (data == null) return false;


        // 現在所持しているアイテムから探す
        foreach (var list in m_itemDataList)
        {
            if (list.ItemTypeID != _itemTypeID || list.ItemID != _itemID) continue;
            if (data.MaxNum <= list.Num) continue;

            // 所持数を追加
            list.Num++;

            return true;
        }

        // 所持しているアイテムに追加できなければ新規作成
        if (IsInList())
        {
            // アイテムデータを作成して追加
            var pocketItemData = PocketItemData.CreateItemData(_itemTypeID, _itemID, 1);
            m_itemDataList.Add(pocketItemData);
            return true;
        }

        // 追加できなければ
        return false;
    }




    #region メソッド説明
    ///--------------------------------------
    /// <summary>
    /// アイテムリストに存在する引数アイテムを減算/削除する
    /// </summary>
    /// -------------------------------------
    /// <param name="_itemTypeID">
    /// 減算/削除したいアイテムの種類
    /// </param>
    /// --------------------------------------
    /// <param name="_itemID">
    /// 減算/削除したいアイテムのID
    /// </param>
    　/// --------------------------------------
    /// <param name="_removeNum">
    /// 減算数
    /// </param>
    /// --------------------------------------
    /// /// <returns>
    /// true - 減算/削除できた
    /// false - 減算/削除できなかった
    /// </returns>
    /// --------------------------------------
    #endregion
    virtual public bool RemoveItem(ItemTypeID _itemTypeID, uint _itemID, int _removeNum = 1)
    {

        PocketItemData targetData = null;

        // 検索
        foreach (var data in m_itemDataList)
        {
            if (data.ItemTypeID != _itemTypeID || data.ItemID != _itemID) continue;

            if (targetData == null)
            {
                targetData = data;
                continue;
            }
            else
            {
                // 一番所持数が少ないものから取り除く
                if (data.Num <= targetData.Num)
                {
                    targetData = data;
                    continue;
                }
            }


        }

        // ターゲットデータの所持数を減らす
        if (targetData != null)
        {
            // 減算
            targetData.Num -= _removeNum;

            // もし手持ちが0になれば取り除く
            if (targetData.Num <= 0) m_itemDataList.Remove(targetData);

            return true;
        }


        // 削除できなかった場合
        return false;

    }


    #region メソッド説明
    ///--------------------------------------
    /// <summary>
    /// ポケット空き容量が最大数に達しているか確認するメソッド
    /// </summary>
    /// --------------------------------------
    /// <returns>
    /// true - 達していない(取得可能)
    /// false - 達している(取得不可)
    /// </returns>
    /// --------------------------------------
    #endregion
    virtual public bool IsInList()
    {

        if (m_listMaxSize <= m_itemDataList.Count)
        {

            Debug.Log("リストの最大数を超えているため、所持できません");

            return false;
        }


        return true;

    }




    //=====================================================================================
    //                               取得用メソッド
    //=====================================================================================

    #region メソッド説明
    ///--------------------------------------
    /// <summary>
    /// 引数IDのアイテム所持数を取得するメソッド
    /// </summary>
    /// -------------------------------------
    /// <param name="_itemTypeID">
    /// 取得したいアイテムの種類
    /// </param>
    /// -------------------------------------
    /// <param name="_itemID">
    /// 取得したいアイテムのID
    /// </param>
    /// --------------------------------------
    /// <returns>
    /// 引数IDのアイテム所持数
    /// </returns>
    /// --------------------------------------
    #endregion
    // 引数IDのアイテム所持数を取得
    virtual public int GetItemNum(ItemTypeID _itemTypeID, uint _itemID)
    {
        // 所持数
        int num = 0;

        // 検索
        foreach (var list in m_itemDataList)
        {

            if (list.ItemTypeID != _itemTypeID)
            {
                continue;
            }

            if (list.ItemID != _itemID)
            {
                continue;
            }

            num += list.Num;

        }

        // 当てはまったデータを返す
        return num;

    }



    virtual public List<PocketItemData> GetItemList(ItemTypeID _typeID)
    {

        List<PocketItemData> itemList = new();

        foreach (var list in m_itemDataList)
        {
            if (_typeID == ItemTypeID.ALL || list.ItemTypeID == _typeID)
            {
                itemList.Add(list);
            }
        }

        return itemList;
    }


    virtual public bool IsHave(ItemTypeID _itemTypeID)
    {
        foreach (var data in m_itemDataList)
        {
            if (data == null) continue;

            // 保持していれば
            if (data.ItemTypeID == _itemTypeID)
            {
                return true;
            }
        }

        // 保持していなければ
        return false;
    }


    virtual public PocketItemData GetRandomItemData(ItemTypeID _itemTypeID)
    {
        // リストを用意
        var itemList = m_itemDataList;

        // シャッフル
        for (int i = itemList.Count - 1; i > 0; i--)
        {
            var j = Random.Range(0, i + 1);
            var temp = itemList[i];
            itemList[i] = itemList[j];
            itemList[j] = temp;
        }

        foreach (var data in itemList)
        {
            // 一致するものがあれば返す
            if (data.ItemTypeID == _itemTypeID)
            {
                return data;
            }
        }

        // 存在しなければnull
        return null;

    }


}
