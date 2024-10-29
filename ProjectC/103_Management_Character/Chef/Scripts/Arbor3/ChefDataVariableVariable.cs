using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[System.Serializable]
public class ChefDataVariable
{
	// Arbor3でシェフ情報を扱いやすすくする用
	// 制作者　田内

	[Header("シェフ情報")]
	[SerializeField]
	private ChefData m_chefData = null;

	public ChefData ChefData { get { return m_chefData; } }
}


[System.Serializable]
public class FlexibleChefDataVariable : FlexibleField<ChefDataVariable>
{
	public FlexibleChefDataVariable()
	{
	}

	public FlexibleChefDataVariable(ChefDataVariable value) : base(value)
	{
	}

	public FlexibleChefDataVariable(AnyParameterReference parameter) : base(parameter)
	{
	}

	public FlexibleChefDataVariable(InputSlotAny slot) : base(slot)
	{
	}

	public static explicit operator ChefDataVariable(FlexibleChefDataVariable flexible)
	{
		return flexible.value;
	}

	public static explicit operator FlexibleChefDataVariable(ChefDataVariable value)
	{
		return new FlexibleChefDataVariable(value);
	}
}

[System.Serializable]
public class InputSlotChefDataVariable : InputSlot<ChefDataVariable>
{
}

[System.Serializable]
public class OutputSlotChefDataVariable : OutputSlot<ChefDataVariable>
{
}

[AddComponentMenu("")]
public class ChefDataVariableVariable : Variable<ChefDataVariable>
{
}
