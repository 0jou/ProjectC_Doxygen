using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// タイトル画面のメインメニューのボタンの処理(吉田)
/// </summary>
public class TitleMainMenuButton : MonoBehaviour
{
    // 選択中かどうか
    [ReadOnly]
    [SerializeField]
    private bool m_isSelected = false;
    public bool IsSelected => m_isSelected;

    [SerializeField]
    private CanvasGroup m_canvasGroup = null;

    [SerializeField]
    private BlinkingCanvasGroup m_blinkingCanvasGroup = null;

    [SerializeField]
    private Image m_image = null;

    // 未選択時の透明度
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_deselectedAlpha = 0.5f;

    // 未選択時の色
    [SerializeField]
    private Color m_deselectedColor = Color.white;

    #region Events 
    [Header("コールバック関数はバグの元なので、なるべく使わずスクリプトに書く")]
    [Space(10)]

    [Foldout("Events")]
    [SerializeField]
    private UnityEvent m_onSelected = null;

    [Foldout("Events")]
    [SerializeField]
    private UnityEvent m_onDeselected = null;

    [Foldout("Events")]
    [SerializeField]
    private UnityEvent m_onClick = null;
    #endregion

    private void Start()
    {
        if (m_canvasGroup == null)
        {
            if (TryGetComponent(out CanvasGroup canvasGroup))
            {
                m_canvasGroup = canvasGroup;
            }
            else
            {
                Debug.LogError("CanvasGroupが設定されていません。");
            }
        }

        if (m_blinkingCanvasGroup == null)
        {
            if (TryGetComponent(out BlinkingCanvasGroup blinkingCanvasGroup))
            {
                m_blinkingCanvasGroup = blinkingCanvasGroup;
            }
            else
            {
                Debug.LogError("BlinkingCanvasGroupが設定されていません。");
            }
        }

        if (m_image == null)
        {
            if (TryGetComponent(out Image image))
            {
                m_image = image;
            }
            else
            {
                Debug.LogError("Imageが設定されていません。");
            }
        }

        if(m_isSelected)
        {
            OnSelected();
        }
        else
        {
            OnDeselected();
        }
    }

    // 選択中 
    public void IsSelectedUpdate()
    {
        // 選択中になった瞬間
        if (!m_isSelected)
        {
            OnSelected();
        }
    }

    // 選択解除中
    public void IsDeselectedUpdate()
    {
        // 選択解除された瞬間
        if (m_isSelected)
        {
            OnDeselected();
        }

        if (m_canvasGroup)
        {
            m_canvasGroup.alpha = m_deselectedAlpha;
        }
        if (m_image)
        {
            m_image.color = m_deselectedColor;
        }
    }

    // 選択中になった瞬間
    private void OnSelected()
    {
        if (m_canvasGroup)
        {
            m_canvasGroup.alpha = 1.0f;
        }
        if (m_blinkingCanvasGroup)
        {
            m_blinkingCanvasGroup.StartEasing();
        }
        if (m_image)
        {
            m_image.color = Color.white;
        }

        m_isSelected = true;
        m_onSelected.Invoke();
    }

    // 選択解除された瞬間
    private void OnDeselected()
    {
        if (m_blinkingCanvasGroup)
        {
            m_blinkingCanvasGroup.EndEasing();
        }

        m_isSelected = false;
        m_onDeselected.Invoke();
    }

    public void OnClick()
    {
        m_onClick.Invoke();
    }

}
