using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

using DG.Tweening;

using TMPro;

public class ShowSpeechBubble : MonoBehaviour
{
    // 吹き出しを表示するかしないか
    // 制作者 田内

    private enum SpeechBubbleType
    {
        Alway,     // 常に
        Appearance, // 近づいたら

    }



    [Header("表示/非表示を行うキャンバス")]
    [SerializeField]
    private CanvasGroup m_canvasGroup = null;

    [Header("DOスピード")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    protected float m_doSpead = 0.2f;


    [Header("種類")]
    [SerializeField]
    private SpeechBubbleType m_type = SpeechBubbleType.Alway;


    [EnableIf("m_type", SpeechBubbleType.Appearance)]
    [Header("当たり判定を行うTag")]
    [SerializeField]
    [Tag]
    private string m_tag = "Player";
    private GameObject m_tagetObject = null;


    [EnableIf("m_type", SpeechBubbleType.Appearance)]
    [Header("距離")]
    [SerializeField]
    private float m_distance = 5.0f;


    private void Start()
    {
        GetTargetObject();
        InitText();
    }

    private void Update()
    {
        CalcDistanceTargetObject();
    }

    private void CalcDistanceTargetObject()
    {
        if (m_type == SpeechBubbleType.Alway) return;
        if (m_tagetObject == null) return;

        var pos = gameObject.transform.position;
        var targetPos = m_tagetObject.transform.position;

        var range = Vector3.Distance(targetPos, pos);
        if (range <= m_distance)
        {
            OnShow();
        }
        else
        {
            OnHide();
        }
    }

    private void GetTargetObject()
    {
        // ターゲットのオブジェクトを検索
        m_tagetObject = GameObject.FindGameObjectWithTag(m_tag);
    }


    private void InitText()
    {
        if (m_type == SpeechBubbleType.Alway) return;

        if (m_canvasGroup == null) return;
        m_canvasGroup.alpha = 0.0f;
    }

    private void OnShow()
    {
        if (m_canvasGroup == null) return;
        m_canvasGroup.DOFade(endValue: 1.0f, duration: m_doSpead);
    }

    private void OnHide()
    {
        if (m_canvasGroup == null) return;
        m_canvasGroup.DOFade(endValue: 0.0f, duration: m_doSpead);
    }



}
