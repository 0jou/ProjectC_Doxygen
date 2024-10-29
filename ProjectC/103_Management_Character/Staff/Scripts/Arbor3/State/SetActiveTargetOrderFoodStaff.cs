using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class SetActiveTargetOrderFoodStaff : BaseStaffStateBehaviour
{

    // 制作者　田内
    // ターゲットの料理のアクティブをセットする

    [SerializeField]
    private bool m_active = false;

    private void SetActive()
    {
        var data = GetStaffData();
        if (data == null) return;

        var food = data.TargetOrderFoodData;

        if(food!=null)
        {
            food.gameObject.SetActive(m_active);
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
        SetActive();
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
