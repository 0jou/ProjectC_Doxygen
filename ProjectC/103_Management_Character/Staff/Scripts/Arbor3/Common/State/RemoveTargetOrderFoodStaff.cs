using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Staff/HallStaff/RemoveOrderFoodHallStaff")]
[AddComponentMenu("")]
public class RemoveTargetOrderFoodStaff : BaseStaffStateBehaviour
{
    // ターゲットの料理を手放す
    // 制作者　田内

    //============================================
    //              実行処理
    //============================================

    // ターゲットの料理をnullにする
    void RemoveTargetOrderFood()
    {
        var data = GetStaffData();
        if (data == null) return;

        data.TargetOrderFoodData = null;
    }


    // Use this for enter state
    public override void OnStateBegin()
    {
        RemoveTargetOrderFood();
    }

}
