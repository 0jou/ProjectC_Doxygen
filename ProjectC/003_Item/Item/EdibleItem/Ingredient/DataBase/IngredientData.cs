using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConditionInfo;
using IngredientInfo;
using SelectUseItemInfo;
using ItemInfo;

[CreateAssetMenu]
[System.Serializable]
public class IngredientData : BaseEdibleItem
{

    // 制作者(田内)

    public override void SetData()
    {
        m_itemTypeID = ItemTypeID.Ingredient;
        m_itemID = (uint)m_ingredientID;
    }


    //============================
    // この材料のID


    [Header("材料のID")]
    [SerializeField]
    private IngredientID m_ingredientID = IngredientID.NormalMushroom;



}
