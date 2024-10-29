using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ManagementGameInfo;

public class DineDashEvent : BaseManagementEvent
{
    // 制作者 田内
    // 食い逃げイベント

    private GameObject m_targetObject = null;

    public GameObject TargetObject
    {
        get { return m_targetObject; }
        set { m_targetObject = value; }
    }

    //========================================
    //              実行処理
    //========================================

    public override void OnUpdate()
    {
        // ターゲットのオブジェクトがいなくなれば
        if (m_targetObject == null)
        {
            // イベント解決
            SetEventEnd(EventSolutionType.UnSolution);
        }
    }

}
