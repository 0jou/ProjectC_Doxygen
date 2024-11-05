using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
[AddBehaviourMenu("Vector3/CalcTransformPos")]
[BehaviourTitle("CalcTransformPos")]
public class CalcTransformPos : Calculator {
	// Use this for calculate
	[SerializeField] private FlexibleTransform m_transform;

	[SerializeField] private OutputSlotVector3 m_outPos;

	public override void OnCalculate() {
		if(m_transform.value==null)
		{
			m_outPos.SetValue(Vector3.zero);
			return;
		}
        m_outPos.SetValue(m_transform.value.position);
	}
}
