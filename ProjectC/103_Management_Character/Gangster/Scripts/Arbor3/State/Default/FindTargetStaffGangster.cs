using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;


[AddComponentMenu("Gangster/FindTargetStaffGangster")]
public class FindTargetStaffGangster : BaseGangsterStateBehaviour
{
    // 攻撃するターゲットのスタッフを探す
    // 制作者　田内

    //=========================================================
    // リンク

    [SerializeField]
    private StateLink m_successLink = null;

    [SerializeField]
    private StateLink m_fallLink = null;

    //=========================================================

    private void FindStaff()
    {
        var data = GetGangsterData();
        if (data == null) return;

        var randomList = StaffManager.instance.GetStaffDataList(StaffInfo.StaffType.Hall).RandomList();

        StaffData staffData = null;
        foreach (var staff in randomList)
        {
            if (staff == null) continue;
            staffData = staff;
            break;
        }

        data.TargetStaffData = staffData;

        if (data.TargetStaffData != null)
        {
            SetTransition(m_successLink);
        }
        else
        {
            SetTransition(m_fallLink);
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
        // ターゲットにするスタッフを探す
        FindStaff();
    }

    // Use this for exit state
    public override void OnStateEnd()
    {
    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
    }

    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
    }
}
