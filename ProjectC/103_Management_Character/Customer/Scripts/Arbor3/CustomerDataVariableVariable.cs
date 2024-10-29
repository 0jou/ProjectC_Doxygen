using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[System.Serializable]
public class CustomerDataVariable
{
    // Arbor3で客情報を扱いやすすくする用
    // 制作者　田内

    [Header("客情報")]
    [SerializeField]
    private CustomerData m_customerData = null;

    public CustomerData CustomerData { get { return m_customerData; } }

}

[System.Serializable]
public class FlexibleCustomerDataVariable : FlexibleField<CustomerDataVariable>
{
    public FlexibleCustomerDataVariable()
    {
    }

    public FlexibleCustomerDataVariable(CustomerDataVariable value) : base(value)
    {
    }

    public FlexibleCustomerDataVariable(AnyParameterReference parameter) : base(parameter)
    {
    }

    public FlexibleCustomerDataVariable(InputSlotAny slot) : base(slot)
    {
    }

    public static explicit operator CustomerDataVariable(FlexibleCustomerDataVariable flexible)
    {
        return flexible.value;
    }

    public static explicit operator FlexibleCustomerDataVariable(CustomerDataVariable value)
    {
        return new FlexibleCustomerDataVariable(value);
    }
}

[System.Serializable]
public class InputSlotCustomerDataVariable : InputSlot<CustomerDataVariable>
{
}

[System.Serializable]
public class OutputSlotCustomerDataVariable : OutputSlot<CustomerDataVariable>
{
}

[AddComponentMenu("")]
public class CustomerDataVariableVariable : Variable<CustomerDataVariable>
{
}
