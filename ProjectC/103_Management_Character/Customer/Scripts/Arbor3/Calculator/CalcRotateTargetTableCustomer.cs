using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/CalcRotateTargetTableCustomer")]
[AddComponentMenu("")]
public class CalcRotateTargetTableCustomer : BaseCustomerRotationCalculator 
{
    // ターゲットのテーブルの方向を向く
    // 制作者　田内

    protected override void GetTargetPosition(ref Vector3 _targetPos)
    {
        var data = GetCustomerData();
        if (data == null) return;

        var targetTable = data.TargetTableSetData;
        if (targetTable?.TablePoint == null) return;

        _targetPos = targetTable.TablePoint.position;
    }
}
