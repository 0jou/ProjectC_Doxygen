using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using DG.Tweening;

public class DoColorImageUI : BaseDoTweenUI
{
    // 制作者 田内
    // 色を変更する

    [Header("Image")]
    [SerializeField]
    private Image m_image = null;

    [Header("初期化のカラー")]
    [SerializeField]
    private Color m_initializeColor = Color.white;

    [Header("カラーリスト")]
    [SerializeField]
    private List<Color> m_colorList = new();

    //========================================================
    //                       実行処理
    //========================================================


    protected override void OnInitialize()
    {
        if (m_image == null)
        {
            Debug.LogError("Imageがシリアライズされていません");
            return;
        }

        m_image.color = m_initializeColor;
    }


    public override void StartDoTween()
    {
        // 一度初期化
        KillSequence();

        // 開始
        m_sequence = DOTween.Sequence();

        foreach (var color in m_colorList)
        {
            m_sequence.Append(
           m_image.DOColor(color, m_duration).
            SetDelay(m_delay).
            SetEase(m_ease).
            SetLink(gameObject)).
            SetUpdate(m_isGameStopMove);
        }

        m_sequence.SetLoops(m_loopCount, m_loopType);
        m_sequence.SetUpdate(m_isGameStopMove);
    }

}
