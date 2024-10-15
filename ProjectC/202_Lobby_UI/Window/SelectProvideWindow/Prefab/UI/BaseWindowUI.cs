using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

using UnityEngine.InputSystem;


[RequireComponent(typeof(CanvasGroup))]
public class BaseWindowUI : MonoBehaviour
{
    // 制作者 田内
    // 同ウィンドウで操作を分けたいUIの基底クラス

    //====================================================================
    // 選択されていなくても実行される処理
    //====================================================================

    /// <summary>
    /// 最初に一度初期化
    /// </summary>
    virtual public async UniTask OnInitialize()
    {
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 常に動作
    /// </summary>
    virtual public async UniTask OnUpdate()
    {
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 常に動作、OnUpdateの後処理
    /// </summary>
    virtual public async UniTask OnLateUpdate()
    {
        await UniTask.CompletedTask;
    }


    //====================================================================
    // 選択されている場合ににのみ実行される処理
    //====================================================================


    /// <summary>
    /// 選択されたときに一度初期化
    /// </summary>
    virtual public async UniTask OnSelectInitialize()
    {
        await UniTask.CompletedTask;
    }


    /// <summary>
    /// 選択中の場合に動作(OnUpdateより先に行われる)
    /// </summary>
    virtual public async UniTask OnSelectUpdate()
    {
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 選択終了時に終了処理
    /// </summary>
    virtual public async UniTask OnSelectExit()
    {
        await UniTask.CompletedTask;
    }


    //=========================================
    // その他の処理
    //=========================================


    [Header("キャンバスグループ")]
    [SerializeField]
    private CanvasGroup m_canvasGroup = null;


    [Header("非実行のUIカラー")]
    [SerializeField]
    protected float m_unUpdateAlpha = 0.5f;


    /// <summary>
    /// 透明度をセットする
    /// </summary>
    public void SetAlpha(bool _isUpdate)
    {
        if(m_canvasGroup==null)
        {
            Debug.LogError("CanvasGroupがシリアライズされていません");
            return;
        }

        if (_isUpdate)
        {
            m_canvasGroup.alpha = 1.0f;
        }
        else
        {
            m_canvasGroup.alpha = m_unUpdateAlpha;
        }
    }
}
