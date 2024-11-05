using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StaffInfo;

public partial class StaffData : MonoBehaviour
{
    // 制作者 田内
    // ステータス(1.0が平均値)


    [Header("スタッフタイプ")]
    [SerializeField]
    private StaffType m_staffType = StaffType.Hall;

    public StaffType StaffType
    {
        get { return m_staffType; }
        set { m_staffType = value; }
    }


    // スピードの関する
    [Header("配膳")]
    [SerializeField]
    [Range(0.1f, 2.0f)]
    private float m_provideRatio = 1.0f;

    public float ProvideRatio
    {
        get { return m_provideRatio; }
    }

    // 料理作成時間に関する
    [Header("調理")]
    [SerializeField]
    [Range(0.1f, 2.0f)]
    private float m_cookingRatio = 1.0f;

    public float CookingRatio
    {
        get { return m_cookingRatio; }
    }

    // TODO:未定
    [Header("接客")]
    [SerializeField]
    [Range(0.1f, 2.0f)]
    private float m_serviceRatio = 1.0f;

    public float ServiceRatio
    {
        get {return m_serviceRatio; }
    }

    //===========================================
    //              実行処理
    //===========================================

    private void SetStatus()
    {
        // 配膳(スピード)をセット
        if (gameObject.TryGetComponent<CharacterCore>(out var core))
        {
            core.Status.WalkSpeed *= m_provideRatio;
            core.Status.DushSpeed *= m_provideRatio;
        }
    }
}
