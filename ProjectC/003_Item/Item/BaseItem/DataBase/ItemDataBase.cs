using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemDataBase : ScriptableObject
{

    // 制作者(田内)


    [Header("アイテムのデータリスト")]
    [SerializeField]
    private List<BaseItemData> m_itemDataBaseList = new List<BaseItemData>();


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// 料理のデータベースリストを返す、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 料理のデータベースリスト
    /// </returns>
    /// --------------------------------------
    #endregion
    public List<BaseItemData> ItemDataBaseList { get { return m_itemDataBaseList; } }


}
