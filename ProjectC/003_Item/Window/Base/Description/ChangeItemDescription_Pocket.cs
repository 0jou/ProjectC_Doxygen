using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


using NaughtyAttributes;

public partial class ChangeItemDescription : MonoBehaviour
{
    // 制作者 田内
    // ポケットアイテム

    [Foldout("ポケット")]
    [Header("-------------------------------------------------------")]
    [Header("ポケット所持数Text")]
    [SerializeField]
    protected TextMeshProUGUI m_pocketNumText = null;


    [Foldout("ポケット")]
    [Header("表示/非表示用")]
    [SerializeField]
    protected List<GameObject> m_pocketNumList = new();


    //==========================================================
    //                      実行処理
    //==========================================================

    private void SetPocketDescription(BaseItemData _itemData)
    {
        SetPocketNumText();
    }

    private void InitilizePocketDescription()
    {
        SetPocketNumText(false);
    }

    private void SetPocketActiveList()
    {
        CheckToSetActiveGameObjectList(m_pocketNumText, m_pocketNumList);
    }



    // ポケットの所持数表示
    private void SetPocketNumText(bool _active = true)
    {
        if (m_pocketNumText == null) return;

        m_pocketNumText.gameObject.SetActive(false);
        if (_active == false) return;

        var pocketItemData = GetPocketItemData();
        if (pocketItemData == null) return;

        m_pocketNumText.text = pocketItemData.Num.ToString();

        m_pocketNumText.gameObject.SetActive(true);

    }


    private PocketItemData GetPocketItemData()
    {
        var slotData = GetCurrentSelectItemSlotData();
        if (slotData == null || slotData is PocketItemSlotData == false) return null;
        var pocketItemSlotData = slotData as PocketItemSlotData;
        return pocketItemSlotData.PocketItemData;

    }
}
