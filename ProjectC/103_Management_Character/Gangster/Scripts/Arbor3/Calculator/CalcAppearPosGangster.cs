using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Gangster/CalcAppearPosGangster")]
[AddComponentMenu("")]
public class CalcAppearPosGangster : BaseGangsterStateCalculator
{
    // 制作者　田内
    // 帰る座標を取得

    // Use this for calculate
    public override void OnCalculate()
    {
        var data = GetGangsterData();
        if (data == null) return;

        var pos = data.AppearPos;
        m_outputPos.SetValue(pos);
    }
}
