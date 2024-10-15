using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManagementInputActionButton : InputActionButton
{
    // 制作者 田内
    // 経営シーンへ移動するボタン


    override protected bool IsPress()
    {
        // 提供する料理を決めていなければ
        return ManagementProvideFoodManager.instance.IsSetProvideFood();
    }

}
