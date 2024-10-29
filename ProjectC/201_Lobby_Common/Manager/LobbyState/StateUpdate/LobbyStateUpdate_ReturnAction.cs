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
    private WindowController m_returnActionWindowController = null;

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
        DestoryWindowController();

        // TODO : 移し替えは無しにする
        // 移し替える
        Transfer();

        // 新規獲得アイテムリストを初期化
        NewItemManager.instance.ItemDataList.Clear();

        await UniTask.CompletedTask;
    }


    private async UniTask CreateWindow()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            if (m_returnActionWindowController == null)
            {
                Debug.LogError("ウィンドウがシリアライズされていません");
                return;
            }

            m_createWindowController = Instantiate(m_returnActionWindowController);
            await m_createWindowController.CreateWindow<BaseWindow>();
            cancelToken.ThrowIfCancellationRequested();

            // 削除
            DestoryWindowController();

            // 終了
            SetEnd(m_nextLobbyState);
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

    // ウィンドウを削除する
    void DestoryWindowController()
    {
        // 削除
        if (m_createWindowController != null)
        {
            Destroy(m_createWindowController.gameObject);
        }
    }

    // TODO : 移し替えは無しにする
    // インベントリのアイテムを移し替える
    private void Transfer()
    {
        foreach(var data in InventoryManager.instance.ItemDataList)
        {
            for(int i=0;i<data.Num;++i)
            {
                ManagementStorageManager.instance.AddItem(data.ItemTypeID, data.ItemID);
            }
        }

        // インベントリを初期化
        InventoryManager.instance.ItemDataList.Clear();

    }
}
