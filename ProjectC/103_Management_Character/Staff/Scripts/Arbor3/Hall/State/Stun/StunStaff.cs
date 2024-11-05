using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using StaffInfo;

/// @defgroup UnUse

/// <summary>
/// @ingroup UnUse
/// 使われてない説
/// </summary>
[AddComponentMenu("")]
public class StunStaff : BaseStaffStateBehaviour
{

    // 制作者　田内
    // スタン状態でとどまる

    //================================================
    // 現在のステートを保存するためのDataSlot

    public DataSlot currentStateSlot;


    //================================================


    private void Stun()
    {
        var data = GetStaffData();
        if (data == null) return;

        if (data.CurrentHallStaffState != HallStaffState.Stun)
        {
            // TODO 終了
        }

    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        Stun();
    }
}
