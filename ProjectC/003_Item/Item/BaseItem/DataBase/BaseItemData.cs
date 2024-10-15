using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;


[System.Serializable]
public class BaseItemData : ScriptableObject
{


    // 制作者(田内)

    public virtual void SetData()
    {
        // m_itemTypeID = ItemTypeID.?;
        // m_itemID=?;
        Debug.LogError("overrideしていません");
    }



    //====================================================
    // アイテムの種類ID

    protected ItemTypeID m_itemTypeID = ItemTypeID.Ingredient;

    public ItemTypeID ItemTypeID { get { return m_itemTypeID; } }


    //====================================================
    // アイテムのID


    // ID番号
    protected uint m_itemID = 0;


    public uint ItemID { get { return m_itemID; } }


    //============================
    // アイテムの名前


    [Header("アイテムの名前")]
    [SerializeField]
    protected string m_itemName = "None";


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// この料理の名前確認する、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 料理の名前
    /// </returns>
    /// --------------------------------------
    #endregion
    public string ItemName { get { return m_itemName; } }

    //============================
    // 所持可能数

    [Header("最大保持数")]
    [SerializeField]
    [Min(1)]
    private uint m_maxNum = 10;

    public uint MaxNum
    {
        get { return m_maxNum; }
    }

    //============================
    // アイテムの説明文


    [Header("アイテムの説明文")]
    [SerializeField]
    [Multiline(3)]
    protected string m_itemDescriptionText = "None";


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// この料理の紹介文を確認する、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 料理の紹介文
    /// </returns>
    /// --------------------------------------
    #endregion
    public string ItemDescriptionText { get { return m_itemDescriptionText; } }


    //============================
    // レベル


    [Header("アイテムのレベル")]
    [SerializeField]
    [Range(1, 3)]
    protected uint m_itemLevel = new();


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// この料理のレベルを返す、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 料理のレベル(int)
    /// </returns>
    /// --------------------------------------
    #endregion
    public uint ItemLevel { get { return m_itemLevel; } }


    //============================
    // アイテムの画像


    [Header("アイテムの画像")]
    [SerializeField]
    protected Sprite m_itemSprite = null;


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// この料理の見た目確認する、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 料理の見た目
    /// </returns>
    /// --------------------------------------
    #endregion
    public Sprite ItemSprite
    {
        get
        {

            if (m_itemSprite == null)
            {
                Debug.LogError("画像が登録されていません");
            }

            return m_itemSprite;

        }
    }


    //============================
    // アイテムのプレハブ


    [Header("アイテムのプレハブ(※インスタンスを作成する際に使用します)")]
    [SerializeField]
    protected GameObject m_itemPrefab = null;


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// この料理の見た目確認する、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 料理の見た目
    /// </returns>
    /// --------------------------------------
    #endregion
    public GameObject ItemPrefab
    {
        get
        {

            if (m_itemPrefab == null)
            {
                Debug.LogError(m_itemTypeID + ":" + m_itemID + "のアクション用プレハブが登録されていません");
            }

            return m_itemPrefab;

        }
    }



}
