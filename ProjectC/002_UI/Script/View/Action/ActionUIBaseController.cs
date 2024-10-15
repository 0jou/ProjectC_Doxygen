using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ActionUIの基底クラス（吉田）
/// </summary>
public class ActionUIBaseController : MonoBehaviour
{
    // 制作者(吉田)
    protected bool m_isShowPlay = false;
    protected bool m_isClosePlay = false;

    // 初期設定(非表示の状態)
    public virtual UniTask OnInitialize()
    {
        return UniTask.CompletedTask;
    }

    // 表示時 関数の最初に呼び出す
    public virtual UniTask OnShow()
    {
        m_isShowPlay = true;
        gameObject.SetActive(true);
        return UniTask.CompletedTask;
    }

    // 処理時
    public virtual UniTask OnUpdate()
    {
        return UniTask.CompletedTask;
    }

    // 閉じる直前 関数の最後に呼び出す
    public virtual UniTask OnClose()
    {
        m_isClosePlay = false;
        gameObject.SetActive(false);
        return UniTask.CompletedTask;
    }

    // ウィンドウを削除する
    public virtual UniTask OnDestroy()
    {
        Destroy(gameObject);
        return UniTask.CompletedTask;
    }

}
