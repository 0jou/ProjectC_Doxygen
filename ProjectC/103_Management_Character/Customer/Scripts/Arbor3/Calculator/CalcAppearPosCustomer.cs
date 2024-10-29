using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/CalcAppearPosCustomer")]
[AddComponentMenu("")]
public class CalcAppearPosCustomer : BaseCustomerCalculator
{

	// 出口の座標を取得する
	// 制作者　田内

	

	// Use this for calculate
	public override void OnCalculate() 
	{

		var data = GetCustomerData();
		if (data == null) return;

		var pos = data.AppearPos;
		m_outputPos.SetValue(pos);

	}

}
