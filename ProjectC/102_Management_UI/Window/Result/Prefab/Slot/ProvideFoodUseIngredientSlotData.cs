using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ProvideFoodUseIngredientSlotData : ItemSlotData
{
    // 制作者 田内
    // 経営で使用した材料スロット

    [Header("使用数")]
    [SerializeField]
    private TextMeshProUGUI m_useNumText = null;


    [Header("使用失敗数")]
    [SerializeField]
    private TextMeshProUGUI m_missUseNumText = null;

    //=================================================
    //                  実行処理
    //=================================================

    private void Start()
    {
        m_pocketType = ManagementProvideFoodManager.instance.PocketType;
    }

    public void SetNeedProvideFoodUseIngredient(ProvideFoodData _provideFoodData, FoodData.NeedIngredientObject _needData)
    {
        SetUseNumText(_provideFoodData, _needData);
        SetMissUseNumText(_provideFoodData, _needData);
    }


    private void SetUseNumText(ProvideFoodData _provideFoodData, FoodData.NeedIngredientObject _needData, bool _active = true)
    {

        if (m_useNumText == null) return;

        // 非表示
        m_useNumText.gameObject.SetActive(false);

        // 初期化用
        if (!_active) return;
        if (_provideFoodData == null) return;
        if (_needData == null) return;

        // 使用数
        var num = _provideFoodData.RemoveNum * _needData.Num;

        m_useNumText.gameObject.SetActive(true);
        m_useNumText.text = num.ToString();

    }
    private void SetMissUseNumText(ProvideFoodData _provideFoodData, FoodData.NeedIngredientObject _needData, bool _active = true)
    {

        if (m_missUseNumText == null) return;

        // 非表示
        m_missUseNumText.gameObject.SetActive(false);

        // 初期化用
        if (!_active) return;
        if (_provideFoodData == null) return;
        if (_needData == null) return;

        // 使用数
        var num = _provideFoodData.RemoveNum * _needData.Num;

        // 売れた数
        var soldNum = _provideFoodData.SoldNum * _needData.Num;

        // 失敗数
        var missNum = num - soldNum;

        m_missUseNumText.gameObject.SetActive(true);
        m_missUseNumText.text = missNum.ToString();

    }
}
