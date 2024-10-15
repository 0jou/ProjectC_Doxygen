using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class BaseGangsterStateCalculator : Calculator 
{
	// 制作者　田内

	//======================================
	// ヤンキー情報

	[Header("ヤンキー情報")]
	[SerializeField]
	protected FlexibleGangsterDataVariable m_flexibleGangsterDataVariable = null;


	//=========================================
	// 出力

	[Header("出力:座標")]
	[SerializeField]
	protected OutputSlotVector3 m_outputPos = new();


	#region メソッド説明
	/// <summary>
	/// ヤンキー情報が存在するかのチェック
	/// </summary>
	#endregion
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


	// 一番親の情報を取得する
	protected GameObject GetRootGameObject()
	{
		Transform currentTransform = transform;

		// ルートオブジェクトに到達するまで親を辿る
		while (currentTransform.parent != null)
		{
			currentTransform = currentTransform.parent;
		}

		return currentTransform.gameObject;
	}


}
