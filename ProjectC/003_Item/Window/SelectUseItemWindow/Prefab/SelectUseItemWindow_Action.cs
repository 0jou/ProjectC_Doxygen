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
    ///  // 選択ウィンドウ(アクションアイテムウィンドウ版)作成
    /// </summary>
    #endregion
    private  void EditActionWindow(ItemTypeID _typeID, uint _id)
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
                    EditSelectionWindowforActionWindow_Ingredient(data);
                    break;
                }

            case ItemTypeID.Food:
                {
                    EditSelectionWindowforActionWindow_Food(data);
                    break;
                }
        }

    }


    private void EditSelectionWindowforActionWindow_Ingredient(BaseItemData _itemData)
    {
        AddButton(_id: SelectUseItemID.Put, _active: true, _text: "Put");
        AddButton(_id: SelectUseItemID.Pitch, _active: false, _text: "Throw");//追加（山本）
        AddButton(_id: SelectUseItemID.Eat, _active: false, _text: "Eat");
        AddButton(_id: SelectUseItemID.Exit, _active: true, _text: "Exit");
    }

    private void EditSelectionWindowforActionWindow_Food(BaseItemData _itemData)
    {
        AddButton(_id: SelectUseItemID.Put, _active: true, _text: "Put");
        AddButton(_id: SelectUseItemID.Pitch, _active: true, _text: "Throw");//追加（山本）
        AddButton(_id: SelectUseItemID.Eat, _active: false, _text: "Eat");
        AddButton(_id: SelectUseItemID.Exit, _active: true, _text: "Exit");
    }


}
