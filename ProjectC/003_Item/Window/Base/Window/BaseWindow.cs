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
        Enabled,    // 有効
        Disable     // 無効
    }

    [Header("被写界深度")]
    [SerializeField]
    protected DepthOfFieldType m_depthOfFieldType = DepthOfFieldType.Disable;

    //======================================

    protected enum GameStopType
    {
        Enabled,    // 有効
        Disable     // 無効
    }

    [Header("ゲームを停止")]
    [SerializeField]
    protected GameStopType m_gameStopType = GameStopType.Disable;

    //================================================================

    protected enum GameStopMoveType
    {
        Enabled,    // 有効
        Disable     // 無効
    }

    [Header("ゲーム停止中でも動く")]
    [SerializeField]
    protected GameStopMoveType m_gameStopMoveType = GameStopMoveType.Disable;

    //================================================================

    protected enum HideUIType
    {
        Enabled,    // 有効
        Disable     // 無効
    }

    [Header("他UIの非表示")]
    [SerializeField]
    protected HideUIType m_hideUIType = HideUIType.Disable;


    //======================================================

    [BoxGroup("ボタン")]
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

    //=======================================

    // 初期設定(非表示の状態)
    public virtual async UniTask OnInitialize()
    {

        // シェーダーをセット
        SetGlobalVolume();

        // ゲームを停止
        SetTimeScale(0.0f);

        // 隠すキャンバスリストをセット
        SetCanvasGroupList();

        // 他UIを非表示
        await HideOtherUI();

        await UniTask.CompletedTask;
    }

    // 表示時
    public virtual async UniTask OnShow()
    {

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 被写界深度のセット
            SetDepthOfField(true);

            // フェードイン
            await OnDOAlpha(m_doSpead, true);
            cancelToken.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    // 処理時
    public virtual async UniTask OnUpdate()
    {

        // ゲームを停止
        SetTimeScale(0.0f);

        // 被写界深度のセット
        SetDepthOfField(true);

        await UniTask.CompletedTask;

    }

    // 閉じる直前
    public virtual async UniTask OnClose()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 被写界深度のセット
            SetDepthOfField(false);

            // ゲームを再開
            SetTimeScale(1.0f);

            // フェードイン
            await OnDOAlpha(m_doSpead, false);
            cancelToken.ThrowIfCancellationRequested();

            // 他UIの表示
            await ShowOtherUI();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    // ウィンドウを削除する
    public virtual async UniTask OnDestroy()
    {
        Destroy(gameObject);

        await UniTask.CompletedTask;
    }



    protected async UniTask OnDOScale(Ease _ease, float _duration, bool _fromFlg, Vector3 _endValue = default)
    {
        bool isUpdate = false;

        // ゲームがストップ中でも動く
        if (m_gameStopMoveType == GameStopMoveType.Enabled)
        {
            isUpdate = true;
        }

        if (_fromFlg == true)
        {
            await transform.DOScale(endValue: _endValue, duration: _duration).
          From().
          SetEase(_ease).
          SetUpdate(isUpdate).
          SetLink(gameObject);
        }
        else
        {
            await transform.DOScale(endValue: _endValue, duration: _duration).
          SetEase(_ease).
          SetUpdate(isUpdate).
          SetLink(gameObject);
        }
    }


    protected async UniTask OnDOAlpha(float _duration, bool _fromFlg, float _endValue = 0.0f)
    {
        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupはnullです");
            return;
        }

        bool isUpdate = false;

        // ゲームがストップ中でも動く
        if (m_gameStopMoveType == GameStopMoveType.Enabled)
        {
            isUpdate = true;
        }

        if (_fromFlg == true)
        {
            await m_canvasGroup.DOFade(endValue: _endValue, duration: _duration).
                From().
                SetUpdate(isUpdate).
                 SetLink(gameObject);
        }
        else
        {
            await m_canvasGroup.DOFade(endValue: _endValue, duration: _duration).
                SetUpdate(isUpdate).
                 SetLink(gameObject);
        }
    }


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

        return m_closeInputActionButton.IsInputActionTrriger();
    }

    // 使用するGlobalVolumeをセット
    protected void SetGlobalVolume()
    {
        if (m_depthOfFieldType != DepthOfFieldType.Enabled) return;

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


    // 被写界深度をセット
    protected void SetDepthOfField(bool _isDepthOfField)
    {
        if (m_globalVolume == null) return;

        DepthOfField depthOfField = null;

        m_globalVolume.profile.TryGet(out depthOfField);

        if (depthOfField == null)
        {
            Debug.LogError("DepthOfFiledが存在しません");
            return;
        }

        if (depthOfField.active != _isDepthOfField)
        {
            depthOfField.active = _isDepthOfField;
        }
    }

    // ゲームを停止
    protected void SetTimeScale(float _timeScale)
    {
        if (m_gameStopType != GameStopType.Enabled) return;

        Time.timeScale = _timeScale;
    }


    // 隠すUIListを非表示（吉田）
    protected async UniTask HideOtherUI()
    {
        if (m_hideUIType != HideUIType.Enabled) return;

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            List<TweenerCore<float, float, FloatOptions>> tweenList = new();

            foreach (var canvas in m_hideCanvasGroupList)
            {

                var fade = canvas.DOFade(0.0f, 0.1f)
                .SetEase(Ease.Linear)
               .SetUpdate(true)
                .SetLink(gameObject);

                tweenList.Add(fade);
            }

            foreach (var tween in tweenList)
            {
                await tween.AsyncWaitForCompletion();
                cancelToken.ThrowIfCancellationRequested();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }


    // 隠すUIListを表示（吉田）
    protected async UniTask ShowOtherUI()
    {
        if (m_hideUIType != HideUIType.Enabled) return;

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            List<TweenerCore<float, float, FloatOptions>> tweenList = new();

            foreach (var canvas in m_hideCanvasGroupList)
            {
                var fade = canvas.DOFade(1.0f, 0.1f)
                    .SetEase(Ease.Linear)
                    .SetUpdate(true)
                    .SetLink(gameObject);

                tweenList.Add(fade);
            }

            foreach (var tween in tweenList)
            {
                await tween.AsyncWaitForCompletion();
                cancelToken.ThrowIfCancellationRequested();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
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


    /// <summary>
    /// ウィンドウコントローラーのインスタンスを生成して実行する
    /// </summary>
    public async UniTask<WindowType> CreateToUpdateWindow<WindowType>(WindowController _windowController, bool _isSelf, System.Func<WindowType, UniTask> onBeforeInitialize = null) where WindowType : BaseWindow
    {
        #region nullチェック
        if (_windowController == null)
        {
            Debug.LogError("引数のWindowControllerがnullです");
            return null;
        }
        #endregion

        var cancelToken = _windowController.GetCancellationTokenOnDestroy();
        try
        {
            // 作成したウィンドウのアップデート
            var window = await _windowController.CreateWindow<WindowType>(_isSelf, async _ =>
                  {
                      // ゲームストップ・被写界深度を停止
                      _.m_gameStopType = GameStopType.Disable;
                      _.m_depthOfFieldType = DepthOfFieldType.Disable;

                      // もし親ウィンドウがゲームをストップしていればストップ中でも動くようにする
                      if (m_gameStopType == GameStopType.Enabled)
                      {
                          _.m_gameStopMoveType = GameStopMoveType.Enabled;
                      }

                      // 追加の外部処理の実行
                      if (onBeforeInitialize != null)
                      {
                          // コンポーネントを取得し、動作を行う
                          if (_.TryGetComponent<WindowType>(out var tCom))
                          {
                              await onBeforeInitialize(tCom);
                              cancelToken.ThrowIfCancellationRequested();
                          }
                          else
                          {
                              Debug.LogError("コンポーネントが存在しませんでした。ジェネレートが合っているか確認してください");
                          }
                      }
                  });
            cancelToken.ThrowIfCancellationRequested();

            return window;

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
            return null;
        }


    }



}
