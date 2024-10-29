using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public class ManagementSelectProvideFoodWindow : BaseWindow
{
    // 制作者　田内
    // 提供する料理を選択する

    [Header("チュートリアルウィンドウコントローラー")]
    [SerializeField]
    private WindowController m_tutorialWindowController = null;

    [Header("経営提供料理コントローラー")]
    [SerializeField]
    private SelectManagementProvideFoodController m_selectManagementProvideFoodController = null;

    [Header("ウィンドウUIコントローラー")]
    [SerializeField]
    private WindowUIController m_windowUIController = null;

    [Header("詳細UI")]
    [SerializeField]
    private SelectFoodDetailUI m_selectFoodDetail = null;

    //=======================================================

    [BoxGroup("Scene")]
    [Header("シーン切り替え")]
    [SerializeField]
    private SceneTransitionManager m_sceneTransitionManager = null;

    [BoxGroup("Scene")]
    [Header("経営開始ボタン")]
    [SerializeField]
    private InputActionButton m_inputActionButton = null;

    //===================================================
    //                    実行処理
    //===================================================

    public override async UniTask OnInitialize()
    {
        #region nullチェック

        if (m_windowUIController == null)
        {
            Debug.LogError("WindowUIControllerがシリアライズされていません");
            return;
        }

        if (m_selectFoodDetail == null)
        {
            Debug.LogError("SelectFoodDetailUIがシリアライズされていません");
            return;
        }

        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // 初期化
            await m_windowUIController.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // 詳細UI処理
            await m_selectFoodDetail.OnInitialize();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }


    public override async UniTask OnUpdate()
    {
        #region nullチェック

        if (m_windowUIController == null)
        {
            Debug.LogError("WindowUIコントローラーがシリアライズされていません");
            return;
        }

        if (m_selectFoodDetail == null)
        {
            Debug.LogError("SelectFoodDetailUIがシリアライズされていません");
            return;
        }

        #endregion

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


                // ウィンドウUI処理
                await m_windowUIController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // 詳細UI処理
                await m_selectFoodDetail.OnUpdate();

                // ウィンドウUI後処理
                await m_windowUIController.OnLateUpdate();
                cancelToken.ThrowIfCancellationRequested();


                m_selectManagementProvideFoodController.OnLateUpdate();

                // ウィンドウを閉じる
                if (IsClose())
                {
                    return;
                }

                // シーン切り替え
                Transition();

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }


    private void Transition()
    {
        #region nullチェック
        if (m_selectManagementProvideFoodController == null)
        {
            Debug.LogError("SelectManagementProvideFoodControllerがシリアライズされていません");
            return;
        }
        if (m_inputActionButton == null)
        {
            Debug.LogError("InputAActionButtonがシリアライズされていません");
            return;
        }
        if (m_sceneTransitionManager == null)
        {
            Debug.LogError("SceneTransitionManagerがシリアライズされていません");
            return;
        }
        #endregion

        if (m_inputActionButton.IsInputActionTrriger())
        {
            // マネージャーにコントローラーのデータをセットする
            m_selectManagementProvideFoodController.SetManagementProvideFoodData();

            // シーン遷移
            _ = m_sceneTransitionManager.SceneChange();
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
        await CreateToUpdateWindow<BaseWindow>(controller, false);
        if (controller != null) Destroy(controller.gameObject);

        await UniTask.CompletedTask;
    }

}
