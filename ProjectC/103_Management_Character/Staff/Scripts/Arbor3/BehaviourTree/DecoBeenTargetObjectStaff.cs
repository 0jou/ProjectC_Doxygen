using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

[AddComponentMenu("")]
public class DecoBeenTargetObjectStaff : BaseStaffDecorator
{

    // 制作者　田内
    // ターゲットのされているかどうかで判別するデコレーター

    protected override bool OnConditionCheck()
    {

        var data = GetStaffData();
        if (data == null) return false;

        if (0 < data.BeenTargetObjectList.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
