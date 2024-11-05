using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using StaffInfo;

[AddComponentMenu("")]
public class SetStateChefStaff :BaseStaffStateBehaviour
{
    // シェフのステートをセットする
    // 制作者　田内

    [Header("シェフにセットするステート")]
    [SerializeField]
    private ChefStaffState m_chefStaffState = new();


    private void SetState()
    {
        var data = GetStaffData();
        if (data == null) return;

        data.CurrentChefStaffState = m_chefStaffState;

    }

    // OnStateUpdate is called once per frame
    public override void OnStateBegin()
    {
        SetState();
    }

}
