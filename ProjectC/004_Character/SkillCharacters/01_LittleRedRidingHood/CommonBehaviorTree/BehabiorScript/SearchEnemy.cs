using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

//エネミーが範囲内にいればTrueを返す(山本)
//移動地点から離れていれば無視して戻る
//参考（EnemySearchTarget.cs）

[AddComponentMenu("")]
public class SearchEnemy : Decorator
{
    [SerializeField] private FlexibleSearchType m_searchFlags;
    [SerializeField] private FlexibleTransform m_targetParameter;
    [SerializeField] private FlexibleChaseParameters m_chaseParameters;
    [SerializeField] private FlexibleComponent m_targetCore;

    private Collider m_myCollider;
    private readonly VisualFieldJudgment m_judgment = new();

    protected override void OnStart()
    {
        m_myCollider = transform.root.GetComponentInChildren<Collider>();
        if (!m_myCollider)
        {
            Debug.LogError("Colliderが見つかりませんでした" + gameObject.name);
        }
    }

    protected override bool OnConditionCheck()
    {
        float nearestDist = m_chaseParameters.value.SearchCharacterDist;
        bool isFind = false;

        foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (chara.GroupNo == CharacterGroupNumber.enemy)
            {
                if (chara.Status.m_hp.Value <= 0.0f)
                {
                    continue;
                }

                if(m_myCollider==null)
                {
                    continue;
                }

                if (m_judgment.SearchTarget(chara.gameObject, m_chaseParameters.value, m_myCollider, ref nearestDist))
                {
                    m_targetParameter.parameter.SetTransform(chara.transform);
                    //敵のCharacterCoreを参照
                    m_targetCore.parameter.SetComponent(chara);
                    isFind = true;
                    continue;
                }
            }
        }
        return isFind;
    }
}
