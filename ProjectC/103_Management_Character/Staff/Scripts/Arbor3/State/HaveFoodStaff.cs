using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using OrderFoodInfo;

[AddBehaviourMenu("Staff/HaveFoodStaff")]
[AddComponentMenu("")]
public class HaveFoodStaff : BaseStaffStateBehaviour
{
    // 料理を運ぶ処理
    // 制作者　田内

    //=====================================================

  
    // 料理を持つ
    private void Have()
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
        // スタッフデータが存在するか確認
        var data = GetStaffData();
        if (data == null) return;

        // 料理データが存在するか確認
        var targetFoodData = data.TargetOrderFoodData;
        if (targetFoodData == null) return;

        // 移動状態に変更
        targetFoodData.CurrentOrderFoodState = OrderFoodState.Carry;
    }

    // Use this for exit state
    public override void OnStateEnd()
    {
    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        // 料理を持つ
        Have();

    }

    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
    }
}
