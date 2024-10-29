using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    /// <summary>
    /// スタッフを追加する
    /// </summary>
    public void AddStaffrData(StaffData _data)
    {
        m_staffDataList.Add(_data);
    }


    // 不必要なデータ取り除く
    private void RemoveStaffData()
    {
        m_staffDataList.RemoveAll(_ => _ == null);
    }


}
