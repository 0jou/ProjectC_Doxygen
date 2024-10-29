using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using IngredientInfo;
using FoodInfo;
using ItemIDEditor;


public class AssignItemID : MonoBehaviour
{
    // 制作者 (田内)

    //===============================================================


    [Header("このアイテムの種類ID")]
    [SerializeField]
    private ItemTypeID m_itemTypeID= ItemTypeID.Food;


    public ItemTypeID ItemTypeID { get { return m_itemTypeID; } }


    //=================================================================


    private uint m_itemID = 0;

    public uint ItemID { get { return m_itemID; } }


    //===================================================================
    

    [ItemIDConditionalDisableInInspector("m_itemTypeID", ItemTypeID.Ingredient)]
    [SerializeField]
    private IngredientID m_ingredientID;


    //===================================================================


    [ItemIDConditionalDisableInInspector("m_itemTypeID", ItemTypeID.Food)]
    [SerializeField]
    private FoodID m_foodID;


    //======================================================================


    private void Awake()
    {

        switch (ItemTypeID)
        {

            case ItemTypeID.Ingredient:
                {
                    m_itemID = (uint)m_ingredientID;
                }
                break;

            case ItemTypeID.Food:
                {
                    m_itemID = (uint)m_foodID;
                }
                break;

            default:
                {
                    Debug.LogError("アイテムのTypeIDが存在しない値です");
                }
                break;
        }
    }

    private void Start()
    {
       IMetaAI<AssignItemID>.Instance.RegisterObject(this);
    }



    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// 引数IDをセットする
    /// </summary>
    /// --------------------------------------
    /// <param name="_itemTypeID">
    /// アイテムの種類
    /// </param> 
    /// /// --------------------------------------
    /// <param name="_itemID">
    /// アイテムの種類
    /// </param>
    /// --------------------------------------
    #endregion
    // アイテムのIDをセット
    public void SetItemID(ItemTypeID _itemTypeID,uint _itemID)
    {

        m_itemTypeID = _itemTypeID;

        m_itemID = _itemID;

    }


}


