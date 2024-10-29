using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using DG.Tweening;

public class DOAnchorPosUI : BaseDoTweenUI
{

    // 制作者 田内
    // 目的座標まで移動させる

    [Header("UIRectTransform")]
    [SerializeField]
    private RectTransform m_rectTransform = null;

    [Header("初期の座標")]
    [SerializeField]
    private Vector3 m_initializePos = new Vector3(0.0f, 0.0f, 0.0f);

    [System.Serializable]
    private class DOAnchorPosUIData : BaseEasingData
    {
        [Header("ターゲット座標")]
        [SerializeField]
        Vector3 m_targetPos = Vector3.zero;

        public Vector3 TargetPos
        {
            get { return m_targetPos; }
        }
    }


    [Header("ターゲット座標")]
    [SerializeField]
    private List<DOAnchorPosUIData> m_targetPosList = new();


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
        m_rectTransform.anchoredPosition = m_initializePos;
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

        foreach (var data in m_targetPosList)
        {
            m_sequence.Append(
                m_rectTransform.DOAnchorPos(data.TargetPos, data.Duration).
                SetDelay(data.Delay).
                SetEase(data.Ease).
                SetLink(gameObject)).
                SetUpdate(m_isGameStopMove);
        }

        m_sequence.SetLoops(m_loopCount, m_loopType);
        m_sequence.SetUpdate(m_isGameStopMove);
    }

}
