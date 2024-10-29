using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

[AddComponentMenu("")]
public class NullCheck : Decorator {

	[SerializeField] FlexibleTransform target;

	protected override void OnAwake() {
	}

	protected override void OnStart() {
	}

	protected override bool OnConditionCheck() {
		if(target.value)
		{
			return true;
		}
		return false;
		//return target.value;
	}

	protected override void OnEnd() {
	}
}
