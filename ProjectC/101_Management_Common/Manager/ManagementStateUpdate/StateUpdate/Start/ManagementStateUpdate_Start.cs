using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

using UnityEngine.UI;


public class ManagementStateUpdate_Start : BaseManagementStateUpdate
{

    // 制作者 田内
    // 経営開始処理

    [Header("ターゲットのImages")]
    [SerializeField]
    private Image m_targetImage = null;


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
        // ターゲットのImageが無くなれば
        if (m_targetImage == null)
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
