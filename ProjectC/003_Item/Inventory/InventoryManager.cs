using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;
using NaughtyAttributes;

public class InventoryManager : BasePocketItemDataController
{
    // インベントリの管理マネージャー(シングルトン)
    // 制作者(田内)

     //==================
    // シングルトン

    public static InventoryManager instance;

    protected virtual void Awake()
    {
        // インスタンスがなければ作成
        if (instance == null)
        {
            instance = (InventoryManager)FindObjectOfType(typeof(InventoryManager));

            DontDestroyOnLoad(gameObject);
        }
        // あれば作成しない
        else
        {
            Destroy(gameObject);
        }
    }


    override public bool AddItem(ItemTypeID _itemTypeID, uint _itemID)
    {
        if(base.AddItem(_itemTypeID, _itemID))
        {
            // 新規アイテムとして所持する
            NewItemManager.instance.AddItem(_itemTypeID, _itemID);

            return true;
        }

        return false;
    }



    override public bool RemoveItem(ItemTypeID _itemTypeID, uint _itemID, int _removeNum = 1)
    {
        if(base.RemoveItem(_itemTypeID,_itemID,_removeNum))
        {
            // 新規アイテムから取り除く
            NewItemManager.instance.RemoveItem(_itemTypeID, _itemID, _removeNum);

            return true;
        }

        return false;
    }


}
