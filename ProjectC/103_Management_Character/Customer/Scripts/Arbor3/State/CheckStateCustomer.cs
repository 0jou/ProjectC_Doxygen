/*!
 * @file 	CheckStateCustomer.cs
 * @brief 現在のステートをもとに指定のステートに遷移する
 * @auther 田内
 * @attention 指定していないステートの状態だと無限ループになる可能性あり 上甲
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using CustomerStateInfo;

/// <summary>
/// @brief 現在のステートをもとに指定のステートに遷移する
/// </summary>
[AddComponentMenu("")]
public class CheckStateCustomer : BaseCustomerStateBehaviour
{

    //! @brif ステートと遷移先のリンク
    [System.Serializable]
    private class CustomerStateLink
    {

        public CustomerState State = CustomerState.Normal;

        public StateLink StateLink = null;

    }

    [SerializeField]
    private List<CustomerStateLink> m_linkList = new();


    private void Transition()
    {
        var data = GetCustomerData();
        if (data == null) return;


        foreach (var link in m_linkList)
        {
            if (link.State == data.CurrentCustomerState)
            {
                SetTransition(link.StateLink);
                return;
            }
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
