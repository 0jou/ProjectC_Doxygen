using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementUI : MonoBehaviour
{
    // 制作者 田内
    // 経営UIの処理をまとめたクラス


    [Header("スロット作成")]
    [SerializeField]
    private CreateProvideFoodSlotList m_createProvideFoodSlotList = null;

    //===========================================================
    //                       実行処理
    //===========================================================

    void Start()
    {
        #region nullチェック
        if (m_createProvideFoodSlotList == null)
        {
            Debug.LogError("m_createManagementProvideFoodSlotListがシリアライズされていません");
            return;
        }
        #endregion

        m_createProvideFoodSlotList.CreateSlot();

    }


    void Update()
    {

    }

}
