using ItemInfo;
using PocketItemDataInfo;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class CreateActionItemSlotList : BaseCreateSlotList
{
    //アクションアイテムウィンドウのスロット作成（山本）


    [Header("ポケットマネージャーの種類")]
    [SerializeField]
    protected PocketType m_pocketType = PocketType.Inventory;


    public GameObject SlotObj { get { return m_slot; } }

    // public void RemoveSlotList(ItemTypeID _itemID, data)


    override protected void CreateSlotInstance()
    {
        if (m_slot == null)
        {
            Debug.LogError("作成するスロットが登録されていません");
            return;
        }

        // まだ作成できていないスロットリスト
        var orderFoodList = GetItemSlotDataList();

        foreach (var food in orderFoodList)
        {
            if (food == null) continue;

            // 子オブジェクトにスロットを追加
            var slot = Instantiate(m_slot, transform);

            //スケール値0.8に（仮）
            slot.transform.localScale = new Vector3(0.8f,0.8f,1.0f);

            var slotData = slot.GetComponent<ItemSlotData>();
            if (slotData)
            {
                // 注文アイテムスロットのデータをセット
                var itemData = ItemDataBaseManager.instance.GetItemData(ItemTypeID.Food, (uint)food.ItemID);
                slotData.SetItemSlotData(itemData, m_pocketType);

            }
            else
            {
                Debug.LogError("ItemSlotDataコンポーネントがアタッチされていません");
            }

            // リストに追加
            m_slotList.Add(slotData);

            // UIcontrollerに追加
            AddSelectUIControler(slot);
        }

    }


    private List<PocketItemData> GetItemSlotDataList()
    {
        var foodList = InventoryManager.instance.GetItemList(ItemTypeID.Food);

        List<PocketItemData> creatSlotList = new();

        foreach (var food in foodList)
        {
            // 作成済みか
            bool isCreate = false;

            // 既存のスロットに存在していれば作成しない
            foreach (var slot in m_slotList)
            {
                if (slot == null) continue;

                var slotData = slot.GetComponent<ItemSlotData>();

                if (slotData.ItemTypeID == food.ItemTypeID &&
                    slotData.ItemID == food.ItemID)
                {
                    isCreate = true;
                    break;
                }
            }

            if (isCreate == false)
            {
                // 作成する
                creatSlotList.Add(food);
            }

        }

        return creatSlotList;

    }

}