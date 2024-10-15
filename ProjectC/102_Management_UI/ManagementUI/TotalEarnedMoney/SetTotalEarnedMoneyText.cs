using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class SetTotalEarnedMoneyText : BaseChangeText
{
    // 制作者 田内
    // 現在の総額を表示する

    // 保存用
    private uint m_keepValue = 0;

    //====================================
    //          実行処理
    //====================================

    override protected void ChangeText()
    {
        if (m_text == null)
        {
            Debug.LogError("テキストがシリアライズされていません");
            return;
        }

        uint total = ManagementDataManager.instance.TotalEarnedMoney;

        if (m_keepValue != total)
        {
            m_keepValue = total;

            m_text.text = total.ToString();

            PlayEffect();
            PlayDoTween();
        }
    }


}
