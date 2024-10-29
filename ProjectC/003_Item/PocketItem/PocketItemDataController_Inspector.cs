using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;
using IngredientInfo;
using FoodInfo;

public partial class BasePocketItemDataController: MonoBehaviour
{

    [System.Serializable]
    public class DefaultPocketItemData
    {
        // 制作者 (田内)

        //===============================================================


        [Header("所持数")]
        [SerializeField]
        [Range(0, 99)]
        private int m_num = 1;

        public int Num { get { return m_num; } }


        [Header("このアイテムの種類ID")]
        [SerializeField]
        private ItemTypeID m_itemTypeID = new();

        public ItemTypeID ItemTypeID { get { return m_itemTypeID; } }

        //===================================================================

        [SerializeField]
        private IngredientID m_ingredientID = IngredientID.BigMushroom;


        //===================================================================

        [SerializeField]
        private FoodID m_foodID = FoodID.Omelette;


        //======================================================================


        public uint GetItemID()
        {
            uint id = 0;

            switch (m_itemTypeID)
            {

                case ItemTypeID.Ingredient:
                    {
                        id = (uint)m_ingredientID;
                    }
                    break;

                case ItemTypeID.Food:
                    {
                        id = (uint)m_foodID;
                    }
                    break;

                default:
                    {
                        Debug.LogError("アイテムのTypeIDが存在しない値です");
                    }
                    break;
            }

            return id;
        }

    }


    [Header("初期から保持しておくアイテム")]
    [SerializeField]
    private List<DefaultPocketItemData> m_defaultInventoryItemDataList = new();


    // 初期アイテムをポケットに追加
    private void AddDefaultItemData()
    {
        foreach (var item in m_defaultInventoryItemDataList)
        {
            if (item == null) continue;

            uint id = item.GetItemID();

            for (int i = 0; i < item.Num; ++i)
            {
                // アイテムを追加
                AddItem(item.ItemTypeID, id);
            }

        }

    }

}
