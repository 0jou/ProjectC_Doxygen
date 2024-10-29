using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementUI : MonoBehaviour
{
    // 制作者 田内
    // 経営UIの処理をまとめたクラス


    [Header("スロット作成")]
    [SerializeField]
    private CreateManagementProvideFoodSlotList m_createProvideFoodSlotList = null;

    [Header("イベントUI作成")]
    [SerializeField]
    private CreateManagementEventUI m_createManagementEventUI = null;


    //===========================================================
    //                       実行処理
    //===========================================================

    void Start()
    {
        OnStart();
    }


    void Update()
    {
        OnUpdate();
    }


    private void OnStart()
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

    private void OnUpdate()
    {
        #region nullチェック
        if (m_createManagementEventUI == null)
        {
            Debug.LogError("CreateManagementEventUIがシリアライズされていません");
            return;
        }
        #endregion

        m_createManagementEventUI.OnUpdate();

    }

}
