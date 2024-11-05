using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StaffInfo;

public partial class StaffData : MonoBehaviour
{
    // 制作者　田内

    //===========================================
    // スタッフのステート

   
    private ChefStaffState m_currentChefStaffState = ChefStaffState.Find;

    public ChefStaffState CurrentChefStaffState
    {
        get { return m_currentChefStaffState; }
        set { m_currentChefStaffState = value; }
    }


}
