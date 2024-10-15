using Arbor;
using ConditionInfo;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

// 麻痺状態時の処理（伊波）

public class Paralysis : MonoBehaviour, ICondition
{
    [Header("しびれ回数")]
    [SerializeField, EnumIndex(typeof(ConditionInfo.ResistanceID))]
    public int[] m_paralysisCounts = new int[(int)ConditionInfo.ResistanceID.ResistanceTypeNum];
    [Header("一回ごとのしびれ時間")]
    [SerializeField] private float m_paralysisTime;
    [Header("しびれるまでの間隔")]
    [SerializeField] private float m_paralysisInterval;

    // 麻痺エフェクト(倉田)
    [Header("エフェクト")]
    [SerializeField] private GameObject m_effectAssetPrefab;
    private GameObject m_paralysisEffect;   //自由に削除するためのキャッシュ

    [Header("サウンド")]
    [SerializeField] private GameObject m_sePrefab;

    private ConditionInfo.ConditionID m_conditionID = ConditionID.Paralysis;
    public ConditionInfo.ConditionID ConditionID => m_conditionID;

    public GameObject Owner { get; set; }
    private Animator m_animator;
    private int m_paralysisCount;

    public bool IsEffective() { return m_paralysisCount > 0; }
    public float DamageMulti(ConditionInfo.ResistanceID[] resistances) { return 1.0f; }
    public void ReplaceCondition(ICondition newCondition, ConditionInfo.ResistanceID[] resistances)
    {
        Paralysis newParalysis = newCondition as Paralysis;
        if (newParalysis == null) return;
        if ((int)m_conditionID < resistances.Length)
        {
            if (!gameObject.transform.parent.TryGetComponent(out ConditionManager m_conditionManager)) return;
            float nowSumTime = m_paralysisTime * m_paralysisCount;
            float newSumTime = newParalysis.m_paralysisTime * newParalysis.m_paralysisCounts[(int)m_conditionManager.Resistances[(int)m_conditionID]];
            if (nowSumTime < newSumTime)
            {
                m_paralysisCount = newParalysis.m_paralysisCounts[(int)m_conditionManager.Resistances[(int)m_conditionID]];
                m_paralysisTime = newParalysis.m_paralysisTime;
                m_paralysisInterval = newParalysis.m_paralysisInterval;
            }
        }
    }
    private void Awake()
    {
        m_conditionID = ConditionID.Paralysis;
    }

    async void Start()
    {
        Owner = gameObject;
        if (!gameObject.transform.parent.TryGetComponent(out ConditionManager conditionManager))
        {
            return;
        }

        m_paralysisCount = m_paralysisCounts[(int)conditionManager.Resistances[(int)m_conditionID]];


        ParameterContainer parameterContainer;
        if (!transform.root.TryGetComponent(out parameterContainer))
        {
            m_paralysisCount = 0;
            return;
        }
        if (!parameterContainer.TryGetComponent("Animator", out m_animator))
        {
            Debug.LogError("ParameterContainerにAnimatorがありません" + gameObject.transform.root.name);
            m_paralysisCount = 0;
            return;
        }
        m_animator.SetBool("IsParalysis", true);


        // エフェクトを出す
        // 一番親オブジェクト(敵本体)に出す
        var rootTrans = transform.root;
        m_paralysisEffect = Instantiate(m_effectAssetPrefab, rootTrans);

        while (IsEffective())
        {
            //HP0なら強制的に麻痺状態をぬける(山本)
            if(gameObject.transform.root.TryGetComponent(out CharacterCore characterCore))
            {
                if(characterCore.Status.m_hp.Value<=0.0f)
                {
                    m_paralysisEffect.SetActive(false);
                    m_animator.SetBool("IsParalysis", false);
                    break;
                }
            }

            if (m_paralysisEffect == null) break;
            m_paralysisEffect.SetActive(true);
            Instantiate(m_sePrefab, rootTrans);
            m_animator.SetBool("IsParalysis", true);
            await UniTask.Delay(TimeSpan.FromSeconds(m_paralysisTime));


            if (m_paralysisEffect == null) break;
            m_paralysisEffect.SetActive(false);
            m_animator.SetBool("IsParalysis", false);
            await UniTask.Delay(TimeSpan.FromSeconds(m_paralysisInterval));

            m_paralysisCount--;
        }
    }

    private void OnDestroy()
    {
        if (m_animator) m_animator.SetBool("IsParalysis", false);
        if (m_paralysisEffect) Destroy(m_paralysisEffect);
    }
}
