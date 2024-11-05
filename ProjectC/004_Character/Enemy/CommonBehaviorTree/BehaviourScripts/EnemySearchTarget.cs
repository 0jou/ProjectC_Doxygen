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
    [SerializeField]
    [SlotType(typeof(EnemyParameters))]
    private FlexibleComponent m_enemyParameters;

    private Collider m_myCollider;
    private Vector3 m_startPos;
    private VisualFieldJudgment m_judgment = new();
    private EnemyParameters enemyParameters;
    private ChaseData m_chaseData;

    protected override void OnAwake()
    {
        transform.root.TryGetComponent(out m_myCollider);
        if (transform.root.TryGetComponent(out AgentController agent))
        {
            m_startPos = agent.StartPosition;
        }

        enemyParameters = m_enemyParameters.value as EnemyParameters;
        m_chaseData = enemyParameters.GetChaseData();
        if (m_chaseData == null)
        {
            Debug.Log("ChaseDataが取得できませんでした" + gameObject.name);
        }
    }

    //protected override void OnStart()
    //{
    //}

    protected override bool OnConditionCheck()
    {
        if (Vector3.Distance(transform.position, m_startPos) >= m_chaseData.DistAwayFromSpawnPos) return false;

        float nearestDist = float.MaxValue;
        bool isFind = false;

        // プレイヤーが範囲内にいるか調べる
        foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if ((enemyParameters.SearchTargets & SearchTargets.Player) == SearchTargets.Player)
            {
                if (chara.GroupNo == CharacterGroupNumber.player)
                {
                    if (m_judgment.SearchTargetNearSpawn(chara.gameObject, m_chaseData, m_myCollider, m_startPos, ref nearestDist, m_chaseData.SearchCharacterDist))
                    {
                        enemyParameters.Target = chara.transform;
                        isFind = true;
                        continue;
                    }
                }
            }
            if ((enemyParameters.SearchTargets & SearchTargets.Enemy) == SearchTargets.Enemy)
            {
                if (chara.GroupNo == CharacterGroupNumber.enemy)
                {
                    if (m_judgment.SearchTargetNearSpawn(chara.gameObject, m_chaseData, m_myCollider, m_startPos, ref nearestDist, m_chaseData.SearchCharacterDist))
                    {
                        enemyParameters.Target = chara.transform;
                        isFind = true;
                        continue;
                    }
                }
            }
        }

        // 罠ごはんが範囲内にあるか調べる　より正面にあればそちらを優先で使用
        if ((enemyParameters.SearchTargets & SearchTargets.FoodItem) == SearchTargets.FoodItem)
        {
            foreach (var item in IMetaAI<AssignItemID>.Instance.ObjectList)
            {
                if (item.ItemTypeID != ItemInfo.ItemTypeID.Food) continue;

                if (m_judgment.SearchTargetNearSpawn(item.gameObject, m_chaseData, m_myCollider, m_startPos, ref nearestDist, m_chaseData.SearchCharacterDist))
                {
                    enemyParameters.Target = item.transform;
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
