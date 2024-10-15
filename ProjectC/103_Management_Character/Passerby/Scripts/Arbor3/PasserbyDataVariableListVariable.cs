using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[System.Serializable]
public class FlexibleListPasserbyDataVariable : FlexibleList<PasserbyDataVariable>
{
	public FlexibleListPasserbyDataVariable()
	{
	}

	public FlexibleListPasserbyDataVariable(IList<PasserbyDataVariable> value) : base(value)
	{
	}

	public FlexibleListPasserbyDataVariable(AnyParameterReference parameter) : base(parameter)
	{
	}

	public FlexibleListPasserbyDataVariable(InputSlotAny slot) : base(slot)
	{
	}
}

[System.Serializable]
public class InputSlotListPasserbyDataVariable : InputSlot<IList<PasserbyDataVariable>>
{
}

[System.Serializable]
public class OutputSlotListPasserbyDataVariable : OutputSlot<IList<PasserbyDataVariable>>
{
}

[AddComponentMenu("")]
public class PasserbyDataVariableListVariable : VariableList<PasserbyDataVariable>
{
}
