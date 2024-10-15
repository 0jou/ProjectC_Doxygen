using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using ManagementStateUpdateInfo;

public class ManagementStateUpdate_Update : BaseManagementStateUpdate
{

    // 制作者 田内
    // 経営終了処理



    //====================================================
    //                   実行処理
    //====================================================


    public override async UniTask OnInitialize()
    {
        // 開始
        Time.timeScale = 1.0f;

        await UniTask.CompletedTask;
    }

    public override async UniTask OnUpdate()
    {
        // 経過時間を超えたら
        if (ManagementGameManager.instance.IsTimeOver())
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
