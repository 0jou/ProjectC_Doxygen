using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;

public class ItemTypeDataBaseManager : BaseManager<ItemTypeDataBaseManager>
{
    // 制作者(田内)

    [Header("アイテム種類のデータベース")]
    [SerializeField]
    private ItemTypeDataBase m_dataBase = null;


    public ItemTypeDataBase DataBase { get { return m_dataBase; } }


    //===============================================================================
    //===============================================================================
    //===============================================================================


    #region メソッド説明
    ///--------------------------------------
    /// <summary>
    /// 引数IDのItemTypeDataを返す
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 引数IDと一致するItemData(一致しなければnullを返す)
    /// </returns>
    /// --------------------------------------
    #endregion
    // 引数IDのItemTypeDataを返す
    public ItemTypeData GetItemTypeData(ItemTypeID _id)
    {

        // 返すデータ
        ItemTypeData data = null;


        foreach (var list in m_dataBase.ItemTypeDataBaseList)
        {


#if DEBUG
            if (!list)
            {

                // データベースがnullでないか確認する
                if (data != null)
                {
                    Debug.LogError("状態データが登録されていません");
                }

                continue;

            }


            if (list.ItemTypeID == _id)
            {

                // デバッグ時はIDが被っていないか確認する
                if (data != null)
                {
                    Debug.LogError("IDが2つ存在します。確認してください");
                }

                data = list;

            }


#else


            if (!list)
            {

                continue;

            }

            // 一致すれば
            if (list.ItemTypeID == _id)
            {
                data = list;
                break;
            }

#endif

        }


        if (data == null)
        {
            Debug.LogError(_id + " このIDのItemTypeは存在しません。登録できているか確認してください");
        }

        return data;

    }

}
