using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GangsterStateInfo;
using ManagementGameInfo;

public class AppearGangsterEvent : BaseManagementEvent
{
    // 制作者　田内
    // ヤンキーを出現させる

    //==================================================================

    [Header("ヤンキーのプレハブリスト")]
    [SerializeField]
    private List<GangsterData> m_gangsterPrefabList = new();

    [Header("出現位置")]
    [SerializeField]
    private GameObject m_appearPoint = null;

    // 作成されたヤンキー
    private GangsterData m_createGangster = null;

    //==================================================================

    public override void OnStart()
    {
        Create();
    }


    public override void OnUpdate()
    {
        // ヤンキーがnullもしくは倒された後であれば
        if (m_createGangster == null || m_createGangster.CurrentGangsterState == GangsterState.Dead)
        {
            // イベント解決
            SetEventEnd(EventSolutionType.Solution);
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
            Destroy(m_createGangster.gameObject);
        }

    }


}
