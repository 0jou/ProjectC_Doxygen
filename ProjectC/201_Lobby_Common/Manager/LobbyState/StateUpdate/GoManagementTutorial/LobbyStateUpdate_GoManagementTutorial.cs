using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class LobbyStateUpdate_GoManagementTutorial : BaseLobbyStateUpdate
{
    // 制作者 田内
    // 経営に行くチュートリアル


    [Header("ウィンドウコントローラー")]
    [SerializeField]
    private WindowController m_windowController = null;


    // 作成したウィンドウ
    private WindowController m_createWindowController = null;


    //====================================================
    //                   実行処理
    //====================================================



    public override async UniTask OnInitialize()
    {

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // ウィンドウを作成/処理
            await CreateWindow();
            cancelToken.ThrowIfCancellationRequested();

            await UniTask.CompletedTask;
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }



    override public async UniTask OnExit()
    {
        // ウィンドウを削除
        DestoryControllerWindow();

        await UniTask.CompletedTask;
    }


    private async UniTask CreateWindow()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            if (m_windowController == null)
            {
                Debug.LogError("WindowControllerがシリアライズされていません");
                return;
            }

            m_createWindowController = Instantiate(m_windowController);

            // 処理を行う
            await m_createWindowController.CreateWindow<BaseWindow>();
            cancelToken.ThrowIfCancellationRequested();

            // 削除
            DestoryControllerWindow();

            // 終了
            SetEnd();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

    // ウィンドウコントローラーを削除する
    void DestoryControllerWindow()
    {
        if (m_createWindowController != null)
        {
            // 削除
            Destroy(m_createWindowController.gameObject);
        }
    }

}
