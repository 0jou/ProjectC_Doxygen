using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;
using FoodInfo;

public class CreateManagementProvideFoodSlotList : BaseCreateSlotList
{
    // 制作者　田内
    // 提供料理のスロットを作成する


    override protected void CreateSlotInstance()
    {
        // まだ作成されていないスロットを作成する
        var provideFoodList = GetProvideFoodList();

        // 作成
        foreach (var data in provideFoodList)
        {
            if (data == null) continue;

            // 子オブジェクトにスロットを追加
            var slot = Instantiate(m_slot, transform);

            if (slot.TryGetComponent<ManagementProvideFoodSlotData>(out var slotData))
            {

                // スロットのデータをセット
                var itemData = ItemDataBaseManager.instance.GetItemData(ItemTypeID.Food, (uint)data.FoodID);
                slotData.SetItemSlotData(itemData, ManagementProvideFoodManager.instance.PocketType);

                // リストに追加
                m_slotList.Add(slotData);

            }
            else
            {
                Debug.LogError("ProvideFoodSlotDataコンポーネントがアタッチされていません");
            }

            // UIcontrollerに追加
            AddSelectUIControler(slot);
        }
    }


    private List<ProvideFoodData> GetProvideFoodList()
    {
        var provideFoodlist = ManagementProvideFoodManager.instance.ProvideFoodDataList;

        List<ProvideFoodData> creatSlotList = new();

        foreach (var data in provideFoodlist)
        {
            // 作成済みか
            bool isCreate = false;

            // 既存のスロットに存在していれば作成しない
            foreach (var slot in m_slotList)
            {
                if (slot == null) continue;

                if (slot.TryGetComponent<ManagementProvideFoodSlotData>(out var slotData))
                {
                    // 既に作成されていれば
                    if ((FoodID)slotData.ItemID == data.FoodID)
                    {
                        isCreate = true;
                        break;
                    }
                }
            }

            // まだ作成されていなければ
            if (isCreate == false) creatSlotList.Add(data);
        }

        return creatSlotList;

    }

}
