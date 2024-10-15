using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class CreateCustometCanvasCustomer : BaseCustomerStateBehaviour
{
    // 制作者 田内
    // 客のUIキャンバスを作成する

    [Header("客キャンバス")]
    [SerializeField]
    private CustomerCanvas m_customerCanvas = null;

    //================================================
    //              実行処理
    //================================================

    private void Create()
    {
        var data = GetCustomerData();
        if (data == null) return;

        if(m_customerCanvas==null)
        {
            Debug.LogError("CustometCanvasがシリアライズされていません");
            return;
        }

        var canvas = Instantiate<CustomerCanvas>(m_customerCanvas, GetRootGameObject().transform);
        canvas.TargetCustomerData = data;
    }


    // Use this for enter state
    public override void OnStateBegin()
    {
        Create();
    }

}
