using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagementEventInfo;

namespace ManagementEventInfo
{
    public enum ManagementEventID
    {
        Gangster = 0,
        Cleaning = 1,
        DineDash = 2,
        EmptyDish=3,
    }
}

[CreateAssetMenu(fileName = "ManagementEventData", menuName = "ScriptableObjects/ManagementEvent/作成 ManagementEventData")]
[System.Serializable]
public class ManagementEventData : ScriptableObject
{
    //制作者 田内
    // 経営イベントデータ

    //=============================================

    [Header("ID")]
    [SerializeField]
    private ManagementEventID m_eventID = ManagementEventID.Gangster;

    public ManagementEventID EventID
    {
        get { return m_eventID; }
    }

    //=============================================

    [Header("イベント名")]
    [SerializeField]
    private string m_eventName = null;

    public string EventName
    {
        get { return m_eventName; }
    }

    //=============================================

    [Header("イベント説明文")]
    [TextArea(1, 10)]
    [SerializeField]
    private string m_eventDescription = null;

    public string EventDescription
    {
        get { return m_eventDescription; }
    }

    //=============================================

    [Header("イベント画像")]
    [SerializeField]
    private Sprite m_eventSprite = null;

    public Sprite EventSprite
    {
        get { return m_eventSprite; }
    }

    //=============================================

    [Header("発生イベント")]
    [SerializeField]
    private BaseManagementEvent m_event = null;

    public BaseManagementEvent Event
    {
        get { return m_event; }
    }


}
