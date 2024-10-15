using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearPasserby : MonoBehaviour
{
    // 通行人を出現させる
    // 制作者　田内

    [Header("出現する通行人のリスト")]
    [SerializeField]
    private List<GameObject> m_appearPrefabList = new();

    //============================================================

    [Header("通行人を出現させる待機時間")]
    [SerializeField]
    private float m_appearDelay = 10.0f;

    private float m_appearDelayCount = 0.0f;

    //============================================================

    [Header("出現させる位置")]
    [SerializeField]
    private GameObject m_appearPoint = null;

    //============================================================

    [Header("帰る位置")]
    [SerializeField]
    private GameObject m_exitPoint = null;

    //==============================================================

    private void Update()
    {
        Appear();
    }

    private void Appear()
    {

        m_appearDelayCount += Time.deltaTime;

        if (m_appearDelay < m_appearDelayCount)
        {

            // 初期値に戻す
            m_appearDelayCount -= m_appearDelay;

            // 通行人を作成
            CreateObject();

        }
    }



    private void CreateObject()
    {
        if (m_appearPoint == null || m_exitPoint == null)
        {
            Debug.LogError("ポイントが存在しません");
            return;
        }


        // ランダムで値を取得
        int rand = Random.Range(0, m_appearPrefabList.Count - 1);


        if (m_appearPrefabList[rand] == null)
        {
            Debug.LogError("シリアライズされているGameObjectがnullです");
            return;
        }

        var pos = m_appearPoint.transform.position;
        var rotate = m_appearPoint.transform.rotation;

        // インスタンスを作成
        var obj = Instantiate(m_appearPrefabList[rand], pos, rotate);


        // 通行人情報を更新
        var data = obj.GetComponent<PasserbyData>();
        if (data == null)
        {
            Debug.LogError("通行人情報が存在しません");
            return;
        }


        // 出口座標をセット
        var exitPos = m_exitPoint.transform.position;
        data.ExitPos = exitPos;



        // キャラクターモーターを更新する
        var core = obj.GetComponent<MyCharacterController>();
        if (core == null)
        {
            Debug.LogError("キャラクターコントローラーが存在しません");
        }

        core.SetPositionMotor(pos);


        return;
    }

}
