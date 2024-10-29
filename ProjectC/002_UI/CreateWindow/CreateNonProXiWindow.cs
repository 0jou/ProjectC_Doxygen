using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class CreateNonProXiWindow : MonoBehaviour
{
    //当たり判定なしにウィンドウを作成するクラス（山本）

    [Header("作成するウィンドウ")]
    [SerializeField]
    private GameObject m_windowController = null;

    // 作成したウィンドウ保持
    private GameObject m_createWindow = null;

    ///ウィンドウを作成
    public async UniTask CreateWindow()
    {
       
        if (m_createWindow != null)
        {
            Debug.Log("作成済みです");
            return;
        }

        if (m_windowController == null)
        {
            Debug.LogError("windowContorollerがシリアライズされていません");
            return;
        }

        m_createWindow = Instantiate(m_windowController);

        var controller = m_createWindow.GetComponent<WindowController>();
        if (controller == null)
        {
            Debug.LogError("WindowControllerコンポーネントがアタッチされていません");

            // 削除する
            if (m_createWindow != null)
            {
                Destroy(m_createWindow);
            }

            return;
        }

        // UIの非表示（吉田）
        await HideOtherUI();

        //SE音追加（山本）
        SoundManager.Instance.StartPlayback("SceneChange");

        await controller.CreateWindow<BaseWindow>();

        // UIの表示（吉田）
        await ShowOtherUI();

        // 削除する
        if (m_createWindow != null)
        {
            Destroy(m_createWindow);
        }

    }

    public async UniTask HideOtherUI()
    {
        // UIの集まり
        List<CanvasGroup> canvasGroups = ListCanvasGroup();
        List<TweenerCore<float, float, FloatOptions>> tweens = new();

        foreach (var canvas in canvasGroups)
        {
            var fade = canvas.DOFade(0.0f, 0.1f)
                .SetEase(Ease.Linear)
                .SetUpdate(false)
                .SetLink(gameObject);
            tweens.Add(fade);
        }

        foreach (var tween in tweens)
        {
            await tween.AsyncWaitForCompletion(); ;
        }
    }

    // UIの表示（吉田）
    public async UniTask ShowOtherUI()
    {
        // UIの集まり
        List<CanvasGroup> canvasGroups = ListCanvasGroup();
        List<TweenerCore<float, float, FloatOptions>> tweens = new();

        foreach (var canvas in canvasGroups)
        {
            var fade = canvas.DOFade(1.0f, 0.1f)
                .SetEase(Ease.Linear)
                .SetUpdate(false)
                .SetLink(gameObject);
            tweens.Add(fade);
        }

        foreach (var tween in tweens)
        {
            await tween.AsyncWaitForCompletion(); ;
        }
    }

    // UIの集まりを取得（吉田）
    // TODO：スタート時に一度だけ呼び出す（今は敵などを実行中に増やすこともあるため、毎回やってる）
    private List<CanvasGroup> ListCanvasGroup()
    {
        List<CanvasGroup> canvasGroups = new();

        // typeで指定した型の全てのオブジェクトを配列で取得し,その要素数分繰り返す.
        foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
        {
            // シーン上に存在するオブジェクトならば処理.
            if (!obj.activeInHierarchy) continue;

            if (obj.layer != LayerMask.NameToLayer("UI")) continue;

            if (obj.TryGetComponent<CanvasGroup>(out CanvasGroup canvasGroup))
            {
                canvasGroups.Add(canvasGroup);
            }
        }

        return canvasGroups;
    }

}
