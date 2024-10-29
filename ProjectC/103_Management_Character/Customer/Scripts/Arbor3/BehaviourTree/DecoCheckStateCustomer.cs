/*!
 * @file DecoCheckStateCustomer.cs
 * @brief 客の状態が特定のステートか確認するデコレーター
 * CustomerState で判別する
 * @author 田内
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;
using CustomerStateInfo;

/// <summary>
/// @brief 客の状態が特定のステートか確認するデコレーター
/// CustomerState で判別する
/// </summary>
[AddComponentMenu("")]
public class DecoCheckStateCustomer : BaseCustomerDecorator
{
    // 制作者　田内
    // 客の状態で判別するデコレーター

    [Header("一致するか確認するステート")]
    [SerializeField]
    private List<CustomerState> m_customerStateList = new();

    protected override void OnAwake()
    {
    }

    protected override void OnStart()
    {
    }

    protected override bool OnConditionCheck()
    {
        var data = GetCustomerData();
        if (data == null) return false;

        foreach (var state in m_customerStateList)
        {
            // ステートが一致すれば
            if (data.CurrentCustomerState == state)
            {
                return true;
            }
        }

        // 不一致であれば
        return false;

    }

    protected override void OnEnd()
    {
    }
}
