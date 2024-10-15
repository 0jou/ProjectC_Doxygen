using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FoodInfo;

public class CreateFoodInputActionButton : InputActionButton
{
    // 制作者 田内
    // 料理作成が可能かどうか

    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;


    // 押せるかどうか
    bool m_isPress = false;


    //===========================================
    //              実行処理
    //===========================================

    protected override bool IsPress()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIConflictがシリアライズされていません");
            return false;
        }
        #endregion

        // 選択したか
        if (m_selectUIController.IsSelectChangeFlg)
        {
            // 選択しているUIがなければ
            var currentUI = m_selectUIController.CurrentSelectUI;
            if (currentUI == null || currentUI.TryGetComponent<ItemSlotData>(out var slotData) == false)
            {
                m_isPress = false;
            }
            else
            {
                // 作成可能か確認
                if (FoodData.IsCreate(slotData.PocketType, (FoodID)slotData.ItemID))
                {
                    m_isPress = true;
                }
                else m_isPress = false;
            }
        }

        return m_isPress;
    }
}
