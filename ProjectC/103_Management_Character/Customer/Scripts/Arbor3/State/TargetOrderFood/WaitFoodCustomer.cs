/*!
 * @file WaitFoodCustomer.cs
 * @brief 料理が運ばれる/怒るまで待つステート
 * @author 田内
 *
 * @date 24/10/04 運ばれた際怒りカウントをリセットするよう変更 おかわりできるようにするため
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using CustomerStateInfo;
using OrderFoodInfo;

/// <summary>
/// @brief 料理が運ばれる/怒るまで待つステート
/// </summary>
[AddBehaviourMenu("Customer/WaitFoodCustomer")]
[AddComponentMenu("")]
public class WaitFoodCustomer : BaseCustomerStateBehaviour
{

    //========================================
    // ステート

    //! @brief 成功時のステートリンク
    [SerializeField]
    private StateLink m_successLink = null;

    //! @brief 失敗時のステートリンク
    [SerializeField]
    private StateLink m_failLink = null;

    //====================================

    /// <summary>
    /// @brief 怒りカウント/遷移
    /// m_failLinkに遷移する
    /// </summary>
    private void Count()
    {
        var data = GetCustomerData();
        if (data == null) return;

        // 怒ったら
        if (data.CountAngry())
        {
            // 怒る状態
            SetTransition(m_failLink);

            return;
        }
    }

    /// <summary>
    /// @brief 料理が運ばれたか確認/遷移
    /// m_successLinkに遷移する
    /// </summary>
    private void CheckCurry()
    {

        // 客データ
        var data = GetCustomerData();
        if (data == null) return;

        // 料理データ
        var targetFood = data.TargetOrderFoodData;
        if (targetFood == null) return;

        // ターゲットの料理がセットされれば
        if (targetFood.CurrentOrderFoodState == OrderFoodState.Set)
        {
            data.AngryCount = 0;

            SetTransition(m_successLink);
        }
    }


    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        Count();

        CheckCurry();
    }

}
