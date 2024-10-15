using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;


[CreateAssetMenu]
[System.Serializable]
public class ItemTypeData : ScriptableObject
{

    //============================
    // アイテム種類ID

    [Header("アイテム種類ID")]
    [SerializeField]
    private ItemTypeID m_itemTypeID = new();

    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// このアイテム種類のIDを確認する、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// アイテム種類のID
    /// </returns>
    /// --------------------------------------
    #endregion
    public ItemTypeID ItemTypeID { get { return m_itemTypeID; } }


    //============================
    // この料理の名前


    [Header("この種類の名前")]
    [SerializeField]
    private string m_itemTypeName = "None";


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// この種類の名前確認する、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 種類の名前
    /// </returns>
    /// --------------------------------------
    #endregion
    public string ItemTypeName { get { return m_itemTypeName; } }


    //============================
    // この料理の紹介文


    [Header("この種類の紹介文")]
    [SerializeField]
    private string m_itemTypeDescriptionText = "None";


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// このアイテム種類の紹介文を確認する、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// アイテム種類の紹介文
    /// </returns>
    /// --------------------------------------
    #endregion
    public string ItemTypeDescriptionText { get { return m_itemTypeDescriptionText; } }


    //============================
    // このアイテム種類の見た目


    [Header("この状態の見た目")]
    [SerializeField]
    private Sprite m_itemTypeSprite = null;


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// このアイテム種類の見た目確認する、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 料理の見た目
    /// </returns>
    /// --------------------------------------
    #endregion
    public Sprite ItemTypeSprite { get { return m_itemTypeSprite; } }

}
