using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementDataManager : BaseManager<ManagementDataManager>
{
    // 制作者 田内
    // 経営に関する情報を管理するマネージャークラス


    // 稼いだ総額
    private uint m_totalEarnedMoney = 0;

    public uint TotalEarnedMoney
    {
        get { return m_totalEarnedMoney; }
    }

    //==================================================
    //                  処理
    //==================================================

    /// <summary>
    /// 総額を追加する
    /// </summary>
    public void AddTotalEarnedMoney(uint _value)
    {
        m_totalEarnedMoney += _value;
    }

}
