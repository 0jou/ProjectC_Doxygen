using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using FoodInfo;

using NaughtyAttributes;

public partial class ChangeItemDescription : MonoBehaviour
{
    // 制作者 田内
    // 料理説明文


    [Foldout("料理")]
    [Header("-------------------------------------------------------")]
    [Header("価格テキスト")]
    [SerializeField]
    private TextMeshProUGUI m_priceText = null;

    [Foldout("料理")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_priceTextList = new();

    //=====================================================================

    [Foldout("料理")]
    [Header("-------------------------------------------------------")]
    [Header("作成可能数テキスト")]
    [SerializeField]
    private TextMeshProUGUI m_createNumText = null;

    [Foldout("料理")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_createNumTextList = new();

    //=====================================================================

    [Foldout("料理")]
    [Header("-------------------------------------------------------")]
    [Header("必要材料スロットを表示するオブジェクト")]
    [SerializeField]
    private CreateNeedIngredientSlot m_createNeedIngredientSlot = null;


    //=====================================================================
    //                      実行処理
    //=====================================================================


    private void SetFoodDescription(BaseItemData _itemData)
    {
        // 必要な材料スロットを作成
        CreateNeedIngredientSlot(_itemData);

        // 価格テキスト
        SetPriceText(_itemData);

        // 作成可能数
        SetCreateNumText(_itemData);

    }

    private void InitilizeFoodDescription()
    {

        SetPriceText(null, false);

        SetCreateNumText(null, false);
    }

    private void SetFoodActiveList()
    {
        CheckToSetActiveGameObjectList(m_priceText, m_priceTextList);

        CheckToSetActiveGameObjectList(m_createNumText, m_createNumTextList);
    }


    // この料理に必要なスロットを作成
    private void CreateNeedIngredientSlot(BaseItemData _itemData)
    {
        if (m_createNeedIngredientSlot == null) return;

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
        if (_active == false) return;

        var foodData = CastFoodData(_data);
        if (foodData == null) return;

        m_priceText.text = foodData.Price.ToString();

        // 表示
        m_priceText.gameObject.SetActive(false);
    }


    private void SetCreateNumText(BaseItemData _data, bool _active = true)
    {
        if (m_createNumText == null) return;

        // 非表示
        m_createNumText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (_data == null) return;

        m_createNumText.text = FoodData.GetCreateNum(m_pocketType, (FoodID)_data.ItemID).ToString();

        // 表示
        m_createNumText.gameObject.SetActive(false);

    }


    /// <summary>
    /// 引数アイテムデータを料理データにキャストする
    /// </summary>
    protected FoodData CastFoodData(BaseItemData _itemData)
    {
        if (_itemData == null) return null;
        if (_itemData is FoodData == false) return null;

        // 料理アイテムデータを取得
        return _itemData as FoodData;
    }


}
