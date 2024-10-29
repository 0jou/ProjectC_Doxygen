using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using FoodInfo;
using PocketItemDataInfo;
using TMPro;

public class ProvideFoodRecipeSlotData : RecipeItemSlotData
{
    // 制作者 田内
    // 提供料理用スロット

    [Header("提供可能数")]
    [SerializeField]
    private TextMeshProUGUI m_provideNum = null;

    //============================================================================
    //               実行処理
    //============================================================================


    void Start()
    {
        m_pocketType = ManagementProvideFoodManager.instance.PocketType;
    }


    // スロットのデータをセットする
    public override void SetItemSlotData(BaseItemData _itemData, PocketType _pocketType)
    {
        base.SetItemSlotData(_itemData, _pocketType);

        SetProvideNumText(_itemData);
    }


    override public void CheckCreate()
    {
        // 作成可能かを確認
        m_isCreate = FoodData.IsProvide(m_pocketType, (FoodID)m_itemID);

        // 色をセット
        SetSlotColor();
    }


    // 作成可能数
    virtual protected void SetProvideNumText(BaseItemData _data, bool _active = true)
    {
        if (m_provideNum == null) return;

        // 非表示
        m_provideNum.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        var foodData = _data as FoodData;
        if (foodData == null) return;

        m_provideNum.text = FoodData.GetProvideNum(m_pocketType, (FoodID)foodData.ItemID).ToString("00");

        // 表示
        m_provideNum.gameObject.SetActive(true);
    }



}
