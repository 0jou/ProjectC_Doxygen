using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class AddDineDashEvent : AddProbabilityEvent
{
    // 制作者 田内
    // 食い逃げイベント用

    //============================================
    //              実行処理
    //============================================

    protected override void EventInitializeProcess()
    {
        if (m_createManageentEvent == null) return;
        if (m_createManageentEvent is DineDashEvent==false)
        {
            Debug.LogError("DineDashEventにキャストできません");
            return;
        }

        var eve = m_createManageentEvent as DineDashEvent;

        eve.TargetObject = gameObject;
    }
}
