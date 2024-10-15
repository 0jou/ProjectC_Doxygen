using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PocketItemSlotData : ItemSlotData
{
    // 制作者 田内
    // ポケット情報のアイテムスロットデータ

   
    [Header("ポケット所持数")]
    [SerializeField]
    private TextMeshProUGUI m_pocketNumText = null;

    // ポケットアイテム情報
    private PocketItemData m_pocketItemData = null;

    public PocketItemData PocketItemData
    {
        get { return m_pocketItemData; }
    }

    //=========================================
    //              実行処理
    //=========================================

    public void SetPocketItemData(PocketItemData _pocketItemData)
    {
        m_pocketItemData = _pocketItemData;

        SetPocketNumText();
    }

    public override void InitializeSlotData()
    {
        base.InitializeSlotData();

        SetPocketNumText(false);
    }


    // 対応するポケットアイテム情報の所持数をセット
    private void SetPocketNumText(bool _active = true)
    {
        if (m_pocketNumText == null) return;

        m_pocketNumText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_pocketItemData == null) return;

        m_pocketNumText.gameObject.SetActive(true);
        m_pocketNumText.text = m_pocketItemData.Num.ToString();

    }
}
