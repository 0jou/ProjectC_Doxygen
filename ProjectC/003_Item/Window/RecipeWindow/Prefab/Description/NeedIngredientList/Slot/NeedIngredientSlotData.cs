using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PocketItemDataInfo;

using TMPro;

public class NeedIngredientSlotData : ItemSlotData
{
    // 制作者 田内
    // 必要材料スロット

    [Header("比較用所持数")]
    [SerializeField]
    protected TextMeshProUGUI m_comparisonNumText = null;

    //==========================================================
    //                         実行処理
    //==========================================================

    public void SetNeedIngredientData(FoodData.NeedIngredientObject _need)
    {
        SetComparisonNumText(_need);
    }

    public override void InitializeSlotData()
    {
        base.InitializeSlotData();

        SetComparisonNumText(null, false);
    }

    // 所持数比較用
    virtual protected void SetComparisonNumText(FoodData.NeedIngredientObject _need, bool _active = true)
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
        int needNum = _need.Num;

        // 文字列に変換
        string numText = needNum + " / " + num;

        m_comparisonNumText.text = numText;

        m_comparisonNumText.gameObject.SetActive(true);

    }


}
