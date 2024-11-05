using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementStandbyWindow : BaseWindow
{
    // 制作者 田内
    // 経営シーンの待機画面


    [Header("チュートリアルウィンドウコントローラー")]
    [SerializeField]
    private WindowController m_tutorialWindowController = null;

    //=====================================
    //          実行処理
    //=====================================

    public override async UniTask OnInitialize()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            await UniTask.CompletedTask;
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    public override async UniTask OnUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            // チュートリアルを表示
            await CreateTutorialWindow();
            cancelToken.ThrowIfCancellationRequested();

            // ボタンを押すまで
            while (cancelToken.IsCancellationRequested == false)
            {
                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // ボタンが押されたら
                if (IsClose()) return;

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    // チュートリアルウィンドウを表示
    private async UniTask CreateTutorialWindow()
    {
        #region nullチェック
        if (m_tutorialWindowController == null)
        {
            Debug.LogError("TutorialWindowControllerがシリアライズされていません");
            return;
        }
        #endregion

        var controller = Instantiate(m_tutorialWindowController);
        await controller.CreateWindow<BaseWindow>();
        if (controller != null) Destroy(controller.gameObject);

        await UniTask.CompletedTask;
    }
}
