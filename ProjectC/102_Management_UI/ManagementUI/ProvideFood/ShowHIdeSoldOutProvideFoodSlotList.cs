using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// 経営時、提供料理をの表示非表示を切り替える
/// 
/// </summary>

public class ShowHIdeSoldOutProvideFoodSlotList : MonoBehaviour, ISerializationCallbackReceiver
{

    [Header("キー情報")]
    [SerializeField]
    private InputActionButton m_inputActionButton = null;

    [Header("表示中か")]
    [SerializeField]
    private bool m_isShow = false;
    public bool IsShow { get { return m_isShow; } }

    [Header("非表示位置")]
    [SerializeField]
    private Vector3 m_hidePosition = new Vector3(0, 0, 0);

    [Header("表示位置")]
    [SerializeField]
    private Vector3 m_showPosition = new Vector3(0, 0, 0);


    void Start()
    {
        SettingPosByFlag();
    }

    void Update()
    {
        if (m_inputActionButton.IsInputActionTrriger())
        {
            Switch();

        }
    }

    void Switch()
    {
        // DOTweenのアニメーションがすでに入っている場合はリターン
        if (gameObject.transform.transform.DOPlay() > 0) return;

        m_isShow = !m_isShow;

        if(m_isShow)
        {
            // 表示アニメーション
            gameObject.transform.DOLocalMove(m_showPosition, 0.1f).
                SetUpdate(false).// タイムスケールに影響される
                SetLink(gameObject);
        }
        else
        {
            // 非表示アニメーション
            gameObject.transform.DOLocalMove(m_hidePosition, 0.1f).
                SetUpdate(false).// タイムスケールに影響される
                SetLink(gameObject);
        }
    }


    /// <summary>
    /// m_isShow　の値に合わせて位置を変更する
    /// </summary>
    [ContextMenu("位置変更")]
    private void SettingPosByFlag()
    {
        if (m_isShow)
        {
            transform.localPosition = m_showPosition;
        }
        else
        {
            transform.localPosition = m_hidePosition;
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        SettingPosByFlag();
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
    }
}
