using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトの表示非表示を制御するクラス
/// </summary>
public class ShowAndHideForCanvasGroup : MonoBehaviour
{

    [SerializeField]
    private CanvasGroup m_canvasGroup;

    [SerializeField]
    private bool m_isShow = false;
    public bool IsShow
    {
        get { return m_isShow; }
        private set { }
    }

    private void Start()
    {
        // nullチェック
        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroup を設定してください。");
        }

        // 初期化
        InitSetting();
    }

    [Button("Init Setting")]
    private void InitSetting()
    {
        // 初期化
        if (m_isShow)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void OnShow()
    {
        if (m_isShow) return;
        Show();
    }

    public void OnHide()
    {
        if (!m_isShow) return;
        Hide();
    }


    private void Show()
    {
        if (m_canvasGroup == null) return;

        m_isShow = true;

        m_canvasGroup.alpha = 1.0f;
    }

    private void Hide()
    {
        if (m_canvasGroup == null) return;

        m_isShow = false;

        m_canvasGroup.alpha = 0.0f;
    }

}
