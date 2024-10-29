using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[System.Serializable]
public class GangsterDataVariable
{
	// Arbor3でヤンキー情報を扱いやすすくする用
	// 制作者　田内

	[Header("ヤンキー情報")]
	[SerializeField]
	private GangsterData m_gangsterData = null;

	public GangsterData GangsterData { get { return m_gangsterData; } }
}

[System.Serializable]
public class FlexibleGangsterDataVariable : FlexibleField<GangsterDataVariable>
{
	public FlexibleGangsterDataVariable()
	{
	}

	public FlexibleGangsterDataVariable(GangsterDataVariable value) : base(value)
	{
	}

	public FlexibleGangsterDataVariable(AnyParameterReference parameter) : base(parameter)
	{
	}

	public FlexibleGangsterDataVariable(InputSlotAny slot) : base(slot)
	{
	}

	public static explicit operator GangsterDataVariable(FlexibleGangsterDataVariable flexible)
	{
		return flexible.value;
	}

	public static explicit operator FlexibleGangsterDataVariable(GangsterDataVariable value)
	{
		return new FlexibleGangsterDataVariable(value);
	}
}

[System.Serializable]
public class InputSlotGangsterDataVariable : InputSlot<GangsterDataVariable>
{
}

[System.Serializable]
public class OutputSlotGangsterDataVariable : OutputSlot<GangsterDataVariable>
{
}

[AddComponentMenu("")]
public class GangsterDataVariableVariable : Variable<GangsterDataVariable>
{
}
