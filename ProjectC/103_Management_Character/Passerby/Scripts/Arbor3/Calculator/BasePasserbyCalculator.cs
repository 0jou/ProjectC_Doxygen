using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class BasePasserbyCalculator : Calculator
{

    // 制作者　田内

    //===========================

    [Header("通行人情報")]
    [SerializeField]
    protected FlexiblePasserbyDataVariable m_flexiblePasserbyDataVariable = null;


    //=========================================
    // 出力

    [Header("出力:座標")]
    [SerializeField]
    protected OutputSlotVector3 m_outputPos = new();

    //==============================================================

    #region メソッド説明
    /// <summary>
    /// 通行人情報が存在するかのチェック
    /// </summary>
    #endregion
    protected PasserbyData GetPasserbyData()
    {

        // スタッフ情報が存在するか確認

        if (m_flexiblePasserbyDataVariable == null)
        {
            Debug.LogError("スタッフ情報がシリアライズされていません");
            return null;
        }

        if (m_flexiblePasserbyDataVariable.value.PasserbyData == null)
        {
            Debug.LogError("スタッフ情報が存在しません");
            return null;
        }

        // 存在する
        return m_flexiblePasserbyDataVariable.value.PasserbyData;

    }

}
