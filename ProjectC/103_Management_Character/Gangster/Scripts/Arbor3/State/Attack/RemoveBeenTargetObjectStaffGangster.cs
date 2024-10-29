using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBeenTargetObjectStaffGangster : BaseGangsterStateBehaviour
{

    // 制作者　田内
    // ターゲットのスタッフに自身のデータをセットする

    void RemoveTargetObject()
    {
        var data = GetGangsterData();
        if (data == null) return;

        if (data.TargetStaffData == null) return;

        data.TargetStaffData.BeenTargetObjectList.Remove(GetRootGameObject());
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
        RemoveTargetObject();
    }
}
