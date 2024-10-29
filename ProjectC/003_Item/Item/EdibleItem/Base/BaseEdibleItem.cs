using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConditionInfo;

public class BaseEdibleItem : BaseItemData
{
    // 制作者　田内
    // 食料アイテム

    //============================
    // 回復量

    [Header("回復値")]
    [SerializeField]
    [Range(0, 99)]
    protected int m_healingValue = new();


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// この料理の回復量を返す、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 料理の回復量(int)
    /// </returns>
    /// --------------------------------------
    #endregion
    public int HealingValue { get { return m_healingValue; } }

    //==============================
    // この材料を食べることが可能か

    public bool IsEat
    {
        get
        {
            bool flg = true;

            if (m_conditionID != ConditionID.Normal)
            {
                flg = false;
            }

            return flg;
        }
    }

    //============================
    // 効果


    [Header("効果")]
    [SerializeField]
    protected ConditionID m_conditionID = ConditionID.Normal;


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// この料理の効果を返す、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 料理の効果(ConditionID)
    /// </returns>
    /// --------------------------------------
    #endregion
    public ConditionID ConditionID { get { return m_conditionID; } }


}
