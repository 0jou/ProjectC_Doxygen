using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ConditionDataBase : ScriptableObject
{

    // 制作者 田内

    [Header("状態の種類")]
    [SerializeField]
    private List<ConditionData> m_conditionDataBaseList = new List<ConditionData>();


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// 状態のデータベースリストを返す、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 状態のデータベースリスト
    /// </returns>
    /// --------------------------------------
    #endregion
    public List<ConditionData> ConditionDataBaseList { get { return m_conditionDataBaseList; } }


}
