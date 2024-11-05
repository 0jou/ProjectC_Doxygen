using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using OrderFoodInfo;

[AddBehaviourMenu("Staff/HallStaff/SetTargetOrderFoodOnHavePositionHallStaff")]
[AddComponentMenu("")]
public class SetTargetOrderFoodOnHavePositionHallStaff : BaseStaffStateBehaviour
{
    // 料理を運ぶ処理
    // 制作者　田内

    //=====================================================
    //                  実行処理
    //=====================================================

    // 料理を持つ
    private void SetTargetOrderFoodPosition()
    {
        // スタッフデータが存在するか確認
        var data = GetStaffData();
        if (data == null) return;

        // 料理データが存在するか確認
        var targetFoodData = data.TargetOrderFoodData;
        if (targetFoodData == null) return;

        // ターゲット料理の座標を持つ座標に更新
        Vector3 pos = data.GetHavePos();
        targetFoodData.transform.position = pos;
    }

    private void SetTargetFoodDataState()
    {
        // スタッフデータが存在するか確認
        var data = GetStaffData();
        if (data == null) return;

        // 料理データが存在するか確認
        var targetFoodData = data.TargetOrderFoodData;
        if (targetFoodData == null) return;

        // ターゲット料理ステートを移動状態に変更
        targetFoodData.CurrentOrderFoodState = OrderFoodState.Carry;
    }



    public override void OnStateBegin()
    {
        SetTargetFoodDataState();
    }


    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        // 料理を持つ
        SetTargetOrderFoodPosition();
    }
}
