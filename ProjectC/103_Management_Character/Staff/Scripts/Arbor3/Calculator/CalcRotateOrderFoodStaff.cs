using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;


[AddBehaviourMenu("Staff/CalcRotationOrderFoodStaff")]
public class CalcRotateOrderFoodStaff : BaseStaffRotationCalculator
{
    // 保持している料理の方向を見る
    // 制作者　田内

    protected override void GetTargetPosition(ref Vector3 _targetPos)
    {

        var data = GetStaffData();
        if (data == null) return;

        var orderFood = data.TargetOrderFoodData;
        if (orderFood == null) return;

        // 料理の方向を見る
        _targetPos = orderFood.transform.position;

    }


}
