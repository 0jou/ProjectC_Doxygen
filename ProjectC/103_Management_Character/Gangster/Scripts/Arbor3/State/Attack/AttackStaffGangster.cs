using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using StaffStateInfo;
using UnityEngine.InputSystem;

/// <summary>
/// @ingroup UnUse
/// お前はいったい何をしているんだ
/// </summary>
[AddComponentMenu("Gangster/AttackStaffGangster")]
public class AttackStaffGangster : BaseGangsterStateBehaviour
{
    // 制作者　田内
    // ターゲットのスタッフを攻撃する

    private void Attack()
    {

    }




    //=================================================

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
    }

    // Use this for exit state
    public override void OnStateEnd()
    {

    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        // 攻撃
        Attack();
    }

    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
    }
}
