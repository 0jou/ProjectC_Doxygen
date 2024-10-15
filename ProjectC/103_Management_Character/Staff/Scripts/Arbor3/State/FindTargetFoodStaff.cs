using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using OrderFoodInfo;

[AddBehaviourMenu("Staff/FindTargetFoodStaff")]
[AddComponentMenu("")]
public class FindTargetFoodStaff : BaseStaffStateBehaviour
{

    // 料理を探す
    // 制作者　田内

    //=====================================================

    // まだ受け取り手のいない料理を探す
    private void FindTargetFood()
    {

        var data = GetStaffData();
        if (data == null) return;

        // すでにターゲットの料理を見つけていれば
        if (data.TargetOrderFoodData != null) return;
       

        var counterController = CounterManager.instance;
        if (counterController == null) return;


        var food = counterController.GetCounterFoodData();
        if (food != null)
        {

            food.CurrentOrderFoodState = OrderFoodState.WaitCounter;

            // ターゲットにする料理をセット
            data.TargetOrderFoodData = food;
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
    }

    // Use this for exit state
    public override void OnStateEnd()
    {
    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        // 料理を探す
        FindTargetFood();
    }

    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
    }
}
