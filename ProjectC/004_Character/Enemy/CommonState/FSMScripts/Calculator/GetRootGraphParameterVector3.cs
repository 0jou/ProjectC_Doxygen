using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using static UnityEngine.Rendering.DebugUI;
using Arbor.BehaviourTree.Actions;
using UnityEngine.AI;

// ルートグラフからしていのパラメータを取り出す（伊波）

[AddComponentMenu("")]
[AddBehaviourMenu("Vector3/GetRootGraphParameterVector3")]
[BehaviourTitle("GetRootGraphParameterVector3")]
public class GetRootGraphParameterVector3 : Calculator
{
    [SerializeField] private FlexibleString m_getParameterName;
    [SerializeField] private OutputSlotVector3 m_result = new OutputSlotVector3();
    public override void OnCalculate()
    {
        if (m_getParameterName.value == null)
        {
            return;
        }
        if (nodeGraph.rootGraph.parameterContainer.TryGetVector3(m_getParameterName.value, out Vector3 vec3))
        {
            m_result.SetValue(vec3);
        }
    }
}