using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using OrderFoodInfo;

[AddBehaviourMenu("Staff/HallStaff/SetTargetOrderFoodOnTablePositionHallStaff")]
[AddComponentMenu("")]
public class SetTargetOrderFoodOnTablePositionHallStaff : BaseStaffStateBehaviour
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




    // Use this for enter state
    public override void OnStateBegin()
    {
        Set();
    }

}
