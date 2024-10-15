using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[System.Serializable]
public class FlexibleListGangsterDataVariable : FlexibleList<GangsterDataVariable>
{
	public FlexibleListGangsterDataVariable()
	{
	}

	public FlexibleListGangsterDataVariable(IList<GangsterDataVariable> value) : base(value)
	{
	}

	public FlexibleListGangsterDataVariable(AnyParameterReference parameter) : base(parameter)
	{
	}

	public FlexibleListGangsterDataVariable(InputSlotAny slot) : base(slot)
	{
	}
}

[System.Serializable]
public class InputSlotListGangsterDataVariable : InputSlot<IList<GangsterDataVariable>>
{
}

[System.Serializable]
public class OutputSlotListGangsterDataVariable : OutputSlot<IList<GangsterDataVariable>>
{
}

[AddComponentMenu("")]
public class GangsterDataVariableListVariable : VariableList<GangsterDataVariable>
{
}
