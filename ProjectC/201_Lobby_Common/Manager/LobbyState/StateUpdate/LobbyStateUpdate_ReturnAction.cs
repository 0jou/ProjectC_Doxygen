using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

public class LobbyStateUpdate_ReturnAction : BaseLobbyStateUpdate
{
    // アクションから帰ってきた動作
    // 制作者　田内

    [Header("新規獲得アイテム確認ウィンドウコントローラー")]
    [SerializeField]
    private GameObject m_returnActionWindowController = null;

  
    // 作成したウィンドウ
    private GameObject m_createReturnActionWindow = null;


    //====================================================
    //                   実行処理
    //====================================================



    public override async UniTask OnInitialize()
    {

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // ウィンドウを作成/処理
            await CreateReturnActionWindow();
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
        DestoryReturnActionWindow();

        // リストを初期化
        NewItemManager.instance.ItemDataList.Clear();

        await UniTask.CompletedTask;
    }


    private async UniTask CreateReturnActionWindow()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            if (m_returnActionWindowController == null)
            {
                Debug.LogError("ウィンドウがシリアライズされていません");
                return;
            }

            m_createReturnActionWindow = Instantiate(m_returnActionWindowController);

            // 処理を行う
            UpdateWindow createWindow = new();
            await createWindow.Update<BaseWindow>(m_createReturnActionWindow);
            cancelToken.ThrowIfCancellationRequested();

            // 削除
            DestoryReturnActionWindow();

            // 終了
            SetEnd();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

    // ウィンドウを削除する
    void DestoryReturnActionWindow()
    {
        // 削除
        if (m_createReturnActionWindow != null)
        {
            Destroy(m_createReturnActionWindow);
        }
    }
}
