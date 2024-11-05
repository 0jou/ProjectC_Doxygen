using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;


[AddComponentMenu("")]
public class SetBeenTargetObjectStaffGangster : BaseGangsterStateBehaviour
{

    // 制作者　田内
    // ターゲットのスタッフに自身のデータをセットする

    void SetTargetObject()
    {

        var data = GetGangsterData();
        if (data == null) return;

        if (data.TargetStaffData == null) return;

        data.TargetStaffData.BeenTargetObjectList.Add(GetRootGameObject());
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
        SetTargetObject();
    }
}
