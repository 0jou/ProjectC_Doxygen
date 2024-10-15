using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

// ターゲットが指定のオブジェクトか調べる(伊波)

[AddComponentMenu("")]
public class CheckTargetType : Decorator
{

    public enum TargetTypes
    {
        None,
        [InspectorName("キャラクター")] Character,
        [InspectorName("設置罠")] PutItem
    }
    [SerializeField] private TargetTypes m_inputType;

    [SerializeField] private FlexibleTransform m_targetTrans;

    protected override void OnAwake()
    {
    }

    protected override void OnStart()
    {
    }

    protected override bool OnConditionCheck()
    {

        switch (m_inputType)
        {
            case TargetTypes.None:
                if (m_targetTrans.value==null)
                {
                    return true;
                }
                break;
            case TargetTypes.Character:
                if (m_targetTrans.value.gameObject.TryGetComponent(out CharacterCore core))
                {
                    return true;
                }
                break;
            case TargetTypes.PutItem:
                if (m_targetTrans.value.gameObject.TryGetComponent(out AssignItemID item))
                {
                    return true;
                }
                break;
            default:
                return false;
        }

        return false;
    }

    protected override void OnEnd()
    {
    }
}
