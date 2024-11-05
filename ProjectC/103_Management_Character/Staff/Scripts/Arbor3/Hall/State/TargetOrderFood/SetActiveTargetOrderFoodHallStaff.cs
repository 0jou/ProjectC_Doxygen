using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Staff/HallStaff/SetActiveTargetOrderFoodHallStaff")]
[AddComponentMenu("")]
public class SetActiveTargetOrderFoodHallStaff : BaseStaffStateBehaviour
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

    public override void OnStateBegin()
    {
        SetActive();
    }
}
