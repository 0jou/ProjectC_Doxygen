using Arbor;
using ItemInfo;
using UnityEngine;

// 通常料理（回復）の処理　伊波

public class Normal : MonoBehaviour, ICondition
{
    [Header("敵のHP回復割合")]
    [SerializeField, EnumIndex(typeof(ConditionInfo.ResistanceID))]
    public float[] m_maxCureRate = new float[(int)ConditionInfo.ResistanceID.ResistanceTypeNum];

    [Header("エフェクト")]
    [SerializeField] private GameObject m_effectAssetPrefab;
    private GameObject m_effect;

    //private float m_fireTime;

    private ConditionInfo.ConditionID m_conditionID = ConditionInfo.ConditionID.Normal;
    public ConditionInfo.ConditionID ConditionID => m_conditionID;

    public GameObject Owner { get; set; }

    private CharacterCore m_core;
    private EnemyParameters m_parameters;

    public bool IsEffective() { return true; }
    public float DamageMulti(ConditionInfo.ResistanceID[] resistances)
    {
        return 1.0f;
    }
    public void ReplaceCondition(ICondition newCondition, ConditionInfo.ResistanceID[] resistances)
    {
        if (m_parameters)
        {
            m_parameters.DropNum = Mathf.Min(m_parameters.DropNum + 1, 3);

            if (m_core)
            {
                if (!gameObject.transform.parent.TryGetComponent(out ConditionManager m_conditionManager)) return;
                var cureRate = m_maxCureRate[(int)m_conditionManager.Resistances[(int)m_conditionID]];
                var hp = m_core.Status.m_hp.Value += cureRate * m_core.Status.MaxHP;
                //最大値以上回復するなら最大値にする
                if (hp > m_core.Status.MaxHP)
                {
                    m_core.Status.m_hp.Value = m_core.Status.MaxHP;
                }
            }
        }
    }

    //private void Awake()
    //{
    //}

    void Start()
    {
        Owner = gameObject;

        if (gameObject.transform.root.TryGetComponent(out m_parameters))
        {
            m_parameters.DropNum = Mathf.Min(m_parameters.DropNum + 1, 3);

            if (gameObject.transform.root.TryGetComponent(out m_core))
            {
                if (!gameObject.transform.parent.TryGetComponent(out ConditionManager m_conditionManager)) return;
                var cureRate = m_maxCureRate[(int)m_conditionManager.Resistances[(int)m_conditionID]];
                var hp = m_core.Status.m_hp.Value += cureRate * m_core.Status.MaxHP;
                //最大値以上回復するなら最大値にする
                if (hp > m_core.Status.MaxHP)
                {
                    m_core.Status.m_hp.Value = m_core.Status.MaxHP;
                }
            }
        }

        // エフェクトを出す
        m_effect = Instantiate(m_effectAssetPrefab, transform.root);
    }

    public void OnDestroy()
    {
        if (m_effect)
        {
            Destroy(m_effect);
        }
    }
}
