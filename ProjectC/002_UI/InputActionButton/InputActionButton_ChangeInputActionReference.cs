using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.InputSystem;

using NaughtyAttributes;
using UniRx;


[RequireComponent(typeof(CanvasGroup))]
public class InputActionButton_ChangeInputActionReference : InputActionButton
{
    // 制作者　吉田
    // 画面中心に出るUI表示に使ってます。
    //
    // 一つのオブジェクトを使いまわしているので、
    // inputActionReferenceを変更するとImageが変わるようにしています。

    //===============================================

    private System.IDisposable m_disposable = null;

    //===============================================

    public void ChangeInputActionReference(InputActionReference _nextInputAction)
    {
        if (m_inputActionReference == _nextInputAction) return;

        m_inputActionReference = _nextInputAction;
        ChangeActionReference();
    }


    private void Start()
    {
        ChangeActionReference();
    }

    private void ChangeActionReference()
    {
        if (m_disposable != null)
        {
            m_disposable.Dispose();
            m_disposable = null;
        }

        // デバイスが変更時にボタン画像を更新 
        //  InputActionReferenceが変更された時にも更新するため変数で保持
        m_disposable =
        PlayerInputManager.instance.CurrentDevice.Subscribe(device =>
        {
            UpdateButtonImage(device);
        }
        ).AddTo(this);
    }

}
