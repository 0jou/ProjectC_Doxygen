using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[System.Serializable]
public class FlexibleListChefDataVariable : FlexibleList<ChefDataVariable>
{
	public FlexibleListChefDataVariable()
	{
	}

	public FlexibleListChefDataVariable(IList<ChefDataVariable> value) : base(value)
	{
	}

	public FlexibleListChefDataVariable(AnyParameterReference parameter) : base(parameter)
	{
	}

	public FlexibleListChefDataVariable(InputSlotAny slot) : base(slot)
	{
	}
}

[System.Serializable]
public class InputSlotListChefDataVariable : InputSlot<IList<ChefDataVariable>>
{
}

[System.Serializable]
public class OutputSlotListChefDataVariable : OutputSlot<IList<ChefDataVariable>>
{
}

[AddComponentMenu("")]
public class ChefDataVariableListVariable : VariableList<ChefDataVariable>
{
}
