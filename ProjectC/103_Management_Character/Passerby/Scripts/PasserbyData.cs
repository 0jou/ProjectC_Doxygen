using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PasserbyStateInfo;

namespace PasserbyStateInfo
{
    public enum PasserbyState
    {
        Exit,   // 出口へ移動
    }
}


public class PasserbyData : MonoBehaviour
{

    // 通行人データ
    // 制作者　田内

    //==========================================================
    // 通行人ステート

    private PasserbyState m_currentPasserbyState = PasserbyState.Exit;

    public PasserbyState CurrentPasserbyState
    {
        get
        {
            return m_currentPasserbyState;
        }
        set
        {
            m_currentPasserbyState = value;
        }


    }


    //==========================================================
    // 出口座標

    private Vector3 m_exitPos = new();

    public Vector3 ExitPos
    {
        get
        {
            return m_exitPos;
        }
        set
        {
            m_exitPos = value;
        }
    }

    //==========================================================

}
