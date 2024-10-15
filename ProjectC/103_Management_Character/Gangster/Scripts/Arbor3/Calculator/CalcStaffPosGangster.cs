using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Gangster/CalcStaffPosGangster")]
[AddComponentMenu("")]
public class CalcStaffPosGangster : BaseGangsterStateCalculator
{
    // ターゲットのスタッフの位置
    // 制作者　田内

    // Use this for calculate
    public override void OnCalculate()
    {
        var data = GetGangsterData();
        if (data?.TargetStaffData == null) return;

        var pos = data.TargetStaffData.transform.position;

        m_outputPos.SetValue(pos);

    }
}
