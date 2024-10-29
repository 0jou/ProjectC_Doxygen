using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using ButtonInfo;

public class SelectStageWindow : BaseWindow
{
    // ステージ選択ウィンドウ
    // 制作者　田内

    [Header("ステージ選択コントローラー")]
    [SerializeField]
    private SelectStageController m_selectStageController = null;

    [Header("ステージ説明")]
    [SerializeField]
    private ChangeSelectStageDescription m_changeSelectStageDescription = null;

    [Header("シーン変更")]
    [SerializeField]
    private SceneTransitionManager m_sceneTransitionManager = null;

    //======================================================
    //                  実行処理
    //======================================================


    public override async UniTask OnInitialize()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            if (m_changeSelectStageDescription == null)
            {
                Debug.LogError("説明文がシリアライズされていません");
                return;
            }

            // 説明文の初期化
            m_changeSelectStageDescription.OnInitialize();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

    public override async UniTask OnUpdate()
    {
        #region nullチェック
        if (m_selectStageController == null)
        {
            Debug.LogError("SelectStageControllerがシリアライズされていません");
            return;
        }

        if (m_changeSelectStageDescription == null)
        {
            Debug.LogError("ChangeSelectStageDescriptionがシリアライズされていません");
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



                // ステージ選択の更新
                m_selectStageController.OnUpdate();

                // 説明文の更新
                m_changeSelectStageDescription.OnUpdate();

                // シーン遷移
                if (Transion()) return;


                // ステージ選択の更新
                m_selectStageController.OnLateUpdate();


                // ウィンドウを閉じる
                if (IsClose())
                {
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
    private bool Transion()
    {
        #region nullチェック
        if (m_sceneTransitionManager == null)
        {
            Debug.LogError("シーン変更クラスがシリアライズされていません");
            return true;
        }
        if (m_selectStageController == null)
        {
            Debug.LogError("SelectStageControllerがシリアライズされていません");
            return true;
        }
        #endregion

        if(m_selectStageController.IsStart())
        {
            var data = m_selectStageController.GetCurrentSelectStageData();
            m_sceneTransitionManager.m_sceneName = data.SceneName;
            _ = m_sceneTransitionManager.SceneChange();
            return true;
        }

        return false;
    }



}
