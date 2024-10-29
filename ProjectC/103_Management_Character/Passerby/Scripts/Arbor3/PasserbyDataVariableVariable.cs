using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[System.Serializable]
public class PasserbyDataVariable
{
	// Arbor3で通行人情報を扱いやすすくする用
	// 制作者　田内

	[Header("通行人情報")]
	[SerializeField]
	private PasserbyData m_passerbyData = null;

	public PasserbyData PasserbyData { get { return m_passerbyData; } }
}

[System.Serializable]
public class FlexiblePasserbyDataVariable : FlexibleField<PasserbyDataVariable>
{
	public FlexiblePasserbyDataVariable()
	{
	}

	public FlexiblePasserbyDataVariable(PasserbyDataVariable value) : base(value)
	{
	}

	public FlexiblePasserbyDataVariable(AnyParameterReference parameter) : base(parameter)
	{
	}

	public FlexiblePasserbyDataVariable(InputSlotAny slot) : base(slot)
	{
	}

	public static explicit operator PasserbyDataVariable(FlexiblePasserbyDataVariable flexible)
	{
		return flexible.value;
	}

	public static explicit operator FlexiblePasserbyDataVariable(PasserbyDataVariable value)
	{
		return new FlexiblePasserbyDataVariable(value);
	}
}

[System.Serializable]
public class InputSlotPasserbyDataVariable : InputSlot<PasserbyDataVariable>
{
}

[System.Serializable]
public class OutputSlotPasserbyDataVariable : OutputSlot<PasserbyDataVariable>
{
}

[AddComponentMenu("")]
public class PasserbyDataVariableVariable : Variable<PasserbyDataVariable>
{
}
