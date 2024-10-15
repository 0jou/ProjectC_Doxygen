using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using StaffStateInfo;

[AddBehaviourMenu("Staff/SetStateStaff")]
[AddComponentMenu("")]
public class SetStateStaff : BaseStaffStateBehaviour
{

    // スタッフのステートをセットする
    // 制作者　田内

    [Header("スタッフにセットするステート")]
    [SerializeField]
    private StaffState m_staffState = new();


    private void SetState()
    {
        var data = GetStaffData();
        if (data == null) return;

        data.CurrentStaffState = m_staffState;

    }

    // OnStateUpdate is called once per frame
    public override void OnStateBegin()
    {
        SetState();
    }

}
