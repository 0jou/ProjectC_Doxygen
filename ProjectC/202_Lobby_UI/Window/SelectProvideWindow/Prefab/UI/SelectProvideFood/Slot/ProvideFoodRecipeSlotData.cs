using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using FoodInfo;
using PocketItemDataInfo;
using TMPro;

public class ProvideFoodRecipeSlotData : RecipeItemSlotData
{
    // 制作者 田内


    [Header("作成可能数を表示するTextMeshProUGUI")]
    [SerializeField]
    private TextMeshProUGUI m_provideNumText = null;


    //============================================================================
    //               実行処理
    //============================================================================


    void Start()
    {
        m_pocketType = ManagementProvideFoodManager.instance.PocketType;
    }

    // アイテムスロットのデータをセットする
    override public void SetItemSlotData(BaseItemData _itemData, PocketType _pocketType)
    {
        base.SetItemSlotData(_itemData, _pocketType);

        SetProvideNumText();
    }

    // 初期化する
    override public void InitializeSlotData()
    {
        base.InitializeSlotData();

        SetProvideNumText(false);
    }


    // 提供数
    private void SetProvideNumText(bool _active = true)
    {
        if (m_provideNumText == null)
        {
            // m_provideNumTextがシリアライズされていません
            return;
        }

        // 非表示
        m_provideNumText.gameObject.SetActive(false);

        // 初期化用
        if (!_active) return;

        // 表示
        m_provideNumText.gameObject.SetActive(true);

        // 作成可能テキストをセット
        int num = FoodData.GetCreateNum(m_pocketType, (FoodID)m_itemID);

        m_provideNumText.text = "×" + num.ToString();

    }
}
