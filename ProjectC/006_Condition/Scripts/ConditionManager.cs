using ConditionInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Arbor.BehaviourTree.Decorator;

// 状態異常の追加・削除を管理するコンポーネント（伊波）

public class ConditionManager : MonoBehaviour
{
    [Header("属性耐性")]
    [SerializeField, EnumIndex(typeof(ConditionInfo.ConditionID))]
    private ConditionInfo.ResistanceID[] m_resistances = new ConditionInfo.ResistanceID[(int)ConditionInfo.ConditionID.ConditionTypeNum];
    public ConditionInfo.ResistanceID[] Resistances { get { return m_resistances; } }

    private GameObject m_owner;
    private Transform m_myTransform;

    public void AddCondition(GameObject conditionPrefab, bool _isAttackCondition)
    {
        if (!conditionPrefab.TryGetComponent(out ICondition newCondition)) return;

        ICondition condition;

        // 攻撃でNormalの付与はしない（料理食べて太る処理になる）
        if (_isAttackCondition)
        {
            if (newCondition.ConditionID == ConditionID.Normal) return;
        }

        for (int i = 0; i < m_myTransform.childCount; i++)
        {
            condition = m_myTransform.GetChild(i).GetComponent<ICondition>();
            if (condition == null) continue;
            if (condition.ConditionID == newCondition.ConditionID)
            {
                condition.ReplaceCondition(newCondition, m_resistances);
                return;
            }
        }

        // 元からある状態異常と被ってなければ新規作成
        Instantiate(conditionPrefab, m_myTransform);
    }

    public bool IsCondition(ConditionID id)
    {
        ICondition condition;
        for (int i = 0; i < m_myTransform.childCount; i++)
        {
            condition = m_myTransform.GetChild(i).GetComponent<ICondition>();
            if (condition == null) continue;
            if (condition.ConditionID == id) return true;
        }
        return false;
    }

    public float DamageMulti()
    {
        float damageMulti = 1f;

        ICondition condition;
        for (int i = 0; i < m_myTransform.childCount; i++)
        {
            condition = m_myTransform.GetChild(i).GetComponent<ICondition>();
            if (condition == null) continue;
            if (!condition.IsEffective()) continue;
            damageMulti += condition.DamageMulti(m_resistances) - 1.0f;
        }
        return damageMulti;
    }

    void Start()
    {
        TryGetComponent(out m_myTransform);
        m_owner = m_myTransform.root.gameObject;
    }

    void Update()
    {
        int childrenNum = m_myTransform.childCount;
        ICondition condition;
        for (int i = childrenNum - 1; i >= 0; --i)
        {
            condition = m_myTransform.GetChild(i).GetComponent<ICondition>();
            if (condition == null) continue;
            if (!condition.IsEffective())
            {
                Destroy(condition.Owner);
                continue;
            }
        }
    }
}
