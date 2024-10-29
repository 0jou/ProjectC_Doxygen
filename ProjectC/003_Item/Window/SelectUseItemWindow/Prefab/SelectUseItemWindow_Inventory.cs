using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using SelectUseItemInfo;

using Cysharp.Threading.Tasks;

public partial class SelectUseItemWindow : BaseWindow
{

    #region メソッド説明
    /// <summary>
    ///  // 選択ウィンドウ(インベントリアイテムウィンドウ版)作成
    /// </summary>
#endregion
    private void EditInventoryWindow(ItemTypeID _typeID, uint _id)
    {
        // アイテムを取得
        var data = ItemDataBaseManager.instance.GetItemData(_typeID, _id);
        if (data == null)
        {
            Debug.LogError("このアイテムのデータは存在しません。登録されているか確認してください");
            return;
        }

        switch (data.ItemTypeID)
        {

            case ItemTypeID.Ingredient:
                {
                    EditSelectionWindowforInventoryWindow_Ingredient(data);
                    break;
                }

            case ItemTypeID.Food:
                {
                    EditSelectionWindowforInventoryWindow_Food(data);
                    break;
                }
        }


    }


    private void EditSelectionWindowforInventoryWindow_Ingredient(BaseItemData _itemData)
    {

        AddButton(_id: SelectUseItemID.Exit, _active: true, _text: "Exit");

    }

    private void EditSelectionWindowforInventoryWindow_Food(BaseItemData _itemData)
    {

        AddButton(_id: SelectUseItemID.Exit, _active: true, _text: "Exit");

    }


}
