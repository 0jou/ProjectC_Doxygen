using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using StaffStateInfo;

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

        if (data.CurrentStaffState != StaffState.Stun)
        {
            // TODO 終了
        }

    }


    // Use this for initialization
    void Start()
    {

    }

    // Use this for awake state
    public override void OnStateAwake()
    {
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
    }

    // Use this for exit state
    public override void OnStateEnd()
    {
    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        Stun();
    }

    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
    }
}
