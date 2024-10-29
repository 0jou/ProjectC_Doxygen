using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using PocketItemDataInfo;
using FoodInfo;

public class CreateNeedIngredientSlot : BaseCreateSlotList
{

    // 制作者 田内
    // 料理に必要な材料のスロットを作成する

    [Header("ポケットマネージャーの種類")]
    [SerializeField]
    protected PocketType m_pocketType = PocketType.Inventory;

    //===============================
    // 必要な材料を確認したい料理ID

    protected FoodID m_foodID = 0;


    //======================================================
    //                     実行処理
    //======================================================

    public void SetFoodID(FoodID _id)
    {
        m_foodID = _id;
    }


    protected override void CreateSlotInstance()
    {

        if (m_slot == null)
        {
            Debug.LogError("作成するスロットが登録されていません");
            return;
        }

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

            if (slot.TryGetComponent<NeedIngredientSlotData>(out var slotData))
            {
                // 必要素材用データセット
                slotData.SetNeedIngredientData(needItem);
                slotData.SetItemSlotData(itemData, m_pocketType);

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
