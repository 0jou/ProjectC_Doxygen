using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateProvideFoodRecipeSlotList :CreateRecipeSlotList
{
    // 制作者 田内

    void Start()
    {
       m_pocketType = ManagementProvideFoodManager.instance.PocketType;
    }


}
