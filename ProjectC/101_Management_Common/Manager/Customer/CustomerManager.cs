using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class CustomerManager : BaseManager<CustomerManager>
{
    // 制作者 田内
    // 客の情報を管理する

    private List<CustomerData> m_customerDataList = new();

    public List<CustomerData> CustomerDataList
    {
        get { return m_customerDataList; }
    }


    //==============================================
    //                  実行処理
    //==============================================

    private void Update()
    {
        RemoveCustomerData();
    }


    /// <summary>
    /// 客を追加する
    /// </summary>
    public void AddCustomerData(CustomerData _data)
    {
        m_customerDataList.Add(_data);
    }


    // 不必要なデータ取り除く
    private void RemoveCustomerData()
    {
        m_customerDataList.RemoveAll(_ => _ == null);
    }


}
