using UnityEngine;
using Cysharp.Threading.Tasks;
using Arbor;
using Arbor.BehaviourTree;
using System;

// 時間が過ぎたらtrue（伊波）

[AddComponentMenu("")]
[AddBehaviourMenu("Time/CountDown")]
public class CountDown : Decorator
{
    [SerializeField] private FlexibleFloat m_maxTime;
    private bool m_isClear;

    //protected override void OnAwake()
    //{
    //}

    protected override async void OnStart()
    {
        m_isClear = false;
        await UniTask.Delay(TimeSpan.FromSeconds(m_maxTime.value));
        m_isClear = true;
    }

    protected override bool OnConditionCheck()
    {
        return m_isClear;
    }

    //protected override void OnEnd()
    //{
    //}
}
