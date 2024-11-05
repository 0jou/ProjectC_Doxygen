using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using OrderFoodInfo;

[AddComponentMenu("")]
public class CheckTargetOrderFoodStateStaff : BaseStaffStateBehaviour
{

    // 制作者　田内
    // ターゲットの料理の状態で進むステートリンクを変更する

    //======================================================
    // ステートリンク

    [System.Serializable]
    private class StaffStateLink
    {

        public OrderFoodState State = OrderFoodState.WaitChefStaff;

        public StateLink StateLink = null;

    }


    [SerializeField]
    private List<StaffStateLink> m_linkList = new();

    // 失敗した場合・当てはまらなかった場合のリンク
    [SerializeField]
    private StateLink m_failLink = null;

    //============================================
    //              
    //============================================

    private void Transition()
    {
        var data = GetStaffData();
        if (data == null || data.TargetOrderFoodData == null) return;


        foreach (var link in m_linkList)
        {
            if (link.State == data.TargetOrderFoodData.CurrentOrderFoodState)
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
