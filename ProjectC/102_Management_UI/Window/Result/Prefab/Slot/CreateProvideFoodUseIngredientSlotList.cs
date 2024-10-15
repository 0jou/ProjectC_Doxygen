using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using PocketItemDataInfo;
using FoodInfo;

public class CreateProvideFoodUseIngredientSlotList : BaseCreateSlotList
{
    // 制作者 田内
    // 経営で使用した材料スロットを作成する

    //======================================================
    //                     実行処理
    //======================================================

    protected override void CreateSlotInstance()
    {
        if (m_slot == null)
        {
            Debug.LogError("作成するスロットが登録されていません");
            return;
        }

        // 提供料理リスト
        foreach (var provideFood in ManagementProvideFoodManager.instance.ProvideFoodDataList)
        {
            if (provideFood == null) continue;

            // 料理データ
            var foodData = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)provideFood.FoodID);
            if (foodData == null) continue;

            // 必要材料リスト
            foreach (var need in foodData.NeedIngredientObjectList)
            {
                var ingredientData = ItemDataBaseManager.instance.GetItemData(ItemTypeID.Ingredient, (uint)need.IngredientID);
                if (ingredientData == null) continue;

                // 子オブジェクトにスロットを追加
                var slot = Instantiate(m_slot, transform);

                if (slot.TryGetComponent<ProvideFoodUseIngredientSlotData>(out var slotData))
                {
                    slotData.SetNeedProvideFoodUseIngredient(provideFood, need);
                    slotData.SetItemSlotData(ingredientData, ManagementProvideFoodManager.instance.PocketType);

                    // リストに追加
                    m_slotList.Add(slotData);
                }
                else
                {
                    Debug.LogError("スロットにNeedIngredientSlotDataコンポーネントがアタッチされていません");
                }

                // UIcontrollerに追加
                AddSelectUIControler(slot);
            }
        }
    }
}
