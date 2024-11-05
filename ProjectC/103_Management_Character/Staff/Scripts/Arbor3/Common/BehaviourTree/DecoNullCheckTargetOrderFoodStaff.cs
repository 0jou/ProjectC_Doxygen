using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

[AddComponentMenu("")]
public class DecoNullCheckTargetOrderFoodStaff : BaseStaffDecorator
{
    // 制作者　田内
    // ターゲットとしている料理があるかどうか

    protected override bool OnConditionCheck()
    {
        var data = GetStaffData();
        if (data == null) return false;

        if (data.TargetOrderFoodData != null)
        {
            // 存在すれば
            return true;
        }
        else
        {
            // 存在しなければ
            return false;
        }

    }

}
