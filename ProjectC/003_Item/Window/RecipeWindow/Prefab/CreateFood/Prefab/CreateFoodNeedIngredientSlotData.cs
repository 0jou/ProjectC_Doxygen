using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PocketItemDataInfo;

public class CreateFoodNeedIngredientSlotData : NeedIngredientSlotData
{
    // 制作者 田内
    // CreateFoodWindow用の必要素材スロット

    // コントローラー
    private CreateFoodController m_createFoodController = null;


    public void SetController(CreateFoodController _createFoodController)
    {
        m_createFoodController = _createFoodController;
    }


    override protected void SetComparisonNumText(FoodData.NeedIngredientObject _need, bool _active = true)
    {

        if (m_comparisonNumText == null) return;

        // 非表示
        m_comparisonNumText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (_need == null) return;


        // 現在の所持数
        var num = m_pocketType.GetPocketItemDataManager().GetItemNum(m_itemTypeID, m_itemID);

        // 必要数
        int needNum = _need.Num * m_createFoodController.CurrentCreateNum;

        // 文字列に変換
        string numText = needNum + " / " + num;

        m_comparisonNumText.text = numText;

        m_comparisonNumText.gameObject.SetActive(true);


    }

}
