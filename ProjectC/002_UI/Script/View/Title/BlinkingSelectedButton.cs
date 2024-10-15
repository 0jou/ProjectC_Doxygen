using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///　選択されたボタンを点滅させる
/// </summary>

public class BlinkingSelectedButton : MonoBehaviour
{

    [Header("点滅させるキャンバスグループ")]
    [SerializeField]
    private CanvasGroup m_canvasGroup = null;

    [Header("ボタンコンポーネント")]
    [SerializeField]
    private Button m_button = null;

    [Header("点滅速度")]
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float m_blinkSpeed = 0.5f;

    [Header("点滅の最小値")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_minAlpha = 0.5f;

    [Header("点滅の最大値")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_maxAlpha = 1.0f;


    private void Awake()
    {
        // ボタンのnullチェック ------------------------------------------------
        if (m_button != null) return;

        m_button = GetComponent<Button>();
        if (m_button == null)
        {
            Debug.LogError("Buttonがアタッチされていません。");

            // コンポーネント破棄
            Destroy(this);
        }

        // キャンバスグループのnullチェック ------------------------------------------------
        if (m_canvasGroup != null) return;

        m_canvasGroup = GetComponent<CanvasGroup>();
        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupがアタッチされていません。");

            // コンポーネント破棄
            Destroy(this);
        }
    }

    private void Update()
    {
        if (m_button == null || m_canvasGroup == null) return;
        var selectable = m_button.FindSelectableOnDown();

        if (selectable == m_button)
        {
            if (m_canvasGroup.DOPlay() <= 0)
                StartEasing();
        }
        else
        {
            EndEasing();
        }
    }

    private void OnDisable()
    {
        EndEasing();
    }

    public void StartEasing()
    {
        if (m_canvasGroup == null) return;

        m_canvasGroup.DOFade(m_minAlpha, m_blinkSpeed)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void EndEasing()
    {
        if (m_canvasGroup == null) return;

        m_canvasGroup.DOKill();
        m_canvasGroup.alpha = m_maxAlpha;
    }
}
