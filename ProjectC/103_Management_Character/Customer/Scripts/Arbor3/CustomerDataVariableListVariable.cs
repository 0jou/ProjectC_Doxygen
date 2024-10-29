using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[System.Serializable]
public class FlexibleListCustomerDataVariable : FlexibleList<CustomerDataVariable>
{
	public FlexibleListCustomerDataVariable()
	{
	}

	public FlexibleListCustomerDataVariable(IList<CustomerDataVariable> value) : base(value)
	{
	}

	public FlexibleListCustomerDataVariable(AnyParameterReference parameter) : base(parameter)
	{
	}

	public FlexibleListCustomerDataVariable(InputSlotAny slot) : base(slot)
	{
	}
}

[System.Serializable]
public class InputSlotListCustomerDataVariable : InputSlot<IList<CustomerDataVariable>>
{
}

[System.Serializable]
public class OutputSlotListCustomerDataVariable : OutputSlot<IList<CustomerDataVariable>>
{
}

[AddComponentMenu("")]
public class CustomerDataVariableListVariable : VariableList<CustomerDataVariable>
{
}
