using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[System.Serializable]
public class FlexibleListAttackData : FlexibleList<AttackData>
{
    public FlexibleListAttackData()
    {
    }

    public FlexibleListAttackData(IList<AttackData> value) : base(value)
    {
    }

    public FlexibleListAttackData(AnyParameterReference parameter) : base(parameter)
    {
    }

    public FlexibleListAttackData(InputSlotAny slot) : base(slot)
    {
    }
}

[System.Serializable]
public class InputSlotListAttackData : InputSlot<IList<AttackData>>
{
}

[System.Serializable]
public class OutputSlotListAttackData : OutputSlot<IList<AttackData>>
{
}

[AddComponentMenu("")]
public class AttackDataListVariable : VariableList<AttackData>
{
}
