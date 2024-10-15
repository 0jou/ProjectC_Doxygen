using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class DoAlphaCanvasGroup : BaseDoTweenUI
{
    // 制作者 田内
    // キャンバスグループで透明度を変更する


    [Header("UICanvasGroup")]
    [SerializeField]
    protected CanvasGroup m_canvasGroup = null;


    [Header("初期化の透明度")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_initializeAlpha = 1.0f;


    [Header("ターゲットの透明度")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private List<float> m_targetAlphaList = new();

    //========================================================
    //                       実行処理
    //========================================================


    protected override void OnInitialize()
    {
        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupがシリアライズされていません");
            return;
        }

        // 初期の透明度をセット
        m_canvasGroup.alpha = m_initializeAlpha;
    }


    public override void StartDoTween()
    {

        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupがシリアライズされていません");
            return;
        }

        // 一度初期化
        KillSequence();

        // 開始
        m_sequence = DOTween.Sequence();

        foreach (var alpha in m_targetAlphaList)
        {
            m_sequence.Append(
            m_canvasGroup.DOFade(alpha, m_duration).
            SetDelay(m_delay).
            SetEase(m_ease).
            SetLink(gameObject)).
            SetUpdate(m_isGameStopMove);
        }

        m_sequence.SetLoops(m_loopCount, m_loopType);
        m_sequence.SetUpdate(m_isGameStopMove);
    }

}
