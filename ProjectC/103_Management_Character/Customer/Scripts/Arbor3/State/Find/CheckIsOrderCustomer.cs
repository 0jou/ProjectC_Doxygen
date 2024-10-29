/*!
 * @file CheckIsOrderCustomer.cs
 * @brief 料理が提供できない状態(在庫切れ)であればステートを遷移
 * @author 田内
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using CustomerStateInfo;

/// <summary>
/// @brief 料理が提供できない状態(在庫切れ)であればステートを遷移
/// </summary>
[AddComponentMenu("Customer/CheckIsOrderCustomer")]
public class CheckIsOrderCustomer : BaseCustomerStateBehaviour
{

    // 料理が提供できない状態になれば帰る
    // 制作者 田内

    [SerializeField]
    private StateLink m_stateLink = null;

    //==================================================================

    private void Check()
    {
        var data = GetCustomerData();
        if (data == null) return;

        // 料理が存在すれば
        if (data.TargetOrderFoodData != null) return;

        // この状態であれば
        if (data.CurrentCustomerState == CustomerState.WaitFood ||
            data.CurrentCustomerState == CustomerState.Eat ||
            data.CurrentCustomerState == CustomerState.AngryExit ||
            data.CurrentCustomerState == CustomerState.Exit)
        {
            return;
        }

        if (!CounterManager.instance.IsOrder())
        {
            SetTransition(m_stateLink);
        }
    }



    // Use this for awake state
    public override void OnStateAwake()
    {
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
    }

    // Use this for exit state
    public override void OnStateEnd()
    {
    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        Check();
    }

    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
    }
}
