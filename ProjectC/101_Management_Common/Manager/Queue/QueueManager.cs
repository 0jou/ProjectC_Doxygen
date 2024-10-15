using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;


public class QueueData
{
    // 保持しているオブジェクト
    public GameObject Object = null;

    // データが更新されたかどうか  
    public bool IsUpdate = true;

    // 先頭から何番目に位置するか
    public int HeadNumber = 0;

    // 目的地
    public Vector3 Destination = new();

}

public class QueueManager : BaseManager<QueueManager>
{
    // 待ち行列を管理するコントローラー
    // 制作者　田内

    //===============================================================

    [Header("先頭にするポイント")]
    [SerializeField]
    private GameObject m_headPoint = null;

    // 行列の先頭ポイントを取得
    public GameObject HeadPoint
    {
        get
        {
            if (m_headPoint == null)
            {
                Debug.LogError("ポイントが存在しません");
                return null;
            }

            return m_headPoint;
        }
    }

    //===============================================================

    [Header("間隔")]
    [SerializeField]
    private float m_distance = 1.0f;

    //==============================================================

    [Header("最高待機人数")]
    [SerializeField]
    [Range(1, 20)]
    private int m_maxQueue = 5;

    //===============================================================
    // 待ち列のオブジェクトリスト

    private List<QueueData> m_queueDataList = new();

    //===============================================================
    // 待機列にGameObjectが存在するか

    public bool IsQueue()
    {
        // 待機列に存在していれば
        if (m_queueDataList.Count > 0)
        {
            return true;
        }

        return false;
    }


    // 待機列に入れるかどうか
    public bool IsInQueue()
    {
        if (m_queueDataList.Count >= m_maxQueue)
        {
            return false;
        }
        return true;
    }

    #region　メソッド説明
    /// <summary>
    /// 待ち列に引数オブジェクトを追加
    /// </summary>
    #endregion
    public QueueData AddQueueObject(GameObject _object)
    {
        if (_object == null) return null;
        if (!IsInQueue()) return null;

        QueueData data = new();

        // オブジェクトをセット
        data.Object = _object;

        // オブジェクトが削除されれば自動でリストから取り除く
        data.Object.OnDestroyAsObservable().Subscribe(_ =>
        {
            if (data == null) return;

            // 取り除く
            RemoveQueueObject(data);

        }
        ).AddTo(this);

        // 待ち列にオブジェクトを追加
        m_queueDataList.Add(data);

        // 更新する
        UpdateQueueData(data, m_queueDataList.Count - 1);


        return data;
    }

    #region　メソッド説明
    /// <summary>
    /// 待ち列リストから引数オブジェクトを取り除く
    /// </summary>
    #endregion
    public void RemoveQueueObject(QueueData _data)
    {
        if (_data == null) return;

        // 取り除く
        m_queueDataList.Remove(_data);

        // 更新する
        UpdateQueueDataList();
    }


    #region　メソッド説明
    /// <summary>
    /// 引数と一致するデータを取得する
    /// </summary>
    #endregion
    public QueueData GetQueueObject(GameObject _object)
    {
        foreach (var data in m_queueDataList)
        {
            if (data == null || data.Object == null) continue;

            if (data.Object == _object)
            {
                return data;
            }
        }

        return null;
    }


    public void UpdateQueueData(QueueData _data, int _number)
    {

        if (m_headPoint == null)
        {
            Debug.LogError("先端のポイントが存在しません");
            return;
        }


        var pos = m_headPoint.transform.position;
        var backword = -m_headPoint.transform.forward;

        // 先頭から何番目に位置するか
        _data.HeadNumber = _number;

        // 目的地を更新
        _data.Destination = pos - (backword * m_distance * (_data.HeadNumber + 1));

        // 更新されたことを記憶
        _data.IsUpdate = true;

    }


    #region　メソッド説明
    /// <summary>
    /// 目的地を計算する
    /// </summary>
    #endregion
    public void UpdateQueueDataList()
    {

        // 座標を更新
        for (int i = 0; i < m_queueDataList.Count; ++i)
        {
            var data = m_queueDataList[i];

            if (data == null) continue;

            UpdateQueueData(data, i);

        }
    }



}
