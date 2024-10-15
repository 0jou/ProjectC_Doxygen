using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameCustomerInfo;

namespace CameCustomerInfo
{
    public enum CameCustomerID
    {
        Normal,
        Angry,
    }
}

public class ManagementGameManager : BaseManager<ManagementGameManager>
{
    // 経営パートプレイ時の情報を管理するマネージャークラス
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

    private Dictionary<CameCustomerID, uint> m_cameCustomerNumDictionary = new();

    public Dictionary<CameCustomerID, uint> CameCustomerNumDictionary
    {
        get { return m_cameCustomerNumDictionary; }
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
    public void AddCameCustomerNum(CameCustomerID _id, uint _value = 1)
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
    /// 経営情報を更新する
    /// </summary>
    public void SetManagementData()
    {
        // この関数はプレイで稼いだ金額等を経営情報管理クラスに更新するものです
        // 適切なところで使用してください

        // 稼いだ金額を総額に追加
        ManagementDataManager.instance.AddTotalEarnedMoney(m_earnedMoney);

    }


}
