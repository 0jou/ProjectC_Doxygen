using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ManagementEventInfo;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ManagementEventUI : MonoBehaviour
{
    // 制作者 田内
    // 経営イベントUI

    [Header("イベント画像")]
    [SerializeField]
    private Image m_eventImage = null;

    [Header("イベント名")]
    [SerializeField]
    private TextMeshProUGUI m_eventNameText = null;

    [Header("イベント説明文")]
    [SerializeField]
    private TextMeshProUGUI m_eventDescriptionText = null;


    // ターゲットのイベント
    private BaseManagementEvent m_targetManagementEvent = null;

    public BaseManagementEvent TargetManagementEvent
    {
        get { return m_targetManagementEvent; }
    }


    //==================================================
    //                  実行処理
    //==================================================

    private void Update()
    {
        CheckDestroy();
    }


    /// <summary>
    /// IDを基にUIをセットする
    /// </summary>
    public void SetUI(BaseManagementEvent _baseManagementEvent)
    {
        m_targetManagementEvent = _baseManagementEvent;
        if (m_targetManagementEvent == null) return;

        var data = ManagementEventManager.instance.ManagementEventDataBase.GetManagementEventData(m_targetManagementEvent.EventID);
        if (data == null) return;

        SetEventImage(data);

        SetEventNameText(data);

        SetEventDescriptionText(data);
    }


    private void SetEventImage(ManagementEventData _data)
    {
        if (m_eventImage == null) return;
        if (_data == null) return;

        m_eventImage.sprite = _data.EventSprite;
    }

    private void SetEventNameText(ManagementEventData _data)
    {
        if (m_eventNameText == null) return;
        if (_data == null) return;

        m_eventNameText.text = _data.EventName;
    }

    private void SetEventDescriptionText(ManagementEventData _data)
    {
        if (m_eventDescriptionText == null) return;
        if (_data == null) return;

        m_eventDescriptionText.text = _data.EventDescription;
    }


    private void CheckDestroy()
    {
        if (m_targetManagementEvent == null)
        {
            Destroy(gameObject);
        }
    }


}
