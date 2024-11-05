using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

using NaughtyAttributes;

public class BaseWindow : MonoBehaviour
{
    // 制作者(田内)

    //======================================

    [Header("キャンバスグループ")]
    [SerializeField]
    protected CanvasGroup m_canvasGroup = null;

    //======================================

    [Header("DOスピード")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    protected float m_doSpead = 0.5f;

    //======================================

    protected enum DepthOfFieldType
    {
        Enable,    // 有効
        Disable     // 無効
    }

    [Header("被写界深度")]
    [SerializeField]
    protected DepthOfFieldType m_depthOfFieldType = DepthOfFieldType.Disable;

    private bool m_alreadyDepthOfField = false;

    //======================================

    protected enum PauseGameType
    {
        Enable,    // 有効
        Disable     // 無効
    }

    [Header("一時停止")]
    [SerializeField]
    protected PauseGameType m_pauseGameType = PauseGameType.Enable;

    private float m_alreadyPauseGameTimeScale = 1.0f;

    //================================================================

    protected enum PauseGameMoveType
    {
        Enable,     // 有効
        Disable     // 無効
    }

    [Header("一時停止動作")]
    [SerializeField]
    protected PauseGameMoveType m_pauseGameMoveType = PauseGameMoveType.Enable;


    //================================================================

    protected enum HideUIType
    {
        Enable,    // 有効
        Disable     // 無効
    }

    [Header("他UIの非表示")]
    [SerializeField]
    protected HideUIType m_hideUIType = HideUIType.Disable;


    //======================================================


    [BoxGroup("SE")]
    [Header("ウィンドウ開きSE")]
    [SerializeField]
    private string m_openSEName = "WindowOpen";

    [BoxGroup("SE")]
    [Header("ウィンドウ閉じSE")]
    [SerializeField]
    private string m_closeSEName = "WindowClose";


    //======================================================

    [BoxGroup("Button")]
    [Header("InputActionButton操作コントローラー")]
    [SerializeField]
    protected InputActionButtonController m_inputActionButtonController = null;

    [BoxGroup("Button")]
    [Header("閉じるボタン")]
    [SerializeField]
    protected InputActionButton m_closeInputActionButton = null;

    //=======================================
    // 被写界深度用Volume

    protected Volume m_globalVolume = null;

    //=======================================
    // 隠したUICanvasリスト

    protected List<CanvasGroup> m_hideCanvasGroupList = new();

    //======================================
    // 開く・閉じるなどのInput

    protected PlayerInput m_input = null;

    public PlayerInput Input { get { return m_input; } set { m_input = value; } }

    //=================================================================================
    //                               実行処理
    //=================================================================================

    /// <summary>
    /// 初期設定(非表示の状態)
    /// </summary>
    public virtual async UniTask OnInitialize()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // シェーダーをセット
            SetGlobalVolume();

            // ゲームを停止
            PauseGame();

            // 隠すキャンバスリストをセット
            SetCanvasGroupList();

            // ボタンコントローラーの初期化
            if (m_inputActionButtonController != null)
            {
                m_inputActionButtonController.OnInitialize();
            }

            // 他UIを非表示
            await HideOtherUI();
            cancelToken.ThrowIfCancellationRequested();

            await UniTask.CompletedTask;

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    /// <summary>
    /// 表示
    /// </summary>
    public virtual async UniTask OnShow()
    {

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // ウィンドウ開き時のSE
            if (m_openSEName != "") SoundManager.Instance.StartPlayback(m_openSEName);

            // 被写界深度のセット
            ActiveTrueDepthOfField();

            // ウィンドウ表示
            await ShowDOAlpha();
            cancelToken.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    /// <summary>
    /// 処理
    /// </summary>
    public virtual async UniTask OnUpdate()
    {
        // ボタンコントローラーの更新
        if (m_inputActionButtonController != null)
        {
            m_inputActionButtonController.OnUpdate();
        }

        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 非表示
    /// </summary>
    public virtual async UniTask OnClose()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 被写界深度を終了
            ActiveFalseDepthOfField();

            // ゲームを再開
            ResumeGame();

            // フェードイン
            await CloseDOAlpha();
            cancelToken.ThrowIfCancellationRequested();

            // 他UIの表示
            await ShowOtherUI();
            cancelToken.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    /// <summary>
    /// 削除
    /// </summary>
    public virtual async UniTask OnDestroy()
    {
        Destroy(gameObject);

        await UniTask.CompletedTask;
    }


    //==================================================================================
    //                               便利機能
    //==================================================================================


    // ウィンドウを閉じるか確認する
    virtual protected bool IsClose()
    {
        #region nullチェック
        if (m_closeInputActionButton == null)
        {
            Debug.LogError("CloseInputActionButtonがシリアライズされていません");
            return false;
        }
        #endregion
        if (m_closeInputActionButton.IsInputActionTrriger())
        {
            // ウィンドウ開き時のSE
            if (m_closeSEName != "") SoundManager.Instance.StartPlayback(m_closeSEName);
            return true;
        }
        return false;
    }


    //===============================================================================================
    //                              GlobalVolume/DepthOfField
    //===============================================================================================


    // 使用するGlobalVolumeをセット
    protected void SetGlobalVolume()
    {
        if (m_depthOfFieldType != DepthOfFieldType.Enable) return;

        // オブジェクトを取得
        var data = GameObject.Find("Global Volume");
        if (data == null)
        {
            Debug.LogError("GlobalVolumeがヒエラルキーに存在していません");
            return;
        }

        // コンポーネントを取得
        m_globalVolume = data.GetComponent<Volume>();
        if (m_globalVolume == null)
        {
            Debug.LogError("Volumeコンポーネントがアタッチされていません");
            return;
        }
    }


    /// <summary>
    /// 被写界深度を開始
    /// </summary>
    protected void ActiveTrueDepthOfField()
    {
        if (m_depthOfFieldType != DepthOfFieldType.Enable) return;
        if (m_globalVolume == null) return;

        if (m_globalVolume.profile.TryGet<DepthOfField>(out var depthOfField))
        {
            if (depthOfField.active == true)
            {
                // 既に被写界深度が設定されていたことをセット
                m_alreadyDepthOfField = true;
            }
            else
            {
                // 被写界深度を開始
                depthOfField.active = true;
            }
        }
    }

    /// <summary>
    /// 被写界深度を終了
    /// </summary>
    protected void ActiveFalseDepthOfField()
    {
        if (m_depthOfFieldType != DepthOfFieldType.Enable) return;
        if (m_globalVolume == null) return;
        if (m_alreadyDepthOfField == true) return;

        // 被写界深度を終了
        if (m_globalVolume.profile.TryGet<DepthOfField>(out var depthOfField))
        {
            depthOfField.active = false;
        }
    }


    //===============================================================================================
    //                              ゲーム停止
    //===============================================================================================


    // ゲームを一時停止
    protected void PauseGame()
    {
        if (m_pauseGameType != PauseGameType.Enable) return;

        // タイムスケールをキープ
        m_alreadyPauseGameTimeScale = Time.timeScale;

        // 停止
        Time.timeScale = 0.0f;
    }


    // ゲームを再開
    protected void ResumeGame()
    {
        if (m_pauseGameType != PauseGameType.Enable) return;

        // 基のタイムスケールに戻す
        Time.timeScale = m_alreadyPauseGameTimeScale;
    }


    //===============================================================================================
    //                                      UI非表示
    //===============================================================================================

    /// <summary>
    /// ウィンドウの表示
    /// </summary>
    protected async UniTask ShowDOAlpha()
    {
        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupはnullです");
            return;
        }

        // ゲーム停止中でも動くか
        bool isUpdate = false;
        if (m_pauseGameMoveType == PauseGameMoveType.Enable) isUpdate = true;

        // 表示を開始(0.0fから基の透明度に)
        await m_canvasGroup.DOFade(endValue: 0.0f, duration: m_doSpead).
            From().
            SetUpdate(isUpdate).
            SetLink(m_canvasGroup.gameObject);
    }


    /// <summary>
    /// ウィンドウの非表示
    /// </summary>
    protected async UniTask CloseDOAlpha()
    {
        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupはnullです");
            return;
        }

        // ゲーム停止中でも動くか
        bool isUpdate = false;
        if (m_pauseGameMoveType == PauseGameMoveType.Enable) isUpdate = true;


        // 表示を開始(基の透明度から0.0fに)
        await m_canvasGroup.DOFade(endValue: 0.0f, duration: m_doSpead).
            SetUpdate(isUpdate).
            SetLink(m_canvasGroup.gameObject);
    }


    // 隠すUIListを非表示（吉田）
    protected async UniTask HideOtherUI()
    {
        if (m_hideUIType != HideUIType.Enable) return;

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            List<TweenerCore<float, float, FloatOptions>> tweenList = new();

            // 隠し用UIリストを非表示
            foreach (var canvas in m_hideCanvasGroupList)
            {
                if (canvas == null) continue;

                var fade = canvas.DOFade(0.0f, 0.1f)
                .SetEase(Ease.Linear)
               .SetUpdate(true)
                .SetLink(canvas.gameObject);

                tweenList.Add(fade);
            }

            // 非表示が終了するまで待機
            foreach (var tween in tweenList)
            {
                if (tween != null && tween.active)
                {
                    await tween.AsyncWaitForCompletion();
                }

                cancelToken.ThrowIfCancellationRequested();
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }
    }


    // 隠すUIListを表示（吉田）
    protected async UniTask ShowOtherUI()
    {
        if (m_hideUIType != HideUIType.Enable) return;

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            List<TweenerCore<float, float, FloatOptions>> tweenList = new();

            // 隠し用UIリストを表示
            foreach (var canvas in m_hideCanvasGroupList)
            {
                if (canvas == null) continue;

                var fade = canvas.DOFade(1.0f, 0.1f)
                    .SetEase(Ease.Linear)
                    .SetUpdate(true)
                     .SetLink(canvas.gameObject);

                tweenList.Add(fade);
            }

            // 表示が終了するまで待機
            foreach (var tween in tweenList)
            {
                if (tween != null && tween.active)
                {
                    await tween.AsyncWaitForCompletion();
                }
                cancelToken.ThrowIfCancellationRequested();
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }
    }


    // UIの集まりを取得（吉田）
    // TODO：スタート時に一度だけ呼び出す（今は敵などを実行中に増やすこともあるため、毎回やってる）
    private void SetCanvasGroupList()
    {
        // typeで指定した型の全てのオブジェクトを配列で取得し,その要素数分繰り返す

        GameObject[] allObjects = FindObjectsOfType<GameObject>(true);
        foreach (GameObject obj in allObjects)
        {

            // 自オブジェクトとその子オブジェクトを除外する
            if (obj == gameObject || obj.transform.parent == gameObject)
            {
                continue;
            }

            // シーン上に存在するオブジェクトならば処理.
            if (!obj.activeInHierarchy) continue;

            if (obj.layer != LayerMask.NameToLayer("UI")) continue;

            if (obj.TryGetComponent<CanvasGroup>(out CanvasGroup canvasGroup))
            {
                // 既に隠れていればリストに追加しない(田内)
                if (canvasGroup.alpha <= 0.0f) continue;

                // 隠す用リストに追加
                m_hideCanvasGroupList.Add(canvasGroup);
            }
        }
    }

}
