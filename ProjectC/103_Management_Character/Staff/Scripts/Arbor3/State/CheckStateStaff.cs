using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using StaffStateInfo;

[AddComponentMenu("")]
public class CheckStateStaff : BaseStaffStateBehaviour
{


    // 制作者　田内
    // ステートを確認する

    [System.Serializable]
    private class StaffStateLink
    {

        public StaffState State = StaffState.Default;

        public StateLink StateLink = null;

    }


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
            if (link.State == data.CurrentStaffState)
            {
                SetTransition(link.StateLink);
                return;
            }
        }

        SetTransition(m_failLink);

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
      
    }

    // Use this for exit state
    public override void OnStateEnd()
    {
    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        Transition();
    }

    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
    }
}
