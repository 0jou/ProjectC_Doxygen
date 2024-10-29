using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arbor;

public class AddEmptyDishProbabilityEvent : AddProbabilityEvent
{

    [Header("設置座標")]
    [SerializeField] private FlexibleVector3 m_tablePosition = new FlexibleVector3();

    protected override void EventInitializeProcess()
    {
        if (m_createManageentEvent == null) return;

        m_createManageentEvent.gameObject.transform.position = m_tablePosition.value;

        if (m_createManageentEvent is not GenerateEmptyDishEvent)
        {
            Debug.LogError("GenerateEmptyDishEventにキャストできません");
            return;
        }

        //var castEmptyDishEvent = m_createManageentEvent as GenerateEmptyDishEvent;
    }



}
