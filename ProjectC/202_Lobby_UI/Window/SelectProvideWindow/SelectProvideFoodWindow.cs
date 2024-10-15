using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public class SelectProvideFoodWindow : BaseWindow
{
    // 制作者　田内
    // 提供する料理を選択する


    [Header("WindowUIコントローラー")]
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
    [Header("ボタン")]
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

        if(m_selectFoodDetail==null)
        {
            Debug.LogError("SelectFoodDetailUIがシリアライズされていません");
            return;
        }

        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
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


                // キャンセル
                if (PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.UI, "Cancel"))
                {
                    SoundManager.Instance.StartPlayback("Cancel");
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
        if (m_inputActionButton==null)
        {
            Debug.LogError("InputAActionButtonがシリアライズされていません");
            return;
        }
        if(m_sceneTransitionManager==null)
        {
            Debug.LogError("SceneTransitionManagerがシリアライズされていません");
            return;
        }
        #endregion

        if(m_inputActionButton.IsInputActionTrriger())
        {
            _ = m_sceneTransitionManager.SceneChange();
        }

    }

}
