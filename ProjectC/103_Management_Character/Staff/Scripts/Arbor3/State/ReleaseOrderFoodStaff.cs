using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("Staff/ReleaseOrderFoodStaff")]
public class ReleaseOrderFoodStaff :BaseStaffStateBehaviour
{
	// ターゲットの料理を手放す
	// 制作者　田内

	void ReleaseOrderFood()
    {
		var data = GetStaffData();
		if (data == null) return;

		data.TargetOrderFoodData = null;

    }


	// Use this for initialization
	void Start () {
	
	}

	// Use this for awake state
	public override void OnStateAwake() {
	}

	// Use this for enter state
	public override void OnStateBegin() 
	{
		ReleaseOrderFood();
	}

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// OnStateUpdate is called once per frame
	public override void OnStateUpdate() {
	}

	// OnStateLateUpdate is called once per frame, after Update has finished.
	public override void OnStateLateUpdate() {
	}
}
