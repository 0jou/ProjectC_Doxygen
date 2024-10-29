using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowUIControllerInputActionButton : InputActionButton
{
    // 制作者 田内
    // WindowUIController用ボタン

    [Header("ウィンドウUIコントローラー")]
    [SerializeField]
    private WindowUIController m_windowUIController = null;

    private enum WindowUIControllerInputActionButtonType
    {
        Next,
        Back,
    }

    [Header("タイプ")]
    [SerializeField]
    private WindowUIControllerInputActionButtonType m_windowUIControllerInputActionButtonType = WindowUIControllerInputActionButtonType.Next;

    //===========================================
    //              実行処理
    //===========================================

    protected override bool IsPress()
    {
        #region nullチェック
        if (m_windowUIController==null)
        {
            Debug.LogError("WindowUIControllerがシリアライズされていません");
            return false;
        }
        #endregion

        switch (m_windowUIControllerInputActionButtonType)
        {
            case WindowUIControllerInputActionButtonType.Next:
                {
                    return m_windowUIController.IsGoNext();
                }
            case WindowUIControllerInputActionButtonType.Back:
                {
                    return m_windowUIController.IsGoBack();
                }
            default:
                {
                    return false;
                }
        }
    }
}
