using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using OrderFoodInfo;

[AddBehaviourMenu("Staff/HallStaff/FindTargetOrderFoodHallStaff")]
[AddComponentMenu("")]
public class FindTargetOrderFoodHallStaff : BaseStaffStateBehaviour
{

    // 料理を探す
    // 制作者　田内

    //=====================================================

    // まだ受け取り手のいない料理を探す
    private void FindTargetFood()
    {
        var data = GetStaffData();
        if (data == null) return;

        // すでにターゲットの料理を見つけていれば
        if (data.TargetOrderFoodData != null) return;
       
        // ターゲットの料理を取得
        var food = CounterManager.instance.GetWaitHallStaffOrderFood();
        if (food != null)
        {
            // ターゲットにする料理をセット
            food.CurrentOrderFoodState = OrderFoodState.WaitCarry;
            data.TargetOrderFoodData = food;
        }

    }



    public override void OnStateUpdate()
    {
        // 料理を探す
        FindTargetFood();
    }

}
