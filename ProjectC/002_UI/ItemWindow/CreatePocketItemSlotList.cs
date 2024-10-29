using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PocketItemDataInfo;
using ItemInfo;

public class CreatePocketItemSlotList : BaseCreateSlotList
{
    // 制作者 田内
    // 対応ポケットの所持アイテムスロットを作成する

    protected enum SlotListType
    {
        MaxSlotDisplay, // スロットを最大まで作成する
        MinSlotDisplay, // スロットを最少で作成する
    }

    [Header("アイテムの種類")]
    [SerializeField]
    protected ItemTypeID m_itemTypeID = ItemTypeID.ALL;

    [Header("スロットリストの種類")]
    [SerializeField]
    protected SlotListType m_slotListType = SlotListType.MaxSlotDisplay;

    [Header("ポケットマネージャーの種類")]
    [SerializeField]
    protected PocketType m_pocketType = PocketType.Inventory;

    //==============================================================================

    // スロットを作成する
    override protected void CreateSlotInstance()
    {
        if (m_slot == null)
        {
            Debug.LogError("作成するスロットが登録されていません");
            return;
        }


        // 現在所持しているアイテムリスト
        var itemList = m_pocketType.GetPocketItemDataManager().GetItemList(m_itemTypeID);

        // 作成するスロットサイズ
        int size = GetSlotSize(itemList);

        // スロット作成
        for (int i = 0; i < size; ++i)
        {
            // 子オブジェクトにスロットを追加
            var slot = Instantiate(m_slot, transform);

            if (slot.TryGetComponent<PocketItemSlotData>(out var slotData))
            {
                if (i < itemList.Count)
                {
                    // アイテムスロットのデータをセット
                    var itemData = ItemDataBaseManager.instance.GetItemData(itemList[i].ItemTypeID, itemList[i].ItemID);
                    slotData.SetItemSlotData(itemData, m_pocketType);
                    slotData.SetPocketItemData(itemList[i]);

                }
                else
                {
                    // スロットのデータを初期化し、コンポーネントを削除
                    slotData.InitializeSlotData();
                    Destroy(slotData);
                }
            }
            else
            {
                Debug.LogError("PocketItemSlotDataコンポーネントがアタッチされていません");
            }

            // リストに追加
            m_slotList.Add(slotData);

            // UIcontrollerに追加
            AddSelectUIControler(slot);

        }

    }


    protected int GetSlotSize(List<PocketItemData> _list)
    {
        //　ポケットの最大サイズ
        int size = 0;

        switch (m_slotListType)
        {
            case SlotListType.MaxSlotDisplay:
                {
                    // 最大までスロット作成
                    size = m_pocketType.GetPocketItemDataManager().ListMaxSize;
                    break;
                }

            case SlotListType.MinSlotDisplay:
                {
                    // 最小でスロット作成
                    size = _list.Count;
                    break;
                }
        }

        return size;
    }

}
