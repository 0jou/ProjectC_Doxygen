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

    //==================================================

    // 料理を注文する
    private void Order()
    {

        var data = GetCustomerData();
        if (data == null) return;

        var controller = CounterManager.instance;
        if (controller == null) return;

        // 料理をオーダーする
        data.TargetOrderFoodData = controller.OrderFood(data);

        if (data.TargetOrderFoodData == null)
        {
            Debug.LogError("注文できませんでした");
        }

    }


    //===========================================================================


    // Use this for enter state
    public override void OnStateBegin()
    {
        Order();
    }

}
