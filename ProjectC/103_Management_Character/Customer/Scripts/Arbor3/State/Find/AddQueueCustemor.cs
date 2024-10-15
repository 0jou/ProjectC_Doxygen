using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/AddQueueCustomer")]
[AddComponentMenu("")]
public class AddQueueCustemor : BaseCustomerStateBehaviour
{
    // 待ち列に自分を追加する
    // 待ち列に自身を追加できるか確認する
    // 制作者　田内

    [SerializeField]
    private StateLink m_successLink = null;

    [SerializeField]
    private StateLink m_failLink = null;

    //========================================================

    private void AddQueue()
    {

        var data = GetCustomerData();
        if (data == null) return;

        var controller = QueueManager.instance;
        if (controller == null) return;

        // 待ち列に追加
        data.QueueData = controller.AddQueueObject(GetRootGameObject());

        if (data.QueueData != null)
        {
            SetTransition(m_successLink);
        }
        else
        {
            SetTransition(m_failLink);
        }

    }




    //==========================================================

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
        AddQueue();
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
