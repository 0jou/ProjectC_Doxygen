using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagementEventInfo;


[CreateAssetMenu(fileName = "ManagementEventDataBase", menuName = "ScriptableObjects/ManagementEvent/作成 ManagementEventDataBase")]
[System.Serializable]
public class ManagementEventDataBase : ScriptableObject
{
    // 制作者 田内
    // 経営イベントデータベース


    [Header("イベントデータリスト")]
    [SerializeField]
    private List<ManagementEventData> m_managementEventDataList = new();

    public List<ManagementEventData> ManagementEventDataList
    {
        get { return m_managementEventDataList; }
    }

    //=======================================================
    //                  実行処理
    //=======================================================

    /// <summary>
    /// 引数IDのイベントを取得
    /// </summary>
    public ManagementEventData GetManagementEventData(ManagementEventID _id)
    {
        foreach (var data in m_managementEventDataList)
        {
            if (data == null)
            {
                Debug.LogError("データがnullのものが存在します");
                continue;
            }

            if (data.EventID == _id)
            {
                return data;
            }
        }


        Debug.LogError("データが存在しませんでした");
        return null;

    }

}
