using Arbor;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

// 麻痺状態時の処理（伊波）

public class ThrowParalysis : MonoBehaviour
{
    [Header("効果時間")]
    [SerializeField] private float m_time;

    [Header("しびれ状態を付与するまでの時間")]
    [SerializeField] private float m_paralysisTime;

    [Header("付与用の痺れプレハブ")]
    [SerializeField] private GameObject m_paralysisPrefab;

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
            if (m_hitConditionManagers[i].m_conditionManager == null)
            {
                continue;
            }

            if (m_hitConditionManagers[i].m_conditionManager.transform.root == other.transform.root)
            {
                m_hitConditionManagers[i].m_hitTime += Time.deltaTime;
                if (m_hitConditionManagers[i].m_hitTime > m_paralysisTime)
                {
                    m_hitConditionManagers[i].m_conditionManager.AddCondition(m_paralysisPrefab, false);
                    m_hitConditionManagers[i].m_hitTime = m_paralysisTime - 1.0f;
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
