using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagementGameInfo;

namespace ManagementGameInfo
{
    public enum CameCustomerType
    {
        Normal,
        Angry,
    }

    /// <summary>
    /// @brief イベント終了時の解決したか否かの定義
    /// </summary>
    public enum EventSolutionType
    {
        Solution,
        UnSolution,
    }

}

public class ManagementGameManager : BaseManager<ManagementGameManager>
{
    // 経営パートプレイ時の情報を管理するクラス
    // 制作者　田内


    //============
    // 制限時間

    [Header("制限時間")]
    [SerializeField]
    [Range(0.1f, 600.0f)]
    private float m_timeLimit = 120.0f;

    public float TimeLimit
    {
        get { return m_timeLimit; }
    }

    //=============
    // 経過した時間

    private float m_elapsedTime = 0.0f;

    public float ElapsedTime
    {
        get { return m_elapsedTime; }
    }

    //=================
    // 稼いでいるお金

    private uint m_earnedMoney = 0;

    public uint EarnedMoney
    {
        get { return m_earnedMoney; }
    }

    //===========
    // 来店者数

    private Dictionary<CameCustomerType, uint> m_cameCustomerNumDictionary = new();

    public Dictionary<CameCustomerType, uint> CameCustomerNumDictionary
    {
        get { return m_cameCustomerNumDictionary; }
    }


    //==========================
    // イベント数

    private Dictionary<EventSolutionType, uint> m_eventSolutionNumDictionary = new();

    public Dictionary<EventSolutionType, uint> EventSolutionNumDictionary
    {
        get { return m_eventSolutionNumDictionary; }
    }

    //======================================================
    //                      実行処理
    //======================================================

    private void Update()
    {
        TimeCount();
    }

    // 経過時間をカウント
    private void TimeCount()
    {
        if (m_elapsedTime < m_timeLimit)
        {
            m_elapsedTime += Time.deltaTime;
        }
    }


    /// <summary>
    /// 稼いだ金額を追加
    /// </summary>
    public void AddEarnedMoney(uint _value)
    {
        m_earnedMoney += _value;
    }


    /// <summary>
    /// 来客数を追加
    /// </summary>
    public void AddCameCustomerNum(CameCustomerType _id, uint _value = 1)
    {

        if (m_cameCustomerNumDictionary.ContainsKey(_id))
        {
            m_cameCustomerNumDictionary[_id] += _value;
        }
        // 存在しなければ
        else
        {
            m_cameCustomerNumDictionary[_id] = _value;
        }
    }

    /// <summary>
    /// イベント数を追加
    /// </summary>
    public void AddEventSolutionNum(EventSolutionType _id, uint _value = 1)
    {

        if (m_eventSolutionNumDictionary.ContainsKey(_id))
        {
            m_eventSolutionNumDictionary[_id] += _value;
        }
        // 存在しなければ
        else
        {
            m_eventSolutionNumDictionary[_id] = _value;
        }
    }

    /// <summary>
    /// 経過時間が制限時間を過ぎたかどうか
    /// </summary>
    public bool IsTimeOver()
    {
        if (m_timeLimit < m_elapsedTime)
        {
            return true;
        }

        return false;
    }


    /// <summary>
    /// 料理が提供できるか
    /// </summary>
    public bool IsProvide()
    {
        // 提供できなければ
        if (ManagementProvideFoodManager.instance.IsCreateAll() == false) return true;

        return false;
    }


    /// <summary>
    /// ゲームが終了したかどうか
    /// </summary>
    public bool IsGameEnd()
    {
        if (IsProvide() || IsTimeOver())
        {
            // 客が残っていないか
            if (CustomerManager.instance.IsCustomerExistence() == false)
            {
                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// 経営情報を更新する
    /// この関数はプレイで稼いだ金額等を経営情報管理クラスに更新するものです
    /// 適切なところで使用してください
    /// </summary>
    public void SetManagementData()
    {
        // この関数はプレイで稼いだ金額等を経営情報管理クラスに更新するものです
        // 適切なところで使用してください

        // 稼いだ金額を総額に追加
        ManagementDataManager.instance.AddTotalEarnedMoney(m_earnedMoney);

    }


}
