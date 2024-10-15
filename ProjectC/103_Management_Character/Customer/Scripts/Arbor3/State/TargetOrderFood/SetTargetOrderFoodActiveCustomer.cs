using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class SetTargetOrderFoodActiveCustomer : BaseCustomerStateBehaviour
{
    // 制作者　田内
    // ターゲットの料理のアクティブをセットする

    private void Set()
    {
        var data = GetCustomerData();
        if (data == null) return;

        if(data.TargetOrderFoodData!=null)
        {
            data.TargetOrderFoodData.gameObject.SetActive(false);
        }
    }


    // Use this for initialization
    void Start()
    {

    }

    // Use this for awake state
    public override void OnStateAwake()
    {
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
        Set();
    }

    // Use this for exit state
    public override void OnStateEnd()
    {
    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
    }

    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
    }
}
