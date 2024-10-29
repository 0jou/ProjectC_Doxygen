using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class BaseCustomerStateBehaviour : StateBehaviour
{

    // 制作者　田内

    //===========================================================

    [Header("客情報")]
    [SerializeField]
    protected FlexibleCustomerDataVariable m_flexibleCustomerDataVariable = null;


    //================================================================================


    #region メソッド説明
    /// <summary>
    /// 客情報が存在するかのチェック
    /// </summary>
    #endregion
    protected CustomerData GetCustomerData()
    {

        // 客情報が存在するか確認

        if (m_flexibleCustomerDataVariable == null)
        {
            Debug.LogError(this.name + "客情報がシリアライズされていません");
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


    protected void SetTransition(StateLink _link)
    {
        if (_link == null)
        {
            Debug.LogError("次のシーンがセットされていません");
            return;
        }

        Transition(_link);

    }

    // 一番親の情報を取得する
    protected GameObject GetRootGameObject()
    {
        Transform currentTransform = transform;

        // ルートオブジェクトに到達するまで親を辿る
        while (currentTransform.parent != null)
        {
            currentTransform = currentTransform.parent;
        }

        return currentTransform.gameObject;
    }

}
