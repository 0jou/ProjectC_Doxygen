using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProvideFoodCreateNeedIngredientSlotList : CreateNeedIngredientSlot
{
    // 制作者 田内

    void Start()
    {
        m_pocketType = ManagementProvideFoodManager.instance.PocketType;
    }
}
