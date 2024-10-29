using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using NaughtyAttributes;

public class DoScaleUI : BaseDoTweenUI
{
    // 制作者 田内
    // UIを拡縮する

    [Header("UIRectTransform")]
    [SerializeField]
    private RectTransform m_rectTransform = null;

    [Header("初期の大きさ")]
    [SerializeField]
    private Vector3 m_initializeScale = new Vector3(1.0f, 1.0f, 1.0f);

    [System.Serializable]
    private class DoScaleUIData : BaseEasingData
    {
        [Header("大きさ")]
        [SerializeField]
        private Vector3 m_targetScale = Vector3.one;

        public Vector3 TargetScale
        {
            get { return m_targetScale; }
        }
    }

    [Header("目標の大きさ")]
    [SerializeField]
    private List<DoScaleUIData> m_targetScaleList = new();


    //========================================================
    //                       実行処理
    //========================================================

    protected override void OnInitialize()
    {
        if (m_rectTransform == null)
        {
            Debug.LogError("RectTransformがシリアライズされていません");
            return;
        }

        // 初期のサイズをセット
        m_rectTransform.localScale = m_initializeScale;
    }


    public override void StartDoTween()
    {
        if (m_rectTransform == null)
        {
            Debug.LogError("RectTransformがシリアライズされていません");
            return;
        }


        // 一度初期化
        KillSequence();

        // 開始
        m_sequence = DOTween.Sequence();

        foreach (var data in m_targetScaleList)
        {
            m_sequence.Append(
            m_rectTransform.DOScale(data.TargetScale,data.Duration).
            SetDelay(data.Delay).
            SetEase(data.Ease).
            SetLink(gameObject)).
            SetUpdate(m_isGameStopMove);
        }

        m_sequence.SetLoops(m_loopCount, m_loopType);
        m_sequence.SetUpdate(m_isGameStopMove);

    }

}
