using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSetManager : BaseManager<TableSetManager>
{
    // テーブルセットを管理・操作するコントローラー
    // 制作者　田内


    //============================================================

    [Header("テーブルセットのリスト")]
    [SerializeField]
    private List<TableSetData> m_tableSetList = new();

    //============================================================

    #region メソッド説明
    /// <summary>
    /// ランダムで空席を取得する
    /// </summary>
    /// <returns> 空席のTableSetData</returns>
#endregion
    public TableSetData GetVacantTableSet()
    {
        var list = m_tableSetList;
       // Shuffle(list);

        foreach (var table in list)
        {
            // 座られていれば
            if (table.SitObject != null) continue;

            return table;

        }

        return null;
    }




    // シャッフルする
    private void Shuffle<T>(List<T> _list)
    {
        for (int i = _list.Count - 1; i > 0; i--)
        {
            var j = Random.Range(0, i + 1);
            var temp = _list[i];
            _list[i] = _list[j];
            _list[j] = temp;
        }
    }


}
