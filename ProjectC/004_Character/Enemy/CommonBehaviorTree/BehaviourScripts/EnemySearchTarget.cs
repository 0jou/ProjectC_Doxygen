using UnityEngine;
using Arbor;
using Arbor.BehaviourTree;
using System;
using UnityEngine.AI;

[Arbor.Internal.Documentable]
[System.Flags]
public enum SearchTargets
{
    Player = 1 << 0,
    Enemy = 1 << 1,
    FoodItem = 1 << 2,
}
// プレイヤーと罠ごはんの中が範囲内に1つでもがあればTrue（伊波）
// スポーン地点から離れていれば無視

[AddComponentMenu("")]
[AddBehaviourMenu("Enemy/EnemySearchTarget")]
public class EnemySearchTarget : Decorator
{
    [SerializeField] private FlexibleSearchType m_searchFlags;
    [SerializeField] private FlexibleTransform m_targetParameter;
    [SerializeField] private FlexibleChaseParameters m_chaseParameters;

    private Collider m_myCollider;
    private Vector3 m_startPos;
    private VisualFieldJudgment m_judgment=new();

    protected override void OnAwake()
    {
        transform.root.TryGetComponent(out m_myCollider);
        if(transform.root.TryGetComponent(out AgentController agent))
        {
            m_startPos = agent.StartPosition;
        }
    }

    //protected override void OnStart()
    //{
    //}

    protected override bool OnConditionCheck()
    {
        if (Vector3.Distance(transform.position, m_startPos) >= m_chaseParameters.value.DistAwayFromSpawnPos) return false;
       
        float nearestDist = float.MaxValue;
        bool isFind = false;

        // プレイヤーが範囲内にいるか調べる
        foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if ((m_searchFlags.value.searchTargets & SearchTargets.Player) == SearchTargets.Player)
            {
                if (chara.GroupNo == CharacterGroupNumber.player)
                {
                    if (m_judgment.SearchTargetNearSpawn(chara.gameObject, m_chaseParameters.value, m_myCollider, m_startPos, ref nearestDist, m_chaseParameters.value.SearchCharacterDist))
                    {
                        m_targetParameter.parameter.SetTransform(chara.transform);
                        isFind = true;
                        continue;
                    }
                }
            }
            if ((m_searchFlags.value.searchTargets & SearchTargets.Enemy) == SearchTargets.Enemy)
            {
                if (chara.GroupNo == CharacterGroupNumber.enemy)
                {
                    if (m_judgment.SearchTargetNearSpawn(chara.gameObject, m_chaseParameters.value, m_myCollider, m_startPos, ref nearestDist, m_chaseParameters.value.SearchCharacterDist))
                    {
                        m_targetParameter.parameter.SetTransform(chara.transform);
                        isFind = true;
                        continue;
                    }
                }
            }
        }

        // 罠ごはんが範囲内にあるか調べる　より正面にあればそちらを優先で使用
        if ((m_searchFlags.value.searchTargets & SearchTargets.FoodItem) == SearchTargets.FoodItem)
        {
            foreach (var item in IMetaAI<AssignItemID>.Instance.ObjectList)
            {
                if (item.ItemTypeID != ItemInfo.ItemTypeID.Food) continue;

                if (m_judgment.SearchTargetNearSpawn(item.gameObject, m_chaseParameters.value, m_myCollider, m_startPos, ref nearestDist, m_chaseParameters.value.SearchCharacterDist))
                {
                    m_targetParameter.parameter.SetTransform(item.transform);
                    isFind = true;
                }
            }
        }

        if (isFind)
        {
            return true;
        }
        return false;
    }

    //protected override void OnEnd()
    //{
    //}
}
