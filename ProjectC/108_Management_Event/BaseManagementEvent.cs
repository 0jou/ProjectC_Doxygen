/*!
 * @file BaseManagementEvent.cs
 * @brief 経営イベントの基底クラス
 * @author 田内
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagementEventInfo;
using ManagementGameInfo;

/// <summary>
/// @brief 経営イベントの基底クラス
/// マネージャーから管理される想定 (ManagementEventManager.cs)
/// </summary>
public class BaseManagementEvent : MonoBehaviour
{

    [Header("イベントID")]
    [SerializeField]
    private ManagementEventID m_eventID = ManagementEventID.Gangster;

    public ManagementEventID EventID
    {
        get { return m_eventID; }
    }


    //! @brief 動作が終了したかどうか
    private bool m_isEventEnd = false;

    //! @brief 動作終了かどうかの参照用
    public bool IsEventEnd
    {
        get { return m_isEventEnd; }
    }


    //====================================================
    //                  実行処理
    //====================================================


    //! @brief イベントの開始 継承先でオーバーライドする
    /// <summary>
    /// イベント開始処理
    /// </summary>
    virtual public void OnStart()
    {
        // 開始処理はここで行う
    }


    /// <summary>
    /// イベント実行処理
    /// </summary>
    virtual public void OnUpdate()
    {
        m_isEventEnd = true;
        Debug.LogError(this.GetType().Name + " クラスで OnUpdate がオーバーライドされていません");
    }


    /// <summary>
    /// イベント終了処理
    /// </summary>
    virtual public void OnExit()
    {
        // 終了処理があればここで行う
    }

    /// <summary>
    /// @brief イベント終了処理
    /// イベントの解決での終了か失敗での終了か指定できる
    /// </summary>
    /// <param name="_type"></param>
    protected void SetEventEnd(EventSolutionType _type)
    {
        if (m_isEventEnd) return;

        // イベント数を追加
        ManagementGameManager.instance.AddEventSolutionNum(_type);

        // イベント終了
        m_isEventEnd = true;
    }

}
