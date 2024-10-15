using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionUIGroupController : ActionUIBaseController
{
    // 制作者(吉田)

    private Vector3 m_initPos = Vector3.zero;

    //[Header("DOスピード")]
    //[SerializeField]
    private float m_doSpead = 0.3f;

    private float m_hidePosX = 300;

    // 初期設定(非表示の状態)
    public override UniTask OnInitialize()
    {
        m_initPos = transform.localPosition;
        m_initPos.z = 0;
        return UniTask.CompletedTask;
    }

    // 表示時
    public override async UniTask OnShow()
    {
        if (m_isShowPlay) return;
        await base.OnShow();

        transform.localPosition = new Vector3(m_initPos.x + m_hidePosX, m_initPos.y, m_initPos.z);
        await transform.DOLocalMoveX(
            endValue: m_initPos.x, duration: m_doSpead)
            .SetUpdate(false)        // ゲームストップ中は動かない
            .SetLink(gameObject);

        m_isShowPlay = false;
    }

    // 閉じる直前
    public override async UniTask OnClose()
    {
        if (m_isClosePlay) return;
        m_isClosePlay = true;

        transform.localPosition = new Vector3(m_initPos.x, m_initPos.y, m_initPos.z);
        await transform.DOLocalMoveX(
            endValue: m_initPos.x + m_hidePosX, duration: m_doSpead)
            .SetUpdate(false)        // ゲームストップ中は動かない
            .SetLink(gameObject);

        await base.OnClose();
    }
}
