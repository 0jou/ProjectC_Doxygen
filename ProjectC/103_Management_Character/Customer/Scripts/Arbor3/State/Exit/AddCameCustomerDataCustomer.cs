/*!
 * @file AddCameCustomerDataCustomer.cs
 * @brief 帰宅時に経営マネージャーに現在のステートを基準に来客者情報を追加する
 * @author 田内
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arbor;

using CameCustomerInfo;
using CustomerStateInfo;

/// <summary>
/// @brief 経営マネージャーに来客者情報を追加する
/// </summary>
[AddComponentMenu("")]
public class AddCameCustomerDataCustomer : BaseCustomerStateBehaviour
{
    // 制作者 田内
    // 経営マネージャーに来客者として追加する


    private void Add()
    {
        var data = GetCustomerData();
        if (data == null) return;

        switch (data.CurrentCustomerState)
        {
            // 問題なく帰った場合
            case CustomerState.Exit:
                {
                    ManagementGameManager.instance.AddCameCustomerNum(CameCustomerID.Normal);
                    break;
                }

            // 怒って帰った場合
            case CustomerState.AngryExit:
                {
                    ManagementGameManager.instance.AddCameCustomerNum(CameCustomerID.Angry);
                    break;
                }
        }
    }


    public override void OnStateBegin()
    {
        Add();
    }

}
