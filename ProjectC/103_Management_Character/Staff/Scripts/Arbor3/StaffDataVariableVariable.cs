using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[System.Serializable]
public class StaffDataVariable
{
	// Arbor3で客情報を扱いやすすくする用
	// 制作者　田内

	[Header("スタッフ情報")]
	[SerializeField]
	private StaffData m_staffData = null;

	public StaffData StaffData { get { return m_staffData; } }
}

[System.Serializable]
public class FlexibleStaffDataVariable : FlexibleField<StaffDataVariable>
{
	public FlexibleStaffDataVariable()
	{
	}

	public FlexibleStaffDataVariable(StaffDataVariable value) : base(value)
	{
	}

	public FlexibleStaffDataVariable(AnyParameterReference parameter) : base(parameter)
	{
	}

	public FlexibleStaffDataVariable(InputSlotAny slot) : base(slot)
	{
	}

	public static explicit operator StaffDataVariable(FlexibleStaffDataVariable flexible)
	{
		return flexible.value;
	}

	public static explicit operator FlexibleStaffDataVariable(StaffDataVariable value)
	{
		return new FlexibleStaffDataVariable(value);
	}
}

[System.Serializable]
public class InputSlotStaffDataVariable : InputSlot<StaffDataVariable>
{
}

[System.Serializable]
public class OutputSlotStaffDataVariable : OutputSlot<StaffDataVariable>
{
}

[AddComponentMenu("")]
public class StaffDataVariableVariable : Variable<StaffDataVariable>
{
}
