using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using GangsterStateInfo;

[AddComponentMenu("")]
public class SetStateGangster : BaseGangsterStateBehaviour
{
    // 制作者 田内
    // ヤンキーのステートをセットする


    [Header("ヤンキーにセットするステート")]
    [SerializeField]
    private GangsterState m_gangsterState = new();


    private void SetState()
    {
        var data = GetGangsterData();
        if (data == null) return;

        data.CurrentGangsterState = m_gangsterState;

    }


    // Use this for initialization
    void Start()
    {

    }

    // Use this for awake state
    public override void OnStateAwake()
    {
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
        SetState();
    }

    // Use this for exit state
    public override void OnStateEnd()
    {
    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
    }

    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
    }
}
