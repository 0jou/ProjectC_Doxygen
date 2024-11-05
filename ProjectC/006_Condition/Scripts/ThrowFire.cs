using Arbor;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

// 投げた時の激辛料理の処理（伊波）

public class ThrowFire : MonoBehaviour
{
    [Header("効果時間")]
    [SerializeField] private float m_time;

    [Header("混乱を付与するまでの時間")]
    [SerializeField] private float m_confusionTime;

    [Header("付与用の混乱プレハブ")]
    [SerializeField] private GameObject m_confusionPrefab;

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
            if (m_hitConditionManagers[i].m_conditionManager.transform.root == other.transform.root)
            {
                m_hitConditionManagers[i].m_hitTime += Time.deltaTime;
                if (m_hitConditionManagers[i].m_hitTime > m_confusionTime)
                {
                    m_hitConditionManagers[i].m_conditionManager.AddCondition(m_confusionPrefab, false);
                }
                return;
            }
        }

        ConditionManager conditionmanager = other.GetComponentInChildren<ConditionManager>();
        if (conditionmanager == null) return;

        HitData hitData = new HitData();
        hitData.m_conditionManager = conditionmanager;
        m_hitConditionManagers.Add(hitData);
    }
}
