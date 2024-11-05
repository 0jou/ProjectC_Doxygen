using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using OrderFoodInfo;

[AddComponentMenu("")]
public class CookingTargetOrderFoodChefStaff : BaseStaffStateBehaviour
{
    // 制作者 田内
    // 料理を作成する

    [SerializeField]
    private StateLink m_successLink = null;

    //========================================
    //              実行処理
    //========================================
    
    // 料理を作成する
    private void CookingTargetOrderFood()
    {
        var data = GetStaffData();
        if (data == null || data.TargetOrderFoodData == null) return;


        // 料理作成カウント実行
        // 引数はスタッフの能力値
        if (data.TargetOrderFoodData.CreatCount(data.CookingRatio))
        {
            SetTransition(m_successLink);   
        }

    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        CookingTargetOrderFood();
    }

}
