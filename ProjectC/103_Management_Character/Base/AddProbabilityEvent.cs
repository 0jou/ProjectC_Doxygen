/*!
 * @file AddProbabilityEvent.cs
 * @brief Arbor経由でイベントを発生させる
 * */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using System.Diagnostics;
using ManagementEventInfo;

/// <summary>
/// @brief Arbor経由でイベントを発生させる
/// Arborのステートにアタッチして使用 内部インスペクターに
/// BaseManagementEvent.cs を継承したコンポーネントをセットしたオブジェクトのprefabをセットする
/// 確率も指定できる 0の場合はprefab側の設定が使用される
/// </summary>
[AddComponentMenu("")]
public class AddProbabilityEvent : StateBehaviour
{
    [Header("発生させるイベント")]
    [SerializeField]
    private BaseManagementEvent m_event = null;

    [Header("イベント発生確率(100で確定)")]
    [SerializeField]
    [Range(0, 100)]
    private int m_probability = 50;

    protected BaseManagementEvent m_createManageentEvent = null;


    /// <summary>
    /// @brief イベントを追加する
    /// @detail 確率が0超の場合は確率を設定する
    /// </summary>
    private void AddEvent()
    {
        if (m_event == null) return;

        // 発生率が100%で無ければ
        if (m_probability < 100)
        {
            int rand = Random.Range(0, 100);
            if (m_probability < rand) return;
        }

        // イベント作成
        m_createManageentEvent = Instantiate(m_event);
      
        // 継承先での専用処理実行
        EventInitializeProcess();

        ManagementEventManager.instance.AddEventList(m_createManageentEvent);
    }


    //! @brief 継承先での情報設定用処理
    virtual protected void EventInitializeProcess()
    {
    }


    // Use this for enter state
    public override void OnStateBegin()
    {
        AddEvent();
    }

}
