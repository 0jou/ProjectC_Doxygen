using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUIElementController : ActionUIBaseController
{
    // 制作者(吉田)

    //[Header("DOスピード")]
    //[SerializeField]
    private float m_doSpead = 0.1f;

    private CanvasGroup m_canvasGroup = null;

    // 初期設定(非表示の状態)
    public override UniTask OnInitialize()
    {
        base.OnInitialize();

        m_canvasGroup = GetComponent<CanvasGroup>();
        if(m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupコンポーネントがアタッチされていません");
            return UniTask.CompletedTask;
        }

        return UniTask.CompletedTask;
    }

    // 表示時
    public override async UniTask OnShow()
    {
        if (m_isShowPlay) return;
        await base.OnShow();

        if(m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupコンポーネントがアタッチされていません");
            return;
        }
       
        await m_canvasGroup.DOFade(1, m_doSpead)
            .SetUpdate(false)        // ゲームストップ中は動かない
            .SetLink(gameObject);

        m_isShowPlay = false;
    }

    // 閉じる直前
    public override async UniTask OnClose()
    {
        if (m_isClosePlay) return;
        m_isClosePlay = true;

        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupコンポーネントがアタッチされていません");
            return;
        }

        await m_canvasGroup.DOFade(0, m_doSpead)
            .SetUpdate(false)        // ゲームストップ中は動かない
            .SetLink(gameObject);

        await base.OnClose();
    }

}
