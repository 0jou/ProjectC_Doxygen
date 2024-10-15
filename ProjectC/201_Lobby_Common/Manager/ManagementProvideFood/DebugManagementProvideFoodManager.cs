using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodInfo;
using PocketItemDataInfo;
using ItemInfo;

public class DebugManagementProvideFoodManager : MonoBehaviour
{
    // 制作者 田内
    // デバッグ用

    [Header("デバッグ用")]
    [SerializeField]
    private List<FoodID> m_foodIDList = new();

    [Header("何個作成可能にするか")]
    [SerializeField]
    [Range(1, 100)]
    private int m_num = 1;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var id in m_foodIDList)
        {
            var data = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)id);


            ManagementProvideFoodManager.instance.AddProvideFoodList(id);

            var manager = ManagementProvideFoodManager.instance.PocketType.GetPocketItemDataManager();

            for (int n = 0; n < m_num; ++n)
            {
                foreach (var need in data.NeedIngredientObjectList)
                {
                    if (need == null) continue;
                    for (int i = 0; i < need.Num; ++i)
                    {
                        manager.AddItem(ItemTypeID.Ingredient, (uint)need.IngredientID);
                    }
                }
            }
        }
    }
}
