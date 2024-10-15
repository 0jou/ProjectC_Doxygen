using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Staff/CalcTargetOrderFoodPosStaff")]
[AddComponentMenu("")]
public class CalcTargetOrderFoodPosStaff :BaseStaffCalculator
{
    // 料理の座標を取得
    // 制作者　田内


    // Use this for calculate
    public override void OnCalculate()
    {

        var data = GetStaffData();
        if (data == null) return;

        var foodData = data.TargetOrderFoodData;
        if (foodData?.CounterPointData?.DestinationPoint == null) return;

        // 出力用座標をセット
        var pos = foodData.CounterPointData.DestinationPoint.position;
        m_outputPos.SetValue(pos);

    }
}
