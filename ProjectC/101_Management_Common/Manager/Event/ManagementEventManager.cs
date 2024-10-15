/*!
 * @file ManagementEventManager.cs
 * @brief 経営のイベントを管理するマネージャー
 * @author 田内
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

/// <summary>
/// @brief 経営のイベントを管理するマネージャー
/// BaseManagementEvent.cs を継承したイベントを抽選し実行する
/// </summary>
public class ManagementEventManager : BaseManager<ManagementEventManager>
{

    [Header("常にランダムで実行するイベントリスト")]
    [SerializeField]
    private List<BaseManagementEvent> m_eventList = new();

    // 作動中のイベント
    private BaseManagementEvent m_currentManagementEvent = null;

    // 外部から追加・実行を行うイベントのリスト
    private List<BaseManagementEvent> m_probabilityEventList = new();


    //======================================================================

    void Update()
    {
        OnUpdate();
        OnProbabilityEventListUpdate();
    }

    //======================================================================


    /// <summary>
    /// @brief 外部からイベントを追加し抽選・実行する
    /// </summary>
    public void AddProbabilityEventList(BaseManagementEvent _baseManagementEvent)
    {
        if (_baseManagementEvent == null) return;

        // 確率で実行する
        int rand = Random.Range(0, 100);
        if (_baseManagementEvent.Probability < rand) return;


        // リストに追加
        m_probabilityEventList.Add(_baseManagementEvent);

        // 初期処理
        _baseManagementEvent.OnStart();

    }


    //! 追加されたイベント群の抽選・実行を行う
    private void OnProbabilityEventListUpdate()
    {
        List<BaseManagementEvent> removeEveList = new();

        foreach (var eve in m_probabilityEventList)
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
            m_probabilityEventList.Remove(eve);
        }
    }

    //! @brief 実行中のイベントの処理を行う
    private void OnUpdate()
    {

        // 実行中のイベントがなければ抽選を行う
        if (m_currentManagementEvent == null)
        {
            if (LotteryToCreateEvent() == false)
            {
                return;
            }
            else
            {
                // 開始処理を行う
                m_currentManagementEvent.OnStart();
            }
        }

        // 処理を行う
        m_currentManagementEvent.OnUpdate();

        // 終了すれば
        if (m_currentManagementEvent.IsEventEnd)
        {
            // 終了処理を行う
            m_currentManagementEvent.OnExit();
            m_currentManagementEvent = null;
        }

    }


    //! @brief 抽選を行いイベントを作成する
    private bool LotteryToCreateEvent()
    {
        // ランダムに抽選する
        m_eventList.RandomList();

        BaseManagementEvent managementEvent = null;

        foreach (var eve in m_eventList)
        {
            if (eve == null) continue;

            int rand = Random.Range(0, 100);

            // 確率でイベントをセット
            if (eve.Probability < rand) continue;

            // 動作するイベントをセット
            managementEvent = eve;
            break;

        }

        // イベントがセットできていなければ
        if (managementEvent == null)
        {
            // イベントを確実にセット
            foreach (var eve in m_eventList)
            {
                if (eve == null) continue;
                managementEvent = eve;
                break;
            }
        }


        if (managementEvent == null)
        {
            // null
            m_currentManagementEvent = null;

            // 動作可能でなければ
            return false;
        }
        else
        {
            // 作成する
            m_currentManagementEvent = Instantiate(managementEvent);

            // 動作可能であれば
            return true;
        }

    }

}
