using Arbor;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

// 投げた時の激辛料理の処理（伊波）

public class ThrowSleep : MonoBehaviour
{
    [Header("効果時間")]
    [SerializeField] private float m_time;

    [Header("眠り状態を付与するまでの時間")]
    [SerializeField] private float m_sleepTime;

    [Header("付与用の睡眠プレハブ")]
    [SerializeField] private GameObject m_sleepPrefab;

    class HitData
    {
        public ConditionManager m_conditionManager;
        public float m_hitTime;
    }
    private List<HitData> m_hitConditionManagers = new List<HitData>();


    private void Update()
    {
        m_time -= Time.deltaTime;
        if (m_time <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        for (int i = 0; i < m_hitConditionManagers.Count; ++i)
        {
            //nullCheck（山本）
            if (m_hitConditionManagers[i].m_conditionManager==null)
            {
                continue;
            }

            if (m_hitConditionManagers[i].m_conditionManager.transform.root == other.transform.root)
            {
                if (m_hitConditionManagers[i].m_conditionManager.IsCondition(ConditionInfo.ConditionID.Sleep))
                {
                    m_hitConditionManagers[i].m_hitTime = 0.0f;
                    continue;
                }

                m_hitConditionManagers[i].m_hitTime += Time.deltaTime;
                if (m_hitConditionManagers[i].m_hitTime > m_sleepTime)
                {
                    m_hitConditionManagers[i].m_conditionManager.AddCondition(m_sleepPrefab, false);
                    m_hitConditionManagers[i].m_hitTime = 0.0f;
                }
            }
        }

        ConditionManager conditionmanager = other.GetComponentInChildren<ConditionManager>();
        if (conditionmanager == null) return;

        HitData hitData = new HitData();
        hitData.m_conditionManager = conditionmanager;
        m_hitConditionManagers.Add(hitData);
    }
}
