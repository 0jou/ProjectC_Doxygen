using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

public class ManagementStateUpdate_Result : BaseManagementStateUpdate
{

    // 制作者 田内
    // 経営終了処理

    [Header("新規獲得アイテム確認ウィンドウコントローラー")]
    [SerializeField]
    private WindowController m_windowController = null;


    // 作成したウィンドウコントローラー
    private WindowController m_createWindowController = null;


    //====================================================
    //                   実行処理
    //====================================================


    public override async UniTask OnInitialize()
    {
        // 経営データを更新
        ManagementGameManager.instance.SetManagementData();

        CreateToUpdateWindow().Forget();

        await UniTask.CompletedTask;
    }


    override public async UniTask OnExit()
    {
        // 初期化
        ManagementProvideFoodManager.instance.Initialize();

        DestoryWindowController();

        await UniTask.CompletedTask;
    }


    // ウィンドウを作成する
    private async UniTask CreateToUpdateWindow()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            if (m_windowController == null)
            {
                Debug.LogError("ウィンドウがシリアライズされていません");
                return;
            }

            m_createWindowController = Instantiate(m_windowController);

            // 処理を行う
            await m_createWindowController.CreateWindow<BaseWindow>();
            cancelToken.ThrowIfCancellationRequested();

            // 削除
            DestoryWindowController();

            // 終了
            SetEnd();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

    // ウィンドウを削除する
    private void DestoryWindowController()
    {
        // 削除
        if (m_createWindowController != null)
        {
            Destroy(m_createWindowController.gameObject);
        }
    }


}
