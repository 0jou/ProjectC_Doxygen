using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

[AddComponentMenu("")]
public class CheckPlayerPosition : Decorator
{

    //プレイヤーの位置情報を確認する（山本）

    [SerializeField]
    private FlexibleTransform m_playerTransform = new FlexibleTransform();
    [SerializeField]
    private FlexibleFloat m_stoppingDistance = new FlexibleFloat();
    [SerializeField]
    private FlexibleFloat m_warpDistance = new FlexibleFloat();

    private Transform m_myTransform;
    private Vector3 m_targetPos = new();

    protected override void OnAwake()
    {
        base.OnAwake();
        m_myTransform = transform.root;

    }

    //protected override void OnStart()
    //{
    //}

    protected override bool OnConditionCheck()
    {
        bool _isWarpFlg = false;

        //プレイヤーの位置情報を探して代入
        foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (chara.transform.tag == "Player")
            {
                m_playerTransform.parameter.SetTransform(chara.transform);
                m_targetPos = m_playerTransform.value.position;

                var vec = m_targetPos - transform.position;
                var length = vec.magnitude;

                if (m_warpDistance.value <= length)
                {

                    _isWarpFlg = true;
                    return _isWarpFlg;
                }

                break;
            }
        }

        return _isWarpFlg;
    }

    //protected override void OnEnd()
    //{
    //}
}
