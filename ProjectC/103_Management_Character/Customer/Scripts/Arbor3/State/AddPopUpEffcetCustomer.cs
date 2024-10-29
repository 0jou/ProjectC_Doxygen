using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class AddPopUpEffcetCustomer : BaseCustomerStateBehaviour
{

    [SerializeField] private string m_objectKey = null;
    // Use this for initialization
    void Start()
    {
        var obj = GetRootGameObject();
        if(!obj)
        {
            return;
        }

        var popup = obj.AddComponent<PopupEmotion>();
        if(!popup)
        {
            return;
        }

        if(m_objectKey!=null)
        popup.Popup(m_objectKey);

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
    }

    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
    }
}
