using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class StageDataBase : ScriptableObject
{

    // 制作者 田内

    [Header("ステージの種類")]
    [SerializeField]
    private List<StageData> m_stageDataBaseList = new();


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// ステージのデータベースリストを返す、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// ステージのデータベースリスト
    /// </returns>
    /// --------------------------------------
    #endregion
    public List<StageData> StageDataBaseList { get { return m_stageDataBaseList; } }

}
