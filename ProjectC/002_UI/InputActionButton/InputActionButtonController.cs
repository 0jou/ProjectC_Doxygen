using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputActionButtonController : MonoBehaviour
{
    // 制作者 田内
    // InputActionButtonを操作するコントローラー

    [Header("実行を管理するInputActionButtonリスト")]
    [SerializeField]
    private List<InputActionButton> m_inputActionButtonList = new();

    //===========================================
    //                  実行処理
    //===========================================

    private void Start()
    {
        // コントローラー用のボタンに変換
        foreach (var button in m_inputActionButtonList)
        {
            if (button == null) continue;
            button.UpdateType = InputActionButton.InputActionButtonUpdateType.Self;
        }
    }

    /// <summary>
    /// 実行処理
    /// </summary>
    public void OnInitialize()
    {
        foreach (var button in m_inputActionButtonList)
        {
            if (button == null) continue;

            button.OnInitialize();
        }
    }

    /// <summary>
    /// 実行処理
    /// </summary>
    public void OnUpdate()
    {
        foreach(var button in m_inputActionButtonList)
        {
            if (button == null) continue;

            button.OnUpdate();
        }
    }

}
