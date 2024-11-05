using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

using StaffInfo;

[AddBehaviourMenu("Staff/HallStaff/DecoCheckStateHallStaff")]
[AddComponentMenu("")]
public class DecoCheckStateHallStaff : BaseStaffDecorator
{
	// 制作者　田内
	// スタッフのステートで判別するデコレーター

	//=============================================================
	// ステート

	[Header("一致するか確認するステート")]
	[SerializeField]
	private List<HallStaffState> m_hallStaffStateList = new();


	//=============================================================
	//					実行処理
	//=============================================================

	protected override bool OnConditionCheck() 
	{
		var data = GetStaffData();
		if (data == null) return false;

		foreach (var state in m_hallStaffStateList)
		{
			// ステートが一致すれば
			if (data.CurrentHallStaffState == state)
			{
				return true;
			}
		}

		// 不一致であれば
		return false;
	}
}
