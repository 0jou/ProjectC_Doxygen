using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSetManager : BaseManager<TableSetManager>
{
    // テーブルセットを管理・操作するコントローラー
    // 制作者　田内

    [Header("テーブルセットのリスト")]
    [SerializeField]
    private List<TableSetData> m_tableSetList = new();

    //=============================================
    //              実行処理
    //=============================================


    #region メソッド説明
    /// <summary>
    /// ランダムで空席を取得する
    /// </summary>
    /// <returns> 空席のTableSetData</returns>
    #endregion
    public TableSetData GetVacantTableSet()
    {
        var list = m_tableSetList.RandomList();

        foreach (var table in list)
        {
            if (table == null || table.SitObject != null) continue;
            return table;
        }

        return null;
    }

}
