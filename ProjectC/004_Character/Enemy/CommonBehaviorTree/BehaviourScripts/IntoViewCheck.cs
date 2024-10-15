using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

[AddComponentMenu("")]
public class IntoViewCheck : Decorator
{
    [SerializeField] private FlexibleTransform m_myTransform;
    [SerializeField] private FlexibleTransform m_target;
    [SerializeField] private FlexibleInt m_viewAngle;

    protected override void OnAwake()
    {
    }

    protected override void OnStart()
    {
    }

    protected override bool OnConditionCheck()
    {
        return Vector3.Angle(
            m_myTransform.value.forward,
            m_target.value.position - m_myTransform.value.position
            ) <= m_viewAngle.value * 0.5f;
    }

    protected override void OnEnd()
    {
    }
}
