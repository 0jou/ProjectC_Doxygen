using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StaffInfo;

public class StaffManager : BaseManager<StaffManager>
{
    // 制作者 田内
    // スタッフの情報を管理する

    private List<StaffData> m_staffDataList = new();

    public List<StaffData> StaffDataList
    {
        get { return m_staffDataList; }
    }

    //==============================================
    //                  実行処理
    //==============================================

    private void Update()
    {
        RemoveStaffData();
    }


    public List<StaffData> GetStaffDataList(StaffType _type)
    {
        List<StaffData> list = new();

        foreach (var data in m_staffDataList)
        {
            if (data == null) continue;
            if(data.StaffType==_type)
            {
                list.Add(data);
            }
        }

        return list;
    }


    /// <summary>
    /// スタッフを追加する
    /// </summary>
    public void AddStaffData(StaffData _data)
    {
        m_staffDataList.Add(_data);
    }


    // 不必要なデータ取り除く
    private void RemoveStaffData()
    {
        m_staffDataList.RemoveAll(_ => _ == null);
    }


}
