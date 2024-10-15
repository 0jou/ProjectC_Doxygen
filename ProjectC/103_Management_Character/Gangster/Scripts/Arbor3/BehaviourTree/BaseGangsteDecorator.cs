using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

[AddComponentMenu("")]
public class BaseGangsteDecorator : Decorator 
{

	// 制作者　田内

	//======================================
	// ヤンキー情報

	[Header("ヤンキー情報")]
	[SerializeField]
	protected FlexibleGangsterDataVariable m_flexibleGangsterDataVariable = null;

	//======================================

	protected GangsterData GetGangsterData()
	{

		// 客情報が存在するか確認

		if (m_flexibleGangsterDataVariable == null)
		{
			Debug.LogError("ヤンキー情報がシリアライズされていません");
			return null;
		}

		if (m_flexibleGangsterDataVariable.value.GangsterData == null)
		{
			Debug.LogError("ヤンキー情報が存在しません");
			return null;
		}

		// 存在する
		return m_flexibleGangsterDataVariable.value.GangsterData;

	}

}
