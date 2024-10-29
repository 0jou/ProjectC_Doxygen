/*!
 * @file StateRecursionWithProbability.cs
 * @brief 確率で特定のステートに遷移する
 */

using UnityEngine;
using Arbor;

/// <summary>
/// @brief 確率で特定のステートに遷移する
/// 確率は0~100の間(int)で設定する
/// 抽選の実行タイミングはOnStateBeginとOnStateUpdate
/// </summary>
[AddBehaviourMenu("Common/StateBehaviour")]
[AddComponentMenu("")]
public class StateRecursionWithProbability : StateBehaviour
{
    //! @brief 遷移先リンク
    [SerializeField]
    private StateLink m_successLink = null;

    //! @brief 失敗先リンク(田内)
    [SerializeField]
    private StateLink m_failLink = null;

    //! @brief 遷移確率
    [Header("遷移確率")]
    [SerializeField, Range(0, 100)]
    private int m_probability = 50;

    //! @brief 確率判定
    private bool IsSuccess()
    {
        return Random.Range(0, 100) < m_probability;
    }


    //! @brief 確率で遷移を実行する
    private void RecursionwithProbability()
    {
        if (IsSuccess())
        {
            if (m_successLink == null) return;
            Transition(m_successLink);
        }
        else
        {
            if (m_failLink == null) return;
            Transition(m_failLink);
        }
        
    }

    public override void OnStateBegin()
    {
        RecursionwithProbability();
    }
}
