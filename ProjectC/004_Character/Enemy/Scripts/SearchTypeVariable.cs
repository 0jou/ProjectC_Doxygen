using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[System.Serializable]
public class SearchType
{
	public SearchTargets searchTargets;
}

[System.Serializable]
public class FlexibleSearchType : FlexibleField<SearchType>
{
	public FlexibleSearchType()
	{
	}

	public FlexibleSearchType(SearchType value) : base(value)
	{
	}

	public FlexibleSearchType(AnyParameterReference parameter) : base(parameter)
	{
	}

	public FlexibleSearchType(InputSlotAny slot) : base(slot)
	{
	}

	public static explicit operator SearchType(FlexibleSearchType flexible)
	{
		return flexible.value;
	}

	public static explicit operator FlexibleSearchType(SearchType value)
	{
		return new FlexibleSearchType(value);
	}
}

[System.Serializable]
public class InputSlotSearchType : InputSlot<SearchType>
{
}

[System.Serializable]
public class OutputSlotSearchType : OutputSlot<SearchType>
{
}

[AddComponentMenu("")]
public class SearchTypeVariable : Variable<SearchType>
{
}
