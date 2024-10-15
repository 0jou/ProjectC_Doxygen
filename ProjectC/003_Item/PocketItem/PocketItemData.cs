using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;

public class PocketItemData 
{

    // 制作者　田内

    public PocketItemData(ItemTypeID _itemTypeID, uint _itemID,int _num)
    {
        m_itemTypeID = _itemTypeID;
        m_itemID = _itemID;
        m_num = _num;
    }


    public static PocketItemData CreateItemData(ItemTypeID _itemTypeID, uint _itemID,int _num)
    {
        return new PocketItemData(_itemTypeID,_itemID,_num);
    }


    //===================================================================
    // アイテム所持数

    [Header("所持数")]
    [SerializeField]
    private int m_num = 1;

    public int Num { get { return m_num; } set { m_num = value; } }

    //==================================================================
    // アイテムの種類ID

    private ItemTypeID m_itemTypeID = new();

    public ItemTypeID ItemTypeID { get { return m_itemTypeID; } set { m_itemTypeID = value; } }

    //===================================================================
    // アイテムID

    private uint m_itemID = 0;

    public uint ItemID { get { return m_itemID; } set { m_itemID = value; } }

}
