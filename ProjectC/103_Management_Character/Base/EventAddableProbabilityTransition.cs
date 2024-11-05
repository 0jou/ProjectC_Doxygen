/*!
 * @file EventAddableTransition.cs
 * @brief 経営イベントの追加が可能か/確率抽選を判定し、ステートの遷移を行う
 * @author 上甲
 */

using Arbor;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// @brief 経営イベントの追加が可能か/確率抽選を判定し、ステートの遷移を行う
/// リンクを設定されていない場合は何もしない
/// </summary>
[BehaviourTitle("イベントの追加が可能か判定/遷移")]
public class EventAddableProbabilityTransition : StateBehaviour
{
    [SerializeField]
    private StateLink m_successLink = null;

    [SerializeField]
    private StateLink m_failLink = null;

    [Header("遷移確率")]
    [SerializeField, Range(0, 100)]
    private int m_probability = 50;


    //! @brief 確率判定
    private bool ProbabilityLottery()
    {
        return Random.Range(0, 100) < m_probability;
    }

    //! @brief 追加可能か
    private bool IsAddable()
    {
        bool addable = !ManagementEventManager.instance.IsEventMax();
        addable &= ProbabilityLottery();
        return addable;
    }

    public override void OnStateBegin()
    {
        if (IsAddable() && m_successLink != null)
        {
            Transition(m_successLink);
        }
        else if (m_failLink != null)
        {
            Transition(m_failLink);
        }
    }
}
