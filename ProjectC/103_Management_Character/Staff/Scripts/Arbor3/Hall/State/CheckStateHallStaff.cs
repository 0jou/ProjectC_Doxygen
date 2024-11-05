using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using StaffInfo;

[AddBehaviourMenu("Staff/HallStaff/CheckStateHallStaff")]
[AddComponentMenu("")]
public class CheckStateHallStaff : BaseStaffStateBehaviour
{
    // 制作者　田内
    // ステートを確認する

    [System.Serializable]
    private class StaffStateLink
    {

        public HallStaffState State = HallStaffState.Default;

        public StateLink StateLink = null;

    }


    //============================================
    //              実行処理
    //============================================


    [SerializeField]
    private List<StaffStateLink> m_linkList = new();

    // 失敗した場合・当てはまらなかった場合のリンク
    [SerializeField]
    private StateLink m_failLink = null;


    private void Transition()
    {
        var data = GetStaffData();
        if (data == null) return;


        foreach (var link in m_linkList)
        {
            if (link.State == data.CurrentHallStaffState)
            {
                SetTransition(link.StateLink);
                return;
            }
        }

        SetTransition(m_failLink);

    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        Transition();
    }
}
