using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[System.Serializable]
public class FlexibleListChaseParameters : FlexibleList<ChaseParameters>
{
	public FlexibleListChaseParameters()
	{
	}

	public FlexibleListChaseParameters(IList<ChaseParameters> value) : base(value)
	{
	}

	public FlexibleListChaseParameters(AnyParameterReference parameter) : base(parameter)
	{
	}

	public FlexibleListChaseParameters(InputSlotAny slot) : base(slot)
	{
	}
}

[System.Serializable]
public class InputSlotListChaseParameters : InputSlot<IList<ChaseParameters>>
{
}

[System.Serializable]
public class OutputSlotListChaseParameters : OutputSlot<IList<ChaseParameters>>
{
}

[AddComponentMenu("")]
public class ChaseParametersListVariable : VariableList<ChaseParameters>
{
}
