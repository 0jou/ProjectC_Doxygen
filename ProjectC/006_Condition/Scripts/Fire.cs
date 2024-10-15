using Arbor;
using UnityEngine;

// 炎はき状態　伊波

public class Fire : MonoBehaviour, ICondition
{
    [Header("炎を吐く時間")]
    [SerializeField, EnumIndex(typeof(ConditionInfo.ResistanceID))]
    public float[] m_maxFireTime = new float[(int)ConditionInfo.ResistanceID.ResistanceTypeNum];

    
    private float m_fireTime;

    private ConditionInfo.ConditionID m_conditionID = ConditionInfo.ConditionID.Fire;
    public ConditionInfo.ConditionID ConditionID => m_conditionID;

    public GameObject Owner { get; set; }
    private Animator m_animator;
    private ArborFSM m_arborFSM;

    public bool IsEffective() { return m_fireTime > 0.0f; }
    public float DamageMulti(ConditionInfo.ResistanceID[] resistances)
    {
        return 1.0f;
    }
    public void ReplaceCondition(ICondition newCondition, ConditionInfo.ResistanceID[] resistances)
    {
        Fire newFire = newCondition as Fire;
        if (newFire == null) return;
        if ((int)m_conditionID < resistances.Length)
        {
            if (!gameObject.transform.parent.TryGetComponent(out ConditionManager m_conditionManager)) return;
            if (m_fireTime < newFire.m_maxFireTime[(int)m_conditionManager.Resistances[(int)m_conditionID]])
            {
                m_fireTime = newFire.m_maxFireTime[(int)m_conditionManager.Resistances[(int)m_conditionID]];
            }
        }
    }

    private void Awake()
    {
    }

    void Start()
    {
        Owner = gameObject;

        ParameterContainer parameterContainer;
        transform.root.TryGetComponent(out parameterContainer);

        if (!parameterContainer.TryGetComponent("Animator", out m_animator))
        {
            Debug.LogError("ParameterContainerにAnimatorがありません" + gameObject.transform.root.name);
        }
        m_animator.SetBool("IsFire", true);
        m_animator.Play("Move", -1, 0f);

        if (gameObject.transform.parent.TryGetComponent(out ConditionManager m_conditionManager))
        {
            m_fireTime = m_maxFireTime[(int)m_conditionManager.Resistances[(int)m_conditionID]];
        }

        // Arborの処理中断
        if (transform.root.TryGetComponent(out m_arborFSM))
        {
            m_arborFSM.Pause();
        }

    }


    private void Update()
    {
        // FireState中にカウント
        for (int i = 0; i < m_animator.layerCount; ++i)
        {
            if (!m_animator.GetCurrentAnimatorStateInfo(i).IsName("Fire")) continue;
            m_fireTime -= Time.deltaTime;
            break;
        }
    }

    private void OnDestroy()
    {
        m_animator.SetBool("IsFire", false);
        m_arborFSM.Resume();
    }
}
