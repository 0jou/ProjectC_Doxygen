using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using ChefStateInfo;

[AddComponentMenu("")]
public class SetStateChef : BaseChefStateBehavior
{
    // シェフのステートをセットする
    // 制作者　田内

    [Header("シェフにセットするステート")]
    [SerializeField]
    private ChefState m_chefState = new();


    private void SetState()
    {
        var data = GetChefData();
        if (data == null) return;

        data.CurrentChefState = m_chefState;

    }

    // OnStateUpdate is called once per frame
    public override void OnStateBegin()
    {
        SetState();
    }

}
