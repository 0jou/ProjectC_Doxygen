using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LobbyStateInfo;
using Cysharp.Threading.Tasks;

public class LobbyStateUpdate_TrialSession : BaseLobbyStateUpdate
{
    // 体験会用の動作
    // 制作者　田内

    [Header("体験会ウィンドウコントローラー")]
    [SerializeField]
    private GameObject m_trialSessionWindowController = null;


    // 作成したウィンドウ
    private GameObject m_createTrialSessionWindowController = null;


    //====================================================
    //                   実行処理
    //====================================================

    public override async UniTask OnInitialize()
    {
        CreateTrialSessionWindow().Forget();

        await UniTask.CompletedTask;
    }


    override public async UniTask OnExit()
    {
        // ウィンドウを削除
        DestoryTrialSessionWindow();

        await UniTask.CompletedTask;
    }


    // 経営ウィンドウを作成する
    private async UniTask CreateTrialSessionWindow()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            if (m_trialSessionWindowController == null)
            {
                Debug.LogError("体験会用sウィンドウコントローラーがシリアライズされていません");
                return;
            }

            // ウィンドウ作成
            m_createTrialSessionWindowController = Instantiate(m_trialSessionWindowController);

            // 処理を行う
            UpdateWindow createWindow = new();
            await createWindow.Update<BaseWindow>(m_createTrialSessionWindowController);
            cancelToken.ThrowIfCancellationRequested();

            // ウィンドウを削除
            DestoryTrialSessionWindow();

            // 終了
            SetEnd();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    // ウィンドウを削除する
    void DestoryTrialSessionWindow()
    {
        // 削除
        if (m_createTrialSessionWindowController != null)
        {
            Destroy(m_createTrialSessionWindowController);
        }
    }

}
