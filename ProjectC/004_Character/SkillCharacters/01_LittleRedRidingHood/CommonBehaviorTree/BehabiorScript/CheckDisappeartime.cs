using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

[AddComponentMenu("")]
public class CheckDisappeartime : Decorator
{

    [SerializeField]
    private FlexibleComponent m_commponent = new FlexibleComponent();

    private bool m_isDisappear = false;

    protected override void OnAwake()
    {
    }

    protected override void OnStart()
    {
    }

    protected override bool OnConditionCheck()
    {
        m_isDisappear = false;

        var skillParameter = m_commponent.value as PlayerSkillsParameters;

        if(skillParameter.MinusStayStorySkillTime())
        {
            m_isDisappear = true;
        }

        return m_isDisappear;
    }

    protected override void OnEnd()
    {
    }
}
