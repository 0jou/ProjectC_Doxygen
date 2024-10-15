using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/CalcChairPosCustomer")]
[AddComponentMenu("")]
public class CalcChairPosCustomer : BaseCustomerCalculator
{

    // 椅子の座標を取得する


    // Use this for calculate
    public override void OnCalculate()
    {

        var data = GetCustomerData();
        if (data == null) return;

        var tableData = data.TargetTableSetData;
        if (tableData?.ChairPoint == null) return;

        var pos = tableData.ChairPoint.position;
        m_outputPos.SetValue(pos);

    }
}
