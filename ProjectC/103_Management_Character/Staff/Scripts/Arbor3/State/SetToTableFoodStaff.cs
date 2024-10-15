using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using OrderFoodInfo;

[AddBehaviourMenu("Staff/SetToTableFoodStaff")]
[AddComponentMenu("")]
public class SetToTableFoodStaff : BaseStaffStateBehaviour
{

    // 料理をテーブルにセットする処理
    // 制作者　田内


    //==============================================================


    // テーブルに料理を設置する
    private void Set()
    {
        // スタッフデータ
        var data = GetStaffData();
        if (data == null) return;

        // 料理データ
        var targetFoodData = data.TargetOrderFoodData;
        if (targetFoodData != null)
        {

            // 設置されたことを記憶
            targetFoodData.CurrentOrderFoodState = OrderFoodState.Set;

            // テーブルの位置にアイテムをセット
            targetFoodData.transform.position = targetFoodData.TargetTableSetData.TablePoint.position;

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
        Set();
    }

    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
    }
}
