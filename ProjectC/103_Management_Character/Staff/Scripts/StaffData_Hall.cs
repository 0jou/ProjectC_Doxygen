using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StaffInfo;

public partial class StaffData : MonoBehaviour
{
    // 制作者 田内

    private HallStaffState m_currentHallStaffState = HallStaffState.Default;

    public HallStaffState CurrentHallStaffState
    {
        get { return m_currentHallStaffState; }
        set { m_currentHallStaffState = value; }
    }

}
