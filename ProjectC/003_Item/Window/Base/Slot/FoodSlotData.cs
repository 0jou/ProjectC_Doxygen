using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FoodInfo;
using PocketItemDataInfo;
using TMPro;

public class FoodSlotData : ItemSlotData
{
    // 制作者 田内
    // 料理スロット

    //===========================================================

    [Header("値段を表示するTextMeshProUGUI")]
    [SerializeField]
    protected TextMeshProUGUI m_priceNumText = null;

    //===========================================================

    [Header("必要な材料スロットを作成するCreateNeedIngredientSlot")]
    [SerializeField]
    protected CreateNeedIngredientSlot m_createNeedIngredientSlot = null;


    //============================================================================
    //                      実行処理
    //============================================================================

    // アイテムスロットのデータをセットする
    override public void SetItemSlotData(BaseItemData _itemData, PocketType _pocketType)
    {
        base.SetItemSlotData(_itemData, _pocketType);

        SetPriceText(_itemData);
        CreateNeedIngredient();
    }

    // 初期化する
    override public void InitializeSlotData()
    {
        base.InitializeSlotData();

        SetPriceText(null, false);
        CreateNeedIngredient(false);
    }

    // 必要材料スロットの作成
    virtual protected void CreateNeedIngredient(bool _active = true)
    {
        if (m_createNeedIngredientSlot == null)
        {
            // 必要な材料スロット作成クラスがシリアライズされていません
            return;
        }

        // 表示
        m_createNeedIngredientSlot.gameObject.SetActive(false);

        // 初期化用
        if (!_active) return;

        // 表示
        m_createNeedIngredientSlot.gameObject.SetActive(true);

        m_createNeedIngredientSlot.SetFoodID((FoodID)m_itemID);
        m_createNeedIngredientSlot.CreateSlot();

    }


    // 値段
    virtual protected void SetPriceText(BaseItemData _data, bool _active = true)
    {
        if (m_priceNumText == null)
        {
            // 値段を表示するテキストがシリアライズされていません
            return;
        }

        // 非表示
        m_priceNumText.gameObject.SetActive(false);


        var foodData = _data as FoodData;
        if (foodData == null) return;

        // 初期化用
        if (!_active) return;

        // 表示
        m_priceNumText.gameObject.SetActive(true);

        m_priceNumText.text = "価格 : " + foodData.Price.ToString();
    }

}
