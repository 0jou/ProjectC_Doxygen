using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/CalcQueuePointPosCustomer")]
[AddComponentMenu("")]
public class CalcQueuePointPosCustomer : BaseCustomerCalculator
{
    // 待機列のポイント座標
    // 制作者　田内

    // Use this for calculate
    public override void OnCalculate()
    {
        var data = GetCustomerData();
        if (data == null) return;

        var queueData = data.QueueData;
        if (queueData == null) return;

        // 待機列ポイント座標をセット
        m_outputPos.SetValue(queueData.Destination);
    }
}
