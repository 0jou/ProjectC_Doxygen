using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using OrderFoodInfo;

[AddBehaviourMenu("Staff/FindTargetFoodStaff")]
[AddComponentMenu("")]
public class FindTargetFoodStaff : BaseStaffStateBehaviour
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
        var food = CounterManager.instance.GetCounterFoodData();
        if (food != null)
        {
            // ターゲットにする料理をセット
            food.CurrentOrderFoodState = OrderFoodState.WaitCounter;
            data.TargetOrderFoodData = food;
        }

    }



    public override void OnStateUpdate()
    {
        // 料理を探す
        FindTargetFood();
    }

}
