using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/CalcRotateQueueHeadCustomer")]
[AddComponentMenu("")]
public class CalcRotateQueueHeadCustomer : BaseCustomerRotationCalculator
{

    // 制作者　田内

    protected override void GetTargetPosition(ref Vector3 _targetPos)
    {
        var controller = QueueManager.instance;
        if (controller == null) return;

        var point = controller.HeadPoint;
        if (point == null) return;

        _targetPos = point.transform.position;
    }

}
