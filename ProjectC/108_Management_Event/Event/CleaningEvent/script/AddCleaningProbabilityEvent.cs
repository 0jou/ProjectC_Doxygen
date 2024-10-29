/*!
 * @file AddCleaningProbabilityEvent.cs
 * @brief Arborからクリーニングイベントを発生させる
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// @brief クリーニングイベントを発生させる
/// AddProbabilityEventを継承している
/// Arbor側で紐づけられるイベントスクリプトはCleaningEventにキャストできる必要がある
/// </summary>
[AddComponentMenu("")]
public class AddCleaningProbabilityEvent : AddProbabilityEvent
{
    [SerializeField]
    private float m_randomRange = 0.3f;

    /// <summary>
    /// @brief クリーニングイベント用の初期化処理
    /// キャストに失敗するとエラーを出力して終了
    /// </summary>
    protected override void EventInitializeProcess()
    {
        if (m_createManageentEvent == null) return;
        if (m_createManageentEvent is not GenerateCleaningEvent)
        {
            Debug.LogError("CleaningEventにキャストできません");
            return;
        }

        var castCleaningEvent = m_createManageentEvent as GenerateCleaningEvent;

        castCleaningEvent.SetPosition(gameObject.transform.root.position);
        castCleaningEvent.SetRandomRange(m_randomRange);
    }
}
