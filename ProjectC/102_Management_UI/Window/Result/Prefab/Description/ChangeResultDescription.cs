using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using NaughtyAttributes;
using CameCustomerInfo;

public class ChangeResultDescription : MonoBehaviour
{
    // 制作者 田内
    // 経営リザルトの説明文

    [Header("総提供数")]
    [SerializeField]
    private TextMeshProUGUI m_soldNumText = null;

    [Header("稼いだ金額")]
    [SerializeField]
    private TextMeshProUGUI m_earnedMoneyText = null;

    [Header("総額")]
    [SerializeField]
    private TextMeshProUGUI m_totalEarnedMoneyText = null;

    [Header("個別提供料理スロット作成")]
    [SerializeField]
    private CreateProvideFoodSlotList m_createProvideFoodSlotList = null;

    [Header("使用素材スロット作成")]
    [SerializeField]
    private CreateProvideFoodUseIngredientSlotList m_createProvideFoodUseIngredientSlotList = null;

    //==============================================================

    [BoxGroup("来客数")]
    [Header("来客数")]
    [SerializeField]
    private TextMeshProUGUI m_cameCustomerNumText = null;

    [BoxGroup("来客数")]
    [Header("通常来客数")]
    [SerializeField]
    private TextMeshProUGUI m_cameNormalCustomerNumText = null;

    [BoxGroup("来客数")]
    [Header("怒った来客数")]
    [SerializeField]
    private TextMeshProUGUI m_cameAngryCustomerNumText = null;


    //==============================================================
    //                      実行処理
    //==============================================================


    virtual public void OnInitialize()
    {
        SetDescription();
    }


    protected void SetDescription()
    {
        CreateProvideFoodSlot();

        CreateProvideFoodUseIngredientSlot();

        SetEarnedMoneyText();

        SetSoldNumText();

        SetCameCustomerNumText();

        SetCameNormalCustomerNumText();

        SetCameAngryCustomerNumText();

        SetTotalEarnedMoneyText();
    }


    // 総提供数をセット
    private void SetSoldNumText()
    {
        if (m_soldNumText == null)
        {
            // SoldNumTextがシリアライズされていません
            return;
        }
        m_soldNumText.gameObject.SetActive(false);
        m_soldNumText.gameObject.SetActive(true);

        uint soldNum = 0;

        foreach (var data in ManagementProvideFoodManager.instance.ProvideFoodDataList)
        {
            if (data == null) continue;
            soldNum += data.SoldNum;
        }

        m_soldNumText.text = soldNum.ToString("000");
    }


    // 提供料理スロットを作成
    private void CreateProvideFoodSlot()
    {
        if (m_createProvideFoodSlotList == null)
        {
            Debug.LogError("CreateProvideFoodSlotListがシリアライズされていません");
            return;
        }

        m_createProvideFoodSlotList.CreateSlot();

    }

    // 使用食材スロットを作成
    private void CreateProvideFoodUseIngredientSlot()
    {
        if (m_createProvideFoodUseIngredientSlotList == null)
        {
            Debug.LogError("CreateProvideFoodUseIngredientSlotListがシリアライズされていません");
            return;
        }

        m_createProvideFoodUseIngredientSlotList.CreateSlot();
    }


    // 稼いだ金額
    private void SetEarnedMoneyText()
    {
        if (m_earnedMoneyText == null)
        {
            // テキストがシリアライズされていません
            return;
        }

        m_earnedMoneyText.gameObject.SetActive(false);
        m_earnedMoneyText.gameObject.SetActive(true);

        m_earnedMoneyText.text = ManagementGameManager.instance.EarnedMoney.ToString();

    }


    // 総額
    private void SetTotalEarnedMoneyText()
    {
        if (m_totalEarnedMoneyText == null)
        {
            // テキストがシリアライズされていません
            return;
        }

        m_totalEarnedMoneyText.gameObject.SetActive(false);
        m_totalEarnedMoneyText.gameObject.SetActive(true);

        m_totalEarnedMoneyText.text = ManagementDataManager.instance.TotalEarnedMoney.ToString();

    }

    // 来客数テキストをセット
    private void SetCameCustomerNumText()
    {
        if (m_cameCustomerNumText == null)
        {
            // 来客数テキストがシリアライズされていません
            return;
        }

        m_cameCustomerNumText.gameObject.SetActive(false);
        m_cameCustomerNumText.gameObject.SetActive(true);

        uint cameCustomerNum = 0;

        foreach (var num in ManagementGameManager.instance.CameCustomerNumDictionary)
        {
            cameCustomerNum += num.Value;
        }

        m_cameCustomerNumText.text = cameCustomerNum.ToString();

    }


    // 通常の来客数テキストをセット
    private void SetCameNormalCustomerNumText()
    {
        if (m_cameNormalCustomerNumText == null)
        {
            // 通常の来客数テキストがシリアライズされていません
            return;
        }

        m_cameNormalCustomerNumText.gameObject.SetActive(false);
        m_cameNormalCustomerNumText.gameObject.SetActive(true);

        uint num = 0;
        ManagementGameManager.instance.CameCustomerNumDictionary.TryGetValue(CameCustomerID.Normal, out num);


        m_cameNormalCustomerNumText.text = num.ToString();

    }

    // 怒って帰った来客数テキストをセット
    private void SetCameAngryCustomerNumText()
    {
        if (m_cameAngryCustomerNumText == null)
        {
            // 怒った来客数テキストがシリアライズされていません
            return;
        }

        m_cameNormalCustomerNumText.gameObject.SetActive(false);
        m_cameNormalCustomerNumText.gameObject.SetActive(true);

        uint num = 0;
        ManagementGameManager.instance.CameCustomerNumDictionary.TryGetValue(CameCustomerID.Angry, out num);


        m_cameAngryCustomerNumText.text = num.ToString();

    }

}
