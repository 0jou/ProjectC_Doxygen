using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using StaffInfo;

[AddComponentMenu("")]
public class CheckStunStaff : BaseStaffStateBehaviour
{
	// 制作者 田内
	// スタン状態か確認する

	//=======================================
	// ステートリンク

	[SerializeField]
	private StateLink m_stateLink = null;


	//=======================================

	// スタン状態か確認
	private void Check()
    {
		var data = GetStaffData();
		if (data == null) return;

		if(data.CurrentHallStaffState==HallStaffState.Stun)
        {
			SetTransition(m_stateLink);
        }
    }



	// Use this for initialization
	void Start () {
	
	}

	// Use this for awake state
	public override void OnStateAwake() {
	}

	// Use this for enter state
	public override void OnStateBegin() {
	}

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// OnStateUpdate is called once per frame
	public override void OnStateUpdate()
	{
		Check();
	}

	// OnStateLateUpdate is called once per frame, after Update has finished.
	public override void OnStateLateUpdate() {
	}
}
