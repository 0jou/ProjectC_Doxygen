using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトの表示非表示を制御するクラス
/// </summary>
public class ShowAndHideForGameObject : MonoBehaviour
{

    [SerializeField]
    private GameObject m_target;

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
        if (m_target == null)
        {
            Debug.LogError("GameObject を設定してください。");
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
        if (m_target == null) return;

        m_isShow = true;

        m_target.SetActive(true);
    }

    private void Hide()
    {
        if (m_target == null) return;

        m_isShow = false;

        m_target.SetActive(false);
    }

}
