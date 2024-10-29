/*!
 * @file ManagementEventManager.cs
 * @brief 経営のイベントを管理するマネージャー
 * @author 田内
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @brief 経営のイベントを管理するマネージャー
/// BaseManagementEvent を継承したイベントを抽選し実行する
/// </summary>
public class ManagementEventManager : BaseManager<ManagementEventManager>
{

    [Header("データベース")]
    [SerializeField]
    private ManagementEventDataBase m_managementEventDataBase = null;

    public ManagementEventDataBase ManagementEventDataBase
    {
        get { return m_managementEventDataBase; }
    }


    // 発生中のイベントリスト
    private List<BaseManagementEvent> m_eventList = new();

    public List<BaseManagementEvent> EventList
    {
        get { return m_eventList; }
    }


    //==============================================
    //                 実行処理
    //==============================================

    void Update()
    {
        OnEventListUpdate();
    }


    /// <summary>
    /// @brief 外部からイベントを追加し抽選・実行する
    /// </summary>
    public void AddEventList(BaseManagementEvent _baseManagementEvent)
    {
        if (_baseManagementEvent == null) return;

        // リストに追加
        m_eventList.Add(_baseManagementEvent);

        // 初期処理
        _baseManagementEvent.OnStart();

    }


    //! 追加されたイベント群の抽選・実行を行う
    private void OnEventListUpdate()
    {
        // 終了イベントを取り除く
        List<BaseManagementEvent> removeEveList = new();

        foreach (var eve in m_eventList)
        {
            // 処理を行う
            eve.OnUpdate();

            // 終了すれば
            if (eve.IsEventEnd)
            {
                // 終了処理を行う
                eve.OnExit();
                removeEveList.Add(eve);
            }
        }

        foreach (var eve in removeEveList)
        {
            m_eventList.Remove(eve);
            Destroy(eve.gameObject);
        }
    }

}
