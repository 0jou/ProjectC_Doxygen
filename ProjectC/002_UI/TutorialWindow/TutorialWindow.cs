using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWindow : BaseWindow
{
    // 制作者 田内
    // チュートリアルウィンドウ


    [Header("チュートリアルコントローラー")]
    [SerializeField]
    private SelectTutorialController m_selectTutorialController = null;

    [Header("チュートリアル説明文")]
    [SerializeField]
    private ChangeSelectTutorialControllerDescription m_changeTutorialDescription = null;


    //=============================================
    //               実行処理
    //=============================================

    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_changeTutorialDescription == null)
        {
            Debug.LogError("ChangeTutorialDescriptionがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            m_changeTutorialDescription.OnInitialize();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    public override async UniTask OnUpdate()
    {
        #region nullチェック
        if (m_selectTutorialController == null)
        {
            Debug.LogError("SelectTutorialControllerがシリアライズされていません");
            return;
        }
        if (m_changeTutorialDescription == null)
        {
            Debug.LogError("ChangeTutorialDescriptionがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            while (cancelToken.IsCancellationRequested == false)
            {
                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // コントローラー処理
                m_selectTutorialController.OnUpdate();

                // 説明文を更新
                m_changeTutorialDescription.OnUpdate();

                // コントローラー後処理
                m_selectTutorialController.OnLateUpdate();

                // チュートリアルが終了すれば
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

}
