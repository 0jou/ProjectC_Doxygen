/*!
 * @file DecoCheckStateGangster.cs
 * @brief GangsterStateが特定のステートか確認
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;
using GangsterStateInfo;

[AddComponentMenu("")]
public class DecoCheckStateGangster : BaseGangsteDecorator
{
    // 制作者 田内
    // スタッフの状態で判別するデコレーター


    [Header("一致するか確認するステート")]
    [SerializeField]
    private List<GangsterState> m_gangsterStateList = new();

  
    protected override void OnAwake()
    {
    }

    protected override void OnStart()
    {
    }

    protected override bool OnConditionCheck()
    {
        var data = GetGangsterData();
        if (data == null) return false;

        foreach (var state in m_gangsterStateList)
        {
            // ステートが一致すれば
            if (data.CurrentGangsterState == state)
            {
                return true;
            }
        }

        // 不一致であれば
        return false;
    }

    protected override void OnEnd()
    {
    }
}
