using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearGangsterEvent : BaseManagementEvent
{
    // 制作者　田内
    // ヤンキーを出現させる

    //==================================================================

    [Header("ヤンキーのプレハブリスト")]
    [SerializeField]
    private List<GameObject> m_gangsterPrefabList = new();

    [Header("出現位置")]
    [SerializeField]
    private GameObject m_appearPoint = null;

    // 作成されたヤンキー
    private GameObject m_createGangster = null;

    //==================================================================

    public override void OnStart()
    {
        Create();
    }


    public override void OnUpdate()
    {
        // ヤンキーがnullであれば
        if (m_createGangster == null)
        {
            SetEventEnd();
        }
    }


    private void Create()
    {

        if (m_appearPoint == null)
        {
            Debug.LogError("出現ポイントがシリアライズされていません");
            return;
        }

        // ランダムで取得
        var obj = m_gangsterPrefabList.Random();
        if (obj == null)
        {
            Debug.LogError("ヤンキープレハブがシリアライズされていません");
            return;
        }

        // インスタンスを作成
        var pos = m_appearPoint.transform.position;
        var rot = m_appearPoint.transform.rotation;
        m_createGangster = Instantiate(obj, pos, rot);


        // キャラクターモーターを更新する
        var core = m_createGangster.GetComponent<MyCharacterController>();
        if (core != null)
        {
            core.SetPositionMotor(pos);
        }
        else
        {
            Destroy(m_createGangster);
        }

    }


}
