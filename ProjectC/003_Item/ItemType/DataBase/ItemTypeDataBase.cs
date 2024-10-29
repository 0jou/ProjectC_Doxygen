using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ItemTypeDataBase : ScriptableObject
{
    // 制作者(田内)


    [Header("アイテムタイプの種類")]
    [SerializeField]
    private List<ItemTypeData> m_itemTypeDataBaseList = new List<ItemTypeData>();


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// アイテムタイプのデータベースリストを返す、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// アイテムタイプのデータベースリスト
    /// </returns>
    /// --------------------------------------
    #endregion
    public List<ItemTypeData> ItemTypeDataBaseList { get { return m_itemTypeDataBaseList; } }

}
