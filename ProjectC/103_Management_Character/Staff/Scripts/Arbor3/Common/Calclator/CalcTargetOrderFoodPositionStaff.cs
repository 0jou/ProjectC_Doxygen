using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;


[AddBehaviourMenu("Staff/CalcOrderFoodPositionStaff")]
public class CalcTargetOrderFoodPositionStaff : BaseStaffCalculator
{
    // 保持している料理の方向を見る
    // 制作者　田内

    public override void OnCalculate()
    {
        var data = GetStaffData();
        if (data == null) return;

        var orderFood = data.TargetOrderFoodData;
        if (orderFood == null) return;

        // 料理の方向を見る
        m_outputPos.SetValue(orderFood.transform.position);
    }
}
