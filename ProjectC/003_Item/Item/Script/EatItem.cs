using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;


public class EatItem : MonoBehaviour
{
    // 制作者 田内

    //===================================================================================

    private ItemTypeID m_itemTypeID = new();

    public ItemTypeID ItemTypeID { set { m_itemTypeID = value; } get { return m_itemTypeID; } }


    private uint m_itemID = new();

    public uint ItemID { set { m_itemID = value; } get { return m_itemID; } }


    //===================================================================================


    // アイテムを食べる
    // TODO:現在はアイテムを消去するだけなので、ターゲットに渡す
    public void Eat()
    {

        var data = ItemDataBaseManager.instance.GetItemData(m_itemTypeID, m_itemID);
        if (data == null)
        {
            Debug.LogError("このアイテムのデータは存在しません");
            return;
        }


        // 作成したアイテムをインベントリから削除する
        InventoryManager.instance.RemoveItem(data.ItemTypeID, data.ItemID);

    }
}
