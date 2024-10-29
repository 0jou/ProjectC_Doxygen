using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;


public class AppearObject : MonoBehaviour
{
    // characterを出現させる
    // 制作者　田内

    [Header("オブジェクトリスト")]
    [SerializeField]
    private List<GameObject> m_objectList = new();

    //============================================================

    [Header("待機時間")]
    [SerializeField]
    private float m_appearDelay = 10.0f;

    [Header("最大出現数")]
    [SerializeField]
    private int m_maxAppear = 5;

    //============================================================

    // 保存しておくリスト
    private List<GameObject> m_createObjectList = new();


    private float m_appearDelayCount = 0.0f;


    private void Update()
    {
        Appear();
    }

    private void Appear()
    {
        // 作成最大数を越えるのであれば
        if (m_maxAppear <= m_createObjectList.Count) return;

        // タイムをカウントする
        m_appearDelayCount += Time.deltaTime;

        if (m_appearDelay < m_appearDelayCount)
        {
            // 初期値に戻す
            m_appearDelayCount -= m_appearDelay;

            // 作成処理
            UpdateCreate();

        }
    }


    // 作成処理
    virtual protected void UpdateCreate()
    {
        CreateObject();
    }


    // オブジェクトを作成
    protected GameObject CreateObject()
    {
        // ランダムで取得
        var prefab = m_objectList.Random();
        if (prefab == null)
        {
            Debug.LogError("シリアライズされていません");
            return null;
        }

        // インスタンスを作成
        var obj = Instantiate(prefab, transform.position, transform.rotation);

        // リストで保持
        m_createObjectList.Add(obj);
        obj.OnDestroyAsObservable()
               .Subscribe(_ =>
               {
                   m_createObjectList.Remove(obj);

               }).AddTo(this);

        return obj;
    }

}
