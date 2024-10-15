using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using OrderFoodInfo;

[AddComponentMenu("")]
public class CheckTargetOrderFoodStateStaff : BaseStaffStateBehaviour
{

    // 制作者　田内
    // ターゲットの料理の状態で進むステートリンクを変更する

    //======================================================
    // ステートリンク

    [SerializeField]
    private StateLink m_waitCounterLink = null;
    
    [SerializeField]
    private StateLink m_carryLink = null;

    [SerializeField]
    private StateLink m_setLink = null;

    private void Check()
    {
        var data = GetStaffData();
        if (data == null) return;

        var targetFood = data.TargetOrderFoodData;
        if (targetFood == null) return;

        switch (targetFood.CurrentOrderFoodState)
        {
            // カウンターで待機していれば
            case OrderFoodState.WaitCounter:
                {
                    SetTransition(m_waitCounterLink);
                    break;
                }


            // 運ばれていれば
            case OrderFoodState.Carry:
                {
                    SetTransition(m_carryLink);
                    break;
                }

            // セットされていれば
            case OrderFoodState.Set:
                {
                    SetTransition(m_setLink);
                    break;
                }
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
        Check();
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
