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

    [Header("作成可能数")]
    [SerializeField]
    protected TextMeshProUGUI m_createNumText = null;

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
        SetCreateNumText(_itemData);
        CreateNeedIngredient();
    }


    override public void InitializeSlotData()
    {
        base.InitializeSlotData();

        SetPriceText(null, false);
        SetCreateNumText(null, false);
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

        // 初期化用
        if (_active == false) return;
        var foodData = _data as FoodData;
        if (foodData == null) return;

        m_priceNumText.text = "価格 : " + foodData.Price.ToString();

        // 表示
        m_priceNumText.gameObject.SetActive(true);

    }

    // 作成可能数
    virtual protected void SetCreateNumText(BaseItemData _data, bool _active = true)
    {
        if (m_createNumText == null) return;

        // 非表示
        m_createNumText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        var foodData = _data as FoodData;
        if (foodData == null) return;


        m_createNumText.text = FoodData.GetCreateNum(m_pocketType, (FoodID)foodData.ItemID).ToString("00");

        // 表示
        m_createNumText.gameObject.SetActive(true);
    }

}
