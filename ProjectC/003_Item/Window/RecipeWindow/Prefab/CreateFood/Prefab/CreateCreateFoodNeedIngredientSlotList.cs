using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using PocketItemDataInfo;
using FoodInfo;

public class CreateCreateFoodNeedIngredientSlotList : CreateNeedIngredientSlot
{
    // 制作者 田内

    [Header("コントローラー")]
    [SerializeField]
    private CreateFoodController m_createFoodController = null;

    //==============================================
    //              実行処理
    //==============================================

    protected override void CreateSlotInstance()
    {
        #region nullチェック
        if (m_createFoodController == null)
        {
            Debug.LogError("CreateFoodControllerがシリアライズされていません");
            return;
        }
        if (m_slot == null)
        {
            Debug.LogError("作成するスロットが登録されていません");
            return;
        }
        #endregion

        // IDにあった料理を取得
        var foodData = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)m_foodID);
        if (foodData == null) return;

        // 既存のスロットを削除
        DestroySlotList();

        // 必要な材料オブジェクトリスト
        foreach (var needItem in foodData.NeedIngredientObjectList)
        {
            if (needItem == null) continue;

            var itemData = ItemDataBaseManager.instance.GetItemData(ItemTypeID.Ingredient, (uint)needItem.IngredientID);
            if (itemData == null)
            {
                Debug.LogError("このアイテムのデータは存在しません");
                continue;
            }

            // 子オブジェクトにスロットを追加
            var slot = Instantiate(m_slot, transform);

            if (slot.TryGetComponent<CreateFoodNeedIngredientSlotData>(out var slotData))
            {
                // 必要素材用データセット
                slotData.SetController(m_createFoodController);
                slotData.SetItemSlotData(itemData, m_createFoodController.PocketType);
                slotData.SetNeedIngredientData(needItem);

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
