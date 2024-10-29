/*!
 * @file RemoveChairCustomer.cs
 * @brief 椅子から客の紐づけを外す
 * @author 田内
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;


/// <summary>
/// @brief 椅子から客の紐づけを外す
/// </summary>
[AddBehaviourMenu("Customer/LeaveChairCustomer")]
[AddComponentMenu("")]
public class RemoveChairCustomer : BaseCustomerStateBehaviour
{

    //! @brief 椅子情報から客の紐づけを外す
    private void Remove()
    {
		var data = GetCustomerData();
		if (data == null) return;

		var table = data.TargetTableSetData;
		if (table == null) return;

		// 座っているオブジェクトを取り除く
		table.SitObject = null;
    }


	//======================================================


	// Use this for initialization
	void Start ()
	{

	}

	// Use this for awake state
	public override void OnStateAwake() {
	}

	// Use this for enter state
	public override void OnStateBegin()
	{
		Remove();
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
