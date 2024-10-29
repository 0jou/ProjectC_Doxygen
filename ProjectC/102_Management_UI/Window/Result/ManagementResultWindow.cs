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

    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("スクロール座標")]
    [SerializeField]
    private ChangeScrollViewPosition m_changeScrollViewPosition = null;

    [BoxGroup("シーン")]
    [Header("シーン変更")]
    [SerializeField]
    private SceneTransitionManager m_sceneTransitionManager = null;



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
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }
        if(m_changeScrollViewPosition==null)
        {
            Debug.LogError("ChangeScrollViewPositionがシリアライズされていません");
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

                // 選択UIコントローラーに更新
                m_selectUIController.OnUpdate();

                m_changeScrollViewPosition.OnUpdate();

                m_selectUIController.OnLateUpdate();

                // キャンセル
                if (IsClose())
                {
                    Transition();
                    return;
                }

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
    private void Transition()
    {
        #region nullチェック
        if (m_sceneTransitionManager == null)
        {
            Debug.LogError("シーン変更クラスがシリアライズされていません");
            return;
        }
        #endregion
        _ = m_sceneTransitionManager.SceneChange();
    }

}
