using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FoodInfo;

public class ManagementProvideFoodControllerSlotData : FoodSlotData
{
    // 制作者　田内
    // 提供料理スロット

    // コントローラー
    private SelectManagementProvideFoodController m_selectManagementProvideFoodController = null;


    //====================================================
    //               実行処理
    //====================================================

    public void Start()
    {
        m_pocketType = ManagementProvideFoodManager.instance.PocketType;
    }

    private void Update()
    {
        Check();
    }

    public void SetSelectManagementProvideFoodController(SelectManagementProvideFoodController _controller)
    {
        m_selectManagementProvideFoodController = _controller;
    }

    // 存在するか確認
    private void Check()
    {
        #region nullチェック
        if (m_selectManagementProvideFoodController == null)
        {
            Debug.LogError("SelectManagementProvideFoodControllerがセットされていません");
            return;
        }
        #endregion

        // 提供予定料理リストにこの料理がなければ
        if (m_selectManagementProvideFoodController.IsAddedProvideFood((FoodID)m_itemID) == false)
        {
            Destroy(gameObject);
        }
    }
}
