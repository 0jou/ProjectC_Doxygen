using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[System.Serializable]
public class FlexibleListSearchType : FlexibleList<SearchType>
{
	public FlexibleListSearchType()
	{
	}

	public FlexibleListSearchType(IList<SearchType> value) : base(value)
	{
	}

	public FlexibleListSearchType(AnyParameterReference parameter) : base(parameter)
	{
	}

	public FlexibleListSearchType(InputSlotAny slot) : base(slot)
	{
	}
}

[System.Serializable]
public class InputSlotListSearchType : InputSlot<IList<SearchType>>
{
}

[System.Serializable]
public class OutputSlotListSearchType : OutputSlot<IList<SearchType>>
{
}

[AddComponentMenu("")]
public class SearchTypeListVariable : VariableList<SearchType>
{
}
