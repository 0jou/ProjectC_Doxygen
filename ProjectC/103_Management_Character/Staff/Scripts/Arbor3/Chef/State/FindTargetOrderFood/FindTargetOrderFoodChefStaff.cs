using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using OrderFoodInfo;

[AddComponentMenu("")]
public class FindTargetOrderFoodChefStaff : BaseStaffStateBehaviour
{
    // 制作者 田内
    // ターゲットにする料理を探す


    //=========================================
    //				実行処理
    //=========================================

    // 料理を探す
    private void FindTargetOrderFood()
    {
        // ターゲットにできる料理を探す
        var target = CounterManager.instance.GetWaitChefStaffOrderFood();
        if (target != null)
        {
            // ターゲットの料理にセット
            var data = GetStaffData();
            if (data == null) return;

            // ターゲットの料理を保持
            data.TargetOrderFoodData = target;
            data.TargetOrderFoodData.CurrentOrderFoodState = OrderFoodState.Cooking;

        }
    }


    public override void OnStateUpdate()
    {
        FindTargetOrderFood();
    }

}
