using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using FoodInfo;
using PocketItemDataInfo;

public class RecipeItemSlotData : ItemSlotData
{
    // 料理が作成可能か確認などを行う
    // 作成者　田内

    //===========================================================

    [Header("作成可能である場合の色情報")]
    [SerializeField]
    private Color m_validSlotColor = Color.white;


    [Header("作成可能でない場合の色情報")]
    [SerializeField]
    private Color m_invalidSlotColor = Color.gray;

   
    //=======================
    // 作成可能かどうか

    private bool m_isCreate = false;

    public bool IsCreate { get { return m_isCreate; } }

    //===========================================================
    //                       実行処理
    //===========================================================


    // スロットのデータをセットする
    public override void SetItemSlotData(BaseItemData _itemData, PocketType _pocketType)
    {
        base.SetItemSlotData(_itemData, _pocketType);

        // 作成可能か確認
        CheckCreate();
    }




    // 作成可能か確認
    public void CheckCreate()
    {
        // 作成可能かを確認
        m_isCreate = FoodData.IsCreate(m_pocketType, (FoodID)m_itemID);

        // 色をセット
        SetSlotColor();

    }

    private void SetSlotColor()
    {
        var images = gameObject.GetComponentsInChildren<Image>();


        foreach (var image in images)
        {
            if (m_isCreate)
            {
                image.color = m_validSlotColor;
            }
            else
            {
                image.color = m_invalidSlotColor;
            }
        }

    }

}
