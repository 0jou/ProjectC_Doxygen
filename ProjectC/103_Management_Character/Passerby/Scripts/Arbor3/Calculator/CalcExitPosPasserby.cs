using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Passerby/CalcExitPosPasserby")]
[AddComponentMenu("")]
public class CalcExitPosPasserby : BasePasserbyCalculator 
{

	// 制作者　田内

	// Use this for calculate
	public override void OnCalculate() 
	{

		var data = GetPasserbyData();
		if (data == null) return;

		// 出口座標に向かう
		var pos = data.ExitPos;
		m_outputPos.SetValue(pos);
	
	}
}
