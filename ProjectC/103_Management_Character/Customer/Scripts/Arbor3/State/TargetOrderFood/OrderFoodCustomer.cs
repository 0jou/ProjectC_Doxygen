using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using CustomerStateInfo;

[AddBehaviourMenu("Customer/OrderFoodCustomer")]
[AddComponentMenu("")]
public class OrderFoodCustomer : BaseCustomerStateBehaviour
{
    // 料理を注文する
    // 制作者　田内

    // 注文成功時
    [SerializeField]
    private StateLink m_successLink = null;

    // 注文失敗時
    [SerializeField]
    private StateLink m_failLink = null;

    //==================================================

    // 料理を注文する
    private void Order()
    {

        var data = GetCustomerData();
        if (data == null) return;

        // 既に作成していれば削除
        if(data.TargetOrderFoodData!=null)
        {
            Destroy(data.TargetOrderFoodData);
            data.TargetOrderFoodData = null;
        }

        // 料理をオーダーする
        data.TargetOrderFoodData = CounterManager.instance.OrderFood(data);

        if(data.TargetOrderFoodData!=null)
        {
            // 注文成功時
            SetTransition(m_successLink);
        }
        else
        {
            // 注文失敗時
            SetTransition(m_failLink);
        }

    }


    //===========================================================================


    // Use this for enter state
    public override void OnStateBegin()
    {
        Order();
    }

}
