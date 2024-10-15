using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using ManagementStateUpdateInfo;
using UnityEngine.UI;

public class ManagementStateUpdate_End : BaseManagementStateUpdate
{

    // 制作者 田内
    // 経営終了処理

    [Header("終了Image")]
    [SerializeField]
    private Image m_endImage = null;

    //====================================================
    //                   実行処理
    //====================================================

    
    public override async UniTask OnInitialize()
    {
        // 停止
        Time.timeScale = 0.0f;

        await UniTask.CompletedTask;
    }


    public override async UniTask OnUpdate()
    {
        // スタートの画像がnullになれば
        if (m_endImage == null)
        {
            SetEnd();
        }

        await UniTask.CompletedTask;
    }

    override public async UniTask OnExit()
    {
        await UniTask.CompletedTask;
    }


}
