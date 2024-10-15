using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;


[AddBehaviourMenu("Staff/CalcRotateBeenTargetObjectStaff")]
public class CalcRotateBeenTargetObjectStaff : BaseStaffRotationCalculator
{
    // 制作者 田内
    // ターゲットにされたオブジェクトの方向を確認する

    protected override void GetTargetPosition(ref Vector3 _targetPos)
    {
        var data = GetStaffData();
        if (data == null) return;

        if (0 < data.BeenTargetObjectList.Count)
        {
            var obj = data.BeenTargetObjectList[data.BeenTargetObjectList.Count - 1];

            _targetPos = obj.transform.position;
        }

    }
}
