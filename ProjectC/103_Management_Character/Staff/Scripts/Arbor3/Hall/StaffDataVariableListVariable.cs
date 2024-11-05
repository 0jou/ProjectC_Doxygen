using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[System.Serializable]
public class FlexibleListStaffDataVariable : FlexibleList<StaffDataVariable>
{
	public FlexibleListStaffDataVariable()
	{
	}

	public FlexibleListStaffDataVariable(IList<StaffDataVariable> value) : base(value)
	{
	}

	public FlexibleListStaffDataVariable(AnyParameterReference parameter) : base(parameter)
	{
	}

	public FlexibleListStaffDataVariable(InputSlotAny slot) : base(slot)
	{
	}
}

[System.Serializable]
public class InputSlotListStaffDataVariable : InputSlot<IList<StaffDataVariable>>
{
}

[System.Serializable]
public class OutputSlotListStaffDataVariable : OutputSlot<IList<StaffDataVariable>>
{
}

[AddComponentMenu("")]
public class StaffDataVariableListVariable : VariableList<StaffDataVariable>
{
}
