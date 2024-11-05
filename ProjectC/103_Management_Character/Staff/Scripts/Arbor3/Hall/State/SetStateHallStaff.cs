using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using StaffInfo;

[AddBehaviourMenu("Staff/HallStaff/SetStateHallStaff")]
[AddComponentMenu("")]
public class SetStateHallStaff : BaseStaffStateBehaviour
{

    // スタッフのステートをセットする
    // 制作者　田内

    [Header("スタッフにセットするステート")]
    [SerializeField]
    private HallStaffState m_staffState = new();


    private void SetState()
    {
        var data = GetStaffData();
        if (data == null) return;

        data.CurrentHallStaffState = m_staffState;

    }

    // OnStateUpdate is called once per frame
    public override void OnStateBegin()
    {
        SetState();
    }

}
