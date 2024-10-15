/*!
 * @file AddProbabilityEvent.cs
 * @brief Arbor経由でイベントを発生させる
 * */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using System.Diagnostics;

/// <summary>
/// @brief Arbor経由でイベントを発生させる
/// Arborのステートにアタッチして使用 内部インスペクターに
/// BaseManagementEvent.cs を継承したコンポーネントをセットしたオブジェクトのprefabをセットする
/// 確率も指定できる 0の場合はprefab側の設定が使用される
/// </summary>
[AddComponentMenu("")]
public class AddProbabilityEvent : StateBehaviour
{
    [Header("イベント")]
    [SerializeField]
    protected BaseManagementEvent m_managementEvent = null;

    [Header("イベント発生確率 0ならdef")]
    [SerializeField]
    [Range(0, 100)]
    private int m_probability = 50;


    /// <summary>
    /// @brief イベントを追加する
    /// @detail 確率が0超の場合は確率を設定する
    /// </summary>
    private void AddEvent()
    {
        if (m_managementEvent == null)
        {
            UnityEngine.Debug.LogError("イベントがセットできていません");
            return;
        }

        if (m_probability > 0)
        {
            m_managementEvent.Probability = m_probability;
        }

        // 継承先での専用処理実行
        EventInitializeProcess();

        ManagementEventManager.instance.AddProbabilityEventList(m_managementEvent);
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
