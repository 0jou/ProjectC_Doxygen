using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

using NaughtyAttributes;

public class ManagementResultWindow : BaseWindow
{
    // 制作者 田内
    // 経営のリザルトウィンドウ

    [Header("説明文")]
    [SerializeField]
    private ChangeResultDescription m_changeProvideFoodDescription = null;


    [BoxGroup("シーン")]
    [Header("シーン変更")]
    [SerializeField]
    private SceneTransitionManager m_sceneTransitionManager = null;


    [BoxGroup("シーン")]
    [Header("ボタン")]
    [SerializeField]
    private InputActionButton m_inputActionButton = null;


    //==============================================
    //              実行処理
    //==============================================

    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_changeProvideFoodDescription == null)
        {
            Debug.LogError("ChangeProvideFoodDescriptionがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // 説明文を初期化
            m_changeProvideFoodDescription.OnInitialize();

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
            while (cancelToken.IsCancellationRequested == false)
            {

                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // キャンセル
                if (Transition()) return;

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    // シーン遷移を行う
    // 遷移できればtrueを返す
    private bool Transition()
    {
        #region nullチェック
        if (m_sceneTransitionManager == null)
        {
            Debug.LogError("シーン変更クラスがシリアライズされていません");
            return false;
        }
        if (m_inputActionButton == null)
        {
            Debug.LogError("InputActionButtonがシリアライズされていません");
            return false;
        }
        #endregion

        // 選択したボタンがスタートなら
        if (m_inputActionButton.IsInputActionTrriger())
        {
            // Todo:ここでシーン遷移
            _ = m_sceneTransitionManager.SceneChange();

            return true;
        }

        return false;
    }

}
