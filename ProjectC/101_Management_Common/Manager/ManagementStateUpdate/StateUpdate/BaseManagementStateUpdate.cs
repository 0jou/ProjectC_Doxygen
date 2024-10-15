using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ManagementStateUpdateInfo;

public class BaseManagementStateUpdate : BaseGameStateUpdate
{
    // 制作者 田内
    // 基底クラス

    [Header("自身のステート")]
    [SerializeField]
    private ManagementState m_managementState = ManagementState.Start;


    [Header("終了後に進むステート")]
    [SerializeField]
    private ManagementState m_nextManagementState = ManagementState.Start;


    public override int GetState()
    {
        // ステートをセット
        return (int)m_managementState;
    }


    public override int GetNextState()
    {
        return (int)m_nextManagementState;
    }

}
