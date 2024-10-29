using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PocketItemDataInfo;
using UniRx;

using ItemInfo;

public class CreateGetItemSlotList : MonoBehaviour
{
    // 制作者 田内
    // 取得アイテムUIを作成する

    [Header("ポケットタイプ")]
    [SerializeField]
    private PocketType m_pocketType = PocketType.Inventory;

    [Header("表示する種類")]
    [SerializeField]
    private ItemTypeID m_itemTypeID = ItemTypeID.ALL;

    [Header("アイテム取得UI")]
    [SerializeField]
    private ItemSlotData m_getItemSlotData = null;

    //======================================
    //              実行処理
    //======================================

    private void Start()
    {
        Create();
    }

    private void Create()
    {
        // アイテム追加イベントを受信
        MessageBroker.Default.Receive<GlobalAddItemEvent>().Subscribe(_ =>
        {
            if (m_pocketType.GetPocketItemDataManager() == _.PocketItemDataController)
            {
                // 一致すれば
                if (_.ItemType == m_itemTypeID || m_itemTypeID == ItemTypeID.ALL)
                {
                    var data = ItemDataBaseManager.instance.GetItemData(_.ItemType, _.ItemID);
                    if (data == null) return;

                    var ui = Instantiate(m_getItemSlotData, transform);
                    ui.SetItemSlotData(data, m_pocketType);
                }
            }
        }
        ).AddTo(this);
    }


}
