using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャンバスグループを点滅させる
/// </summary>

public class BlinkingCanvasGroup : MonoBehaviour
{

    [Header("点滅させるキャンバスグループ")]
    [SerializeField]
    private CanvasGroup m_canvasGroup = null;

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

    [Header("最小値から始める")]
    [SerializeField]
    private bool m_isStartMin = false;

    [Header("初めから点滅するかどうか")]
    [SerializeField]
    private bool m_isStartBlink = false;


    private void Awake()
    {
        if (m_canvasGroup != null) return;

        m_canvasGroup = GetComponent<CanvasGroup>();
        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupがアタッチされていません。");

            // コンポーネント破棄
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_isStartBlink)
        {
            StartEasing();
        }
    }


    public void StartEasing()
    {
        if (m_canvasGroup == null) return;
        if (m_canvasGroup.DOPlay() > 0) return;

        m_canvasGroup.alpha = m_maxAlpha;
        var doFade =
         m_canvasGroup.DOFade(m_minAlpha, m_blinkSpeed)
             .SetEase(Ease.InOutSine)
             .SetLoops(-1, LoopType.Yoyo);

        if (m_isStartMin)
        {
            doFade.From();
        }
    }

    public void EndEasing()
    {
        if (m_canvasGroup == null) return;

        m_canvasGroup.DOKill();
        m_canvasGroup.alpha = m_maxAlpha;
    }
}
