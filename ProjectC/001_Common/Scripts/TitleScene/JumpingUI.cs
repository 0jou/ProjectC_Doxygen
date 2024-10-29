using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingUI : MonoBehaviour
{
    // 画像のRectTransform
    private RectTransform m_rectTransform = null;

    // ジャンプの高さと時間（インスペクターで変更可能）
    public float m_jumpHeight = 100f;
    public float m_jumpDuration = 0.5f;

    private Vector2 m_originalPosition = new();


    // 現在のジャンプTween
    private Tween m_jumpTween = null;

    private static bool isTweenInitialized = false;

    void Start()
    {
        if (!isTweenInitialized)
        {
            DOTween.Init();
            isTweenInitialized = true;
        }
        m_rectTransform = GetComponent<RectTransform>();

        // 画像をジャンプさせるアニメーションを開始
        StartJumpAnimation();
    }

    void StartJumpAnimation()
    {
        // 現在のTweenがある場合は停止
        StopAnimation();
        if(m_rectTransform == null)
        {
            return;
        }

        // 元の位置を保存
        m_originalPosition = m_rectTransform.anchoredPosition;


        // 画像をジャンプさせるアニメーション
        m_jumpTween = m_rectTransform.DOAnchorPosY(m_originalPosition.y + m_jumpHeight, m_jumpDuration)
            .SetLoops(-1, LoopType.Yoyo);
    }

    void OnValidate()
    {
        // インスペクターで値が変更されたときにジャンプアニメーションを更新
        if (Application.isPlaying && m_rectTransform != null)
        {
            m_rectTransform.anchoredPosition = m_originalPosition;

            StartJumpAnimation();
        }
    }


    void OnDestroy()
    {
        // オブジェクトが破棄されるときにTweenを破棄
        StopAnimation();
    }
    void OnDisable()
    {
        // オブジェクトが非アクティブになるときにTweenを破棄
        StopAnimation();
    }
    void OnEnable()
    {
        // オブジェクトがアクティブになるときにジャンプアニメーションを再開
        StartJumpAnimation();
    }

    private void StopAnimation()
    {
        if (m_jumpTween != null)
        {
            m_jumpTween.Kill();
        }
    }
}
