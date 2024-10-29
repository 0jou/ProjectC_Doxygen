using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

[AddComponentMenu("")]
public class BaseCustomerDecorator : Decorator
{

    // 制作者　田内

    //======================================
    // 客情報

    [Header("客情報")]
    [SerializeField]
    protected FlexibleCustomerDataVariable m_flexibleCustomerDataVariable = null;

    //======================================

    protected CustomerData GetCustomerData()
    {

        // 客情報が存在するか確認

        if (m_flexibleCustomerDataVariable == null)
        {
            Debug.LogError("客情報がシリアライズされていません");
            return null;
        }

        if (m_flexibleCustomerDataVariable.value.CustomerData == null)
        {
            Debug.LogError(this.name + "客情報が存在しません");
            return null;
        }

        // 存在する
        return m_flexibleCustomerDataVariable.value.CustomerData;

    }

}
