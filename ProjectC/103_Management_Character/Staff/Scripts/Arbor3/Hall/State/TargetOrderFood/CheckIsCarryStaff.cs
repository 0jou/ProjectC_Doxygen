using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using CustomerStateInfo;

[AddBehaviourMenu("Staff/CheckAngryStaff")]
[AddComponentMenu("")]
public class CheckIsCarryStaff : BaseStaffStateBehaviour
{

    // 料理を届けることができるのか確認する
    // 制作者　田内

    //============
    // ステート

    [SerializeField]
    private StateLink m_failLink = null;


    //==============================================================


    // 運ぶことができるか確認
    void IsCarry()
    {

        // スタッフデータ
        var data = GetStaffData();
        if (data == null) return;

        // 料理データ
        var targetFood = data.TargetOrderFoodData;

        // 運ぶ料理が存在しなければ
        if (targetFood == null)
        {
            SetTransition(m_failLink);
            return;
        }

    }


    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        // 常に確認する
        IsCarry();
    }
}
