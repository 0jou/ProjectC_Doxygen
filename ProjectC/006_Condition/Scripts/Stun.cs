/**
* @file Stun.cs
* @brief キャラクターのスタン状態処理
*/
using Arbor;
using UnityEngine;

/**
* @brief キャラクターのスタン状態処理
* @details 指定時間動けなくなる
* TODO:レバガチャ対応する？
*/
public class Stun : MonoBehaviour, ICondition
{
    [Header("最長スタン時間")]
    [SerializeField, EnumIndex(typeof(ConditionInfo.ResistanceID))]
    public float[] m_maxStunTime = new float[(int)ConditionInfo.ResistanceID.ResistanceTypeNum];

    [Header("エフェクト")]
    [SerializeField] private GameObject m_effectAssetPrefab;
    private GameObject m_effect;

    [Header("エフェクトを出すShareNodeの名前")]
    [SerializeField] private string m_parent;

    private float m_stunTime;

    private ConditionInfo.ConditionID m_conditionID = ConditionInfo.ConditionID.Stun;
    public ConditionInfo.ConditionID ConditionID => m_conditionID;

    public GameObject Owner { get; set; }
    private ArborFSM m_arbor;
    private Animator m_animator;

    public bool IsEffective() { return m_stunTime > 0.0f; }
    public float DamageMulti(ConditionInfo.ResistanceID[] resistances)
    {
        return 1.0f;
    }

    public void ReplaceCondition(ICondition newCondition, ConditionInfo.ResistanceID[] resistances)
    {
        Stun newStun = newCondition as Stun;
        if (newStun == null) return;
        if ((int)m_conditionID < resistances.Length)
        {
            if (!gameObject.transform.parent.TryGetComponent(out ConditionManager m_conditionManager)) return;
            if (m_stunTime < newStun.m_maxStunTime[(int)m_conditionManager.Resistances[(int)m_conditionID]])
            {
                m_stunTime = newStun.m_maxStunTime[(int)m_conditionManager.Resistances[(int)m_conditionID]];
            }
        }
    }

    private void Awake()
    {
        if (gameObject.transform.parent.TryGetComponent(out ConditionManager m_conditionManager))
        {
            m_stunTime = m_maxStunTime[(int)m_conditionManager.Resistances[(int)m_conditionID]];
        }
        else
        {
            Debug.LogError("ConditionManagerが見つかりませんでした");
        }

        if (!transform.root.TryGetComponent(out m_arbor))
        {
            if (!transform.root.CompareTag("Player"))
            {
                Debug.LogError("ArborFSMが見つかりませんでした");
                m_stunTime = 0.0f;
            }
        }
    }

    void Start()
    {
        if (!IsEffective()) return;
        Owner = gameObject;

        //ShareNodeが存在する場合はShareNodeの指定部位の座標にエフェクトを出現させる（山本）
        if (m_parent.Length != 0)
        {
            if (transform.root.TryGetComponent(out ShareNodes comp))
            {
                if (comp.Nodes.TryGetValue(m_parent, out Transform parent))
                {
                    m_effect = Instantiate(m_effectAssetPrefab, parent);

                }
                else
                {
                    Debug.LogError("指定名称がShareNodeに存在しません。登録してください。");
                }
            }
            else
            {
                Debug.LogError("ShareNodesコンポーネントが見つかりませんでした");
            }

        }
        else
        {
            m_effect = Instantiate(m_effectAssetPrefab, transform.root);
        }

        if (m_arbor)
        {
            m_arbor.Pause();
        }

        m_animator = transform.root.GetComponentInChildren<Animator>();
        if (m_animator)
        {
            m_animator.SetBool("IsStun", true);
        }
        else
        {
            m_stunTime = 0.0f;
        }
    }


    private void Update()
    {
        m_stunTime -= Time.deltaTime;
    }

    private void OnDestroy()
    {
        if (m_effect) Destroy(m_effect);

        if (m_arbor)
        {
            m_arbor.Resume();
        }

        if (m_animator)
        {
            m_animator.SetBool("IsStun", false);
        }
    }
}
