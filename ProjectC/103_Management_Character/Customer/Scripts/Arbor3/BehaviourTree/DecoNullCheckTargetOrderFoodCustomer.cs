using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;
using OrderFoodInfo;

[AddComponentMenu("")]
public class DecoNullCheckTargetOrderFoodCustomer : BaseCustomerDecorator
{
    // 制作者　田内
    // 客のターゲット料理が存在するかで判別するデコレーター

    // 一度通ったら戻らないようにする
    private bool m_isStart = false;


    protected override bool OnConditionCheck()
    {

        var data = GetCustomerData();
        if (data == null) return false;

        if (data.TargetOrderFoodData != null || m_isStart)
        {
            // 一度通ったら常に通るようにする
            m_isStart = true;

            // ターゲットの料理が存在すれば
            return true;
        }
        else
        {
            // ターゲットの料理が存在しなければ
            return false;
        }
    }

}
