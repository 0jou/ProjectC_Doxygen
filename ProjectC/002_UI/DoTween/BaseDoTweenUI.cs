using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using NaughtyAttributes;


[DefaultExecutionOrder(0)]
public abstract class BaseDoTweenUI : MonoBehaviour
{
    // 制作者 田内
    // UIDoTweenの基底クラス

   
    //===================================
    // イージングの種類

    [BoxGroup("イージング")]
    [Header("EaseType")]
    [SerializeField]
    protected Ease m_ease = Ease.InSine;

    [BoxGroup("イージング")]
    [Header("間隔")]
    [SerializeField]
    protected float m_duration = 1.0f;

    [BoxGroup("イージング")]
    [Header("待機時間")]
    [SerializeField]
    protected float m_delay = 0.0f;


    //====================================
    // 処理の種類


    [BoxGroup("処理")]
    [Header("GameStopMove")]
    [SerializeField]
    protected bool m_isGameStopMove = false;


    protected enum UpdateType
    {
        Auto,
        Self,
    }


    [BoxGroup("処理")]
    [Header("UpdateType")]
    [SerializeField]
    protected UpdateType m_updateType = UpdateType.Auto;

    //====================================
    // ループの種類

    [BoxGroup("ループ")]
    [Header("ループ回数(無限ループは-1)")]
    [SerializeField]
    protected int m_loopCount = 0;

    [BoxGroup("ループ")]
    [Header("LoopType")]
    [SerializeField]
    protected LoopType m_loopType = LoopType.Yoyo;


    //====================================
    // 実行中のDoTween

    protected Sequence m_sequence = null;

    /// <summary>
    /// 実行中のSequence
    /// </summary>
    public Sequence Sequence
    {
        get { return m_sequence; }
    }


    //========================================================
    //                       実行処理
    //========================================================

    private void Start()
    {
        if (m_updateType == UpdateType.Self) return;

        OnInitialize();
        StartDoTween();
    }

    private void Update()
    {
        if (m_updateType == UpdateType.Self) return;

        OnUpdate();
    }

    private void OnDestroy()
    {
        // メモリリーク対策
        KillSequence();
    }

    protected void KillSequence()
    {
        if (m_sequence != null)
        {
            m_sequence.Kill();
            m_sequence = null;
        }
    }


    virtual protected void OnInitialize()
    {

    }


    virtual protected void OnUpdate()
    {

    }

    /// <summary>
    /// DOTweenを再生開始する
    /// </summary>
    public abstract void StartDoTween();

}
