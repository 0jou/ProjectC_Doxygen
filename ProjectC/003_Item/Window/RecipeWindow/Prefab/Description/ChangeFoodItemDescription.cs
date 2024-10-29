using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

using TMPro;
using FoodInfo;

public class ChangeFoodItemDescription : ChangeItemDescription
{

    // 制作者 田内
    // アイテムに加えて料理の説明をセットする


    [Foldout("価格")]
    [Header("価格テキスト")]
    [SerializeField]
    private TextMeshProUGUI m_priceText = null;

    [Foldout("価格")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_priceTextList = new();

    //=====================================================================

    [Foldout("作成可能数")]
    [Header("作成可能数テキスト")]
    [SerializeField]
    private TextMeshProUGUI m_createNumText = null;

    [Foldout("作成可能数")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_createNumTextList = new();

    //=====================================================================

    [Header("必要材料スロットを表示するオブジェクト")]
    [SerializeField]
    private CreateNeedIngredientSlot m_createNeedIngredientSlot = null;


    //=====================================================================
    //                      実行処理
    //=====================================================================


    protected override void SetDescription(BaseItemData _itemData)
    {
        base.SetDescription(_itemData);

        // 必要な材料スロットを作成
        CreateNeedIngredientSlot(_itemData);

        // 価格テキスト
        SetPriceText(_itemData);

        // 作成可能数
        SetCreateNumText(_itemData);

    }

    protected override void InitDescription()
    {
        base.InitDescription();

        SetPriceText(null, false);
        SetCreateNumText(null, false);
    }

    override protected void SetActiveList()
    {
        base.SetActiveList();

        CheckToSetActiveGameObjectList(m_priceText, m_priceTextList);
        CheckToSetActiveGameObjectList(m_createNumText, m_createNumTextList);
    }


    // この料理に必要なスロットを作成
    private void CreateNeedIngredientSlot(BaseItemData _itemData)
    {
        // 必要な材料をセット
        if (m_createNeedIngredientSlot == null)
        {
            Debug.LogError("必要な材料を作成するスクリプトが登録されていません");
            return;
        }

        // 必要な材料を確認したい料理のIDをセットしてから作成
        m_createNeedIngredientSlot.SetFoodID((FoodID)_itemData.ItemID);
        m_createNeedIngredientSlot.CreateSlot();

    }

    private void SetPriceText(BaseItemData _data, bool _active = true)
    {
        if (m_priceText == null) return;

        // 非表示
        m_priceText.gameObject.SetActive(false);

        // 初期化用
        if (!_active) return;

        if (_data is FoodData == false) return;
        var foodData = _data as FoodData;

        // 表示
        m_priceText.gameObject.SetActive(false);

        m_priceText.text = foodData.Price.ToString();
    }


    private void SetCreateNumText(BaseItemData _data, bool _active = true)
    {
        if (m_createNumText == null) return;

        // 非表示
        m_createNumText.gameObject.SetActive(false);

        // 初期化用
        if (!_active) return;


        // 表示
        m_createNumText.gameObject.SetActive(false);

        m_createNumText.text = FoodData.GetCreateNum(m_pocketType, (FoodID)_data.ItemID).ToString();
    }

}
