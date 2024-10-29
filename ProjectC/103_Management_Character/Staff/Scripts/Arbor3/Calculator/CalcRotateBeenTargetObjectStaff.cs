using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;


[AddBehaviourMenu("Staff/CalcRotateBeenTargetObjectStaff")]
public class CalcRotateBeenTargetObjectStaff : BaseStaffCalculator
{
    // 制作者 田内
    // ターゲットにされたオブジェクトの方向を確認する

    public override void OnCalculate()
    {
        var data = GetStaffData();
        if (data == null) return;

        if (0 < data.BeenTargetObjectList.Count)
        {
            var obj = data.BeenTargetObjectList[data.BeenTargetObjectList.Count - 1];

            m_outputPos.SetValue(obj.transform.position);
        }
    }

}
