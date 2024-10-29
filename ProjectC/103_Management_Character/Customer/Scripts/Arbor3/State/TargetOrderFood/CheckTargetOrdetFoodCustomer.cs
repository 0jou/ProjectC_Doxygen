using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class CheckTargetOrdetFoodCustomer : BaseCustomerStateBehaviour
{
	// 制作者　田内
	// ターゲットの料理が存在するか確認


	//=============================
	// ステートリンク

	[SerializeField]
	private StateLink m_stateLink = null;

	// OnStateUpdate is called once per frame
	public override void OnStateUpdate() 
	{
		var data = GetCustomerData();
		if (data == null) return;

		if(data.TargetOrderFoodData==null)
        {
			SetTransition(m_stateLink);
        }

	}
}
