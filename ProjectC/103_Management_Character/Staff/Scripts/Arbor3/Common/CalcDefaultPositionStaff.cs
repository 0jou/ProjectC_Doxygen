using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;


[AddBehaviourMenu("Staff/CalcDefaultPosStaff")]
[AddComponentMenu("")]
public class CalcDefaultPositionStaff : BaseStaffCalculator
{
    // スタッフのデフォルト位置を取得する
    // 制作者　田内

    //===============================================


    // Use this for calculate
    public override void OnCalculate()
    {

        var data = GetStaffData();
        if (data == null) return;

        
        m_outputPos.SetValue(data.DefaultPos);


    }
}
