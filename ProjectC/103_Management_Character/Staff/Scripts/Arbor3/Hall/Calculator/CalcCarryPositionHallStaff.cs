using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;


[AddBehaviourMenu("Staff/HallStaff/CalcCarryPositionHallStaff")]
[AddComponentMenu("")]
public class CalcCarryPositionHallStaff : BaseStaffCalculator
{
    // 料理を届ける座標を取得
    // 制作者　田内


    // Use this for calculate
    public override void OnCalculate()
    {
        // スタッフデータ
        var data = GetStaffData();
        if (data == null) return;


        // テーブルデータ
        var foodData = data.TargetOrderFoodData;
        if (foodData?.TargetTableSetData?.DestinationPoint == null) return;


        // 届ける座標をセット
        var pos = foodData.TargetTableSetData.DestinationPoint.position;
        m_outputPos.SetValue(pos);


    }
}
