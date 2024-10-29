using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

[RequireComponent(typeof(CanvasGroup))]
public class DetailUI : MonoBehaviour
{
    // 制作者 田内
    // 詳細な表示を行う時に使う表示・非表示ができるUI

    [Header("キャンバスグループ")]
    [SerializeField]
    private CanvasGroup m_canvasGroup = null;

    [Header("初期の状態")]
    [SerializeField]
    // 表示中かどうか
    private bool m_isDisplay = false;


    //======================================================
    //                     実行処理
    //======================================================


    /// <summary>
    /// 初期化処理
    /// </summary>
    virtual public async UniTask OnInitialize()
    {
        SetDisplay(m_isDisplay);

        await UniTask.CompletedTask;
    }



    /// <summary>
    /// 実行処理
    /// </summary>
    virtual public async UniTask OnUpdate()
    {
        CheckDisplay();

        await UniTask.CompletedTask;
    }



    private void CheckDisplay()
    {
        if (PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.UI,"Detail") == true)
        {
            SetDisplay(!m_isDisplay);
        }
    }


    private void SetDisplay(bool _isDeisplay)
    {

        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupがシリアライズされていません");
            return;
        }

        // 反転させる
        m_isDisplay = _isDeisplay;

        // 透明度を変更
        if (m_isDisplay == true)
        {
            m_canvasGroup.alpha = 1.0f;
        }
        else
        {
            m_canvasGroup.alpha = 0.0f;
        }
    }

}
