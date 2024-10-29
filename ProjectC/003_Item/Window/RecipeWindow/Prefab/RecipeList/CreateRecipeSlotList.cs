using UnityEngine;
using ItemInfo;
using PocketItemDataInfo;

public class CreateRecipeSlotList : BaseCreateSlotList
{
    // 制作者 田内
    // レシピスロットを作成する

    [Header("ポケットマネージャーの種類")]
    [SerializeField]
    protected PocketType m_pocketType = PocketType.Inventory;

    override protected void CreateSlotInstance()
    {

        if (m_slot == null)
        {
            Debug.LogError("作成するスロットが登録されていません");
            return;
        }


        var foodList = ItemDataBaseManager.instance.GetItemList(ItemTypeID.Food);

        foreach (var list in foodList)
        {

            // 子オブジェクトにスロットを追加
            var slot = Instantiate(m_slot, transform);

            if (slot.TryGetComponent<RecipeItemSlotData>(out var slotData))
            {
                // アイテムスロットのデータをセット
                var itemData = ItemDataBaseManager.instance.GetItemData(list.ItemTypeID, list.ItemID);
                slotData.SetItemSlotData(itemData, m_pocketType);

                m_slotList.Add(slotData);
            }
            else
            {
                Debug.LogError("RecipeItemSlotDataコンポーネントがアタッチされていません");
            }

            // UIcontrollerに追加
            AddSelectUIControler(slot);

        }
    }

    /// <summary>
    /// 料理が作成可能か確認
    /// </summary>
    public void CheckCreate()
    {
        // レシピスロットを更新
        foreach (var slot in m_slotList)
        {
            if (slot == null) continue;

            if(slot.TryGetComponent<RecipeItemSlotData>(out var recipeSlot))
            {
                recipeSlot.CheckCreate();
            }
        }
    }
}
