/*
 * @file SetStateCutomer.cs
 * @brief 客のステート情報( CustomerState )をセットする
 * @author 田内
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using CustomerStateInfo;


/// <summary>
/// @brief 客のステートをセットする
/// </summary>
[AddBehaviourMenu("Customer/SetCustomerStaff")]
[AddComponentMenu("")]
public class SetStateCutomer : BaseCustomerStateBehaviour
{

    // 客のステートをセットする
    // 制作者　田内

    [Header("スタッフにセットするステート")]
    [SerializeField]
    private CustomerState m_customerState = CustomerState.Normal;


    private void SetState()
    {
        var data = GetCustomerData();
        if (data == null) return;

        data.CurrentCustomerState = m_customerState;

    }

    // OnStateUpdate is called once per frame
    public override void OnStateBegin()
    {
        SetState();
    }

}
