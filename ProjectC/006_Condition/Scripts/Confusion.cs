/**
* @file Confusion.cs
* @brief 敵の混乱状態処理
*/
using Arbor;
using UnityEngine;

/**
* @brief 敵の混乱状態処理
* @details 味方も間違えて攻撃するようになる
*/
public class Confusion : MonoBehaviour, ICondition
{
    [Header("混乱時間")]
    [SerializeField, EnumIndex(typeof(ConditionInfo.ResistanceID))]
    public float[] m_maxConfusionTime = new float[(int)ConditionInfo.ResistanceID.ResistanceTypeNum];

    [Header("エフェクト")]
    [SerializeField] private GameObject m_effectAssetPrefab;
    private GameObject m_effect;

    private float m_confusionTime;

    private ConditionInfo.ConditionID m_conditionID = ConditionInfo.ConditionID.Confusion;
    public ConditionInfo.ConditionID ConditionID => m_conditionID;

    public GameObject Owner { get; set; }
    private ArborFSM m_arbor;

    public bool IsEffective() { return m_confusionTime > 0.0f; }
    public float DamageMulti(ConditionInfo.ResistanceID[] resistances)
    {
        return 1.0f;
    }

    public void ReplaceCondition(ICondition newCondition, ConditionInfo.ResistanceID[] resistances)
    {
        Confusion newConfusion = newCondition as Confusion;
        if (newCondition == null) return;
        if ((int)m_conditionID < resistances.Length)
        {
            if (!gameObject.transform.parent.TryGetComponent(out ConditionManager m_conditionManager)) return;
            if (m_confusionTime < newConfusion.m_maxConfusionTime[(int)m_conditionManager.Resistances[(int)m_conditionID]])
            {
                m_confusionTime = newConfusion.m_maxConfusionTime[(int)m_conditionManager.Resistances[(int)m_conditionID]];
            }
        }
    }

    private void Awake()
    {
        if (gameObject.transform.parent.TryGetComponent(out ConditionManager m_conditionManager))
        {
            m_confusionTime = m_maxConfusionTime[(int)m_conditionManager.Resistances[(int)m_conditionID]];
        }
        else
        {
            Debug.LogError("ConditionManagerが見つかりませんでした");
        }

        if (!transform.root.TryGetComponent(out m_arbor))
        {
            if (!transform.root.CompareTag("Player")) Debug.LogError("ArborFSMが見つかりませんでした");
            m_confusionTime = 0.0f;
        }

        if (transform.root.TryGetComponent(out CharacterCore core))
        {
            core.DoFriendlyFire = true;
        }
    }

    void Start()
    {
        if (!IsEffective()) return;
        Owner = gameObject;
        m_effect = Instantiate(m_effectAssetPrefab, transform.root);
        if (m_arbor.parameterContainer.TryGetVariable("Search Type", out SearchType type))
        {
            type.searchTargets |= SearchTargets.Enemy;
        }
        else
        {
            Debug.Log("ArborFSMに Search Type がありません");
            m_confusionTime = 0.0f;
        }
    }


    private void Update()
    {
        m_confusionTime -= Time.deltaTime;
    }

    private void OnDestroy()
    {
        if (m_effect) Destroy(m_effect);

        //Todoプレイヤーは混乱にかからないための暫定処置（山本）
        if (m_arbor == null)
        {
            return;
        }

        m_arbor.parameterContainer.SetTransform("Target", null);
        if (transform.root.TryGetComponent(out CharacterCore core))
        {
            core.DoFriendlyFire = false;
        }

        if (m_arbor.parameterContainer.TryGetVariable("Search Type", out SearchType type))
        {
            type.searchTargets &= ~SearchTargets.Enemy;
        }
    }
}
