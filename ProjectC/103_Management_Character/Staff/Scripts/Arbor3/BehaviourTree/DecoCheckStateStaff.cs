using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

using StaffStateInfo;

[AddComponentMenu("")]
public class DecoCheckStateStaff : BaseStaffDecorator
{
	// 制作者　田内
	// スタッフのステートで判別するデコレーター

	//=============================================================
	// ステート

	[Header("一致するか確認するステート")]
	[SerializeField]
	private List<StaffState> m_staffStateList = new();

	//=============================================================

	protected override void OnAwake() {
	}

	protected override void OnStart() {
	}

	protected override bool OnConditionCheck() 
	{
		var data = GetStaffData();
		if (data == null) return false;

		foreach (var state in m_staffStateList)
		{
			// ステートが一致すれば
			if (data.CurrentStaffState == state)
			{
				return true;
			}
		}

		// 不一致であれば
		return false;
	}

	protected override void OnEnd() {
	}
}
