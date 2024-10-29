using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class GetDisappearTime : Calculator {
    // Use this for calculate

    //PlayerSkillParameterから消失時間を取得する（山本）

    [SerializeField] private FlexibleComponent m_component;
    [SerializeField] private OutputSlotFloat m_outputSlot = new OutputSlotFloat();


    public override void OnCalculate() 
    {
        var skillsParameters = m_component.value as PlayerSkillsParameters;

        if(skillsParameters)
        {
            m_outputSlot.SetValue(skillsParameters.DisappearTime);
        }

	}
}
