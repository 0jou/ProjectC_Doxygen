using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/CalcTargetTablePositionCustomer")]
[AddComponentMenu("")]
public class CalcTargetTablePositionCustomer : BaseCustomerCalculator
{
    // ターゲットのテーブルの方向を向く
    // 制作者　田内

    public override void OnCalculate()
    {
        var data = GetCustomerData();
        if (data == null) return;

        var targetTable = data.TargetTableSetData;
        if (targetTable?.TablePoint == null) return;

        m_outputPos.SetValue(targetTable.TablePoint.position);
    }
}
