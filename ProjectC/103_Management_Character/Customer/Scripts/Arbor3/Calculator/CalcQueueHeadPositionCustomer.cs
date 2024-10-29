using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/CalcQueueHeadPositionCustomer")]
[AddComponentMenu("")]
public class CalcQueueHeadPositionCustomer :BaseCustomerCalculator
{

    // 制作者　田内

    public override void OnCalculate()
    {
        var point = QueueManager.instance.HeadPoint;
        if (point == null) return;

        m_outputPos.SetValue(point.transform.position);
    }

}
