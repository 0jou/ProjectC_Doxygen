using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;
using Cysharp.Threading.Tasks;

using FoodInfo;

public class SelectFoodDetailUI : DetailUI
{
    // 制作者 田内
    // 選択中の料理スロットの詳細を表示する
    // 基本は非表示だが、ボタンを押すことで表示される

    [Header("SelectUIコントローラー")]
    [SerializeField]
    protected SelectUIController m_recipeSelectUIController = null;

    [Header("必要材料スロット作成")]
    [SerializeField]
    protected CreateNeedIngredientSlot m_createNeedIngredientSlot = null;

    //======================================================
    //                     実行処理
    //======================================================


    public override async UniTask OnInitialize()
    {
        await base.OnInitialize();

        SetCreateNeedIngredientSlot();

        await UniTask.CompletedTask;
    }


    override public async UniTask OnUpdate()
    {

        await base.OnUpdate();

        #region nullチェック

        if (m_recipeSelectUIController == null)
        {
            Debug.LogError("SelectUIコントローラーがシリアライズされていません");
            return;
        }

        #endregion

        if (m_recipeSelectUIController.IsSelectChangeFlg == false) return;


        SetCreateNeedIngredientSlot();

        await UniTask.CompletedTask;

    }

    // 必要な材料スロットを作成
    private void SetCreateNeedIngredientSlot()
    {

        #region nullチェック

        if (m_createNeedIngredientSlot == null)
        {
            // CreateNeedIngredientSlotがシリアライズされていません
            return;
        }

        if (m_recipeSelectUIController == null)
        {
            Debug.LogError("SelectUIコントローラーがシリアライズされていません");
            return;
        }

        #endregion


        // 選択中のUIから取得
        if (m_recipeSelectUIController.CurrentSelectUI.TryGetComponent<RecipeItemSlotData>(out var itemSlotData) == false) return;

        var data = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, itemSlotData.ItemID);
        if (data == null) return;


        m_createNeedIngredientSlot.SetFoodID((FoodID)data.ItemID);
        m_createNeedIngredientSlot.CreateSlot();

    }



}
