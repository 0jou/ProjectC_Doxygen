using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
[AddBehaviourMenu("Vector3/CalcMyTransformPos")]
[BehaviourTitle("CalcMyTransformPos")]
public class CalcMyTransformPos : Calculator {
	// Use this for calculate

	[SerializeField] private OutputSlotVector3 m_outPos;

	public override void OnCalculate() {
        m_outPos.SetValue(transform.position);
	}
}
