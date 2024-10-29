using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

using OrderFoodInfo;

[AddComponentMenu("")]
public class CookingDecoratorScript : BaseChefDecorator
{
    // 制作者 田内
    // 調理中かどうか判定するデコレーター

    protected override bool OnConditionCheck()
    {
        return CounterManager.instance.IsCooking();
    }
}
