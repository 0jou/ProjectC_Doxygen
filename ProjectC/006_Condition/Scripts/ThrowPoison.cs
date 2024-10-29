using Arbor;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

// 投げた時の毒の処理（伊波）

public class ThrowPoison : MonoBehaviour
{
    [Header("効果時間")]
    [SerializeField] private float m_time;

    [Header("速度低下率")]
    [SerializeField, Range(0.0f, 1.0f)] private float m_rateSpeed = 0.5f;

    [Header("毒を付与するまでの時間")]
    [SerializeField] private float m_poisonedTime;

    [Header("付与用の弱毒プレハブ")]
    [SerializeField] private GameObject m_poisonPrefab;

    class HitData
    {
        public ConditionManager m_conditionManager;
        public MyCharacterController m_characterController;
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
                m_hitConditionManagers[i].m_characterController.SpeedRate = m_rateSpeed;
                m_hitConditionManagers[i].m_hitTime += Time.deltaTime;
                if (m_hitConditionManagers[i].m_hitTime > m_poisonedTime)
                {
                    m_hitConditionManagers[i].m_conditionManager.AddCondition(m_poisonPrefab);
                }
                return;
            }
        }

        ConditionManager conditionmanager = other.GetComponentInChildren<ConditionManager>();
        if (conditionmanager == null) return;
        MyCharacterController controller = other.GetComponentInChildren<MyCharacterController>();
        if (controller == null) return;

        HitData hitData = new HitData();
        hitData.m_conditionManager = conditionmanager;
        hitData.m_characterController = controller;
        m_hitConditionManagers.Add(hitData);
    }
}
