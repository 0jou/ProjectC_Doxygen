using Arbor;
using ConditionInfo;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 眠り状態　伊波

public class Sleeping : MonoBehaviour, ICondition
{
    [Header("エフェクト")]
    [SerializeField] private GameObject m_effectAssetPrefab;
    private GameObject m_sleepingEffect;   //自由に削除するためのキャッシュ

    [Header("ダメージ倍率")]
    [SerializeField, EnumIndex(typeof(ConditionInfo.ResistanceID))]
    public float[] m_damageMulti = new float[(int)ConditionInfo.ResistanceID.ResistanceTypeNum];

    [Header("被ダメ後起きるまでの時間")]
    [SerializeField]
    private float m_wakeUpTime = 1.0f;

    //ShareNode存在時、指定名称の部位からエフェクトを発生させる（山本）
    [Header("ShareNodeに登録された部位の名称")]
    [SerializeField] private string m_parent;

    private ConditionInfo.ConditionID m_conditionID = ConditionID.Sleep;
    public ConditionInfo.ConditionID ConditionID => m_conditionID;

    public GameObject Owner { get; set; }
    private Animator m_animator;
    private ArborFSM m_arbor;
    bool m_damaged = false;

    public bool IsEffective() { return m_wakeUpTime > 0.0f; }
    public float DamageMulti(ConditionInfo.ResistanceID[] resistances)
    {
        m_damaged = true;
        if ((int)m_conditionID < resistances.Length)
        {
            return m_damageMulti[(int)resistances[(int)m_conditionID]];
        }
        return 1.0f;
    }
    public void ReplaceCondition(ICondition newCondition, ConditionInfo.ResistanceID[] resistances)
    {
        Sleeping newSleep = newCondition as Sleeping;
        if (newSleep == null) return;
        if ((int)m_conditionID < resistances.Length)
        {
            if (m_damageMulti[(int)resistances[(int)m_conditionID]] < newSleep.m_damageMulti[(int)resistances[(int)m_conditionID]])
            {
                m_damageMulti = newSleep.m_damageMulti;
            }
        }
    }

    void Start()
    {
        //Todo:Playerが睡眠状態にならないための処置(山本)
        if (transform.root.tag == "Player")
        {
            Destroy(gameObject);
            return;
        }

        Owner = gameObject;

        ParameterContainer parameterContainer;
        transform.root.TryGetComponent(out parameterContainer);

        if (!parameterContainer.TryGetComponent("Animator", out m_animator))
        {
            Debug.LogError("ParameterContainerにAnimatorがありません" + gameObject.transform.root.name);
        }
        m_animator.SetBool("IsSleeping", true);
        m_animator.Play("Move", -1, 0f);

        if (transform.root.TryGetComponent(out m_arbor))
        {
            m_arbor.Pause();
        }

        // エフェクトを出す
        // 一番親オブジェクト(敵本体)に出す
        var rootTrans = transform.root;

        //ShareNodeが存在する場合はShareNodeの指定部位の座標にエフェクトを出現させる（山本）
        if (m_parent.Length != 0)
        {
            if (rootTrans.TryGetComponent(out ShareNodes comp))
            {
                if (comp.Nodes.TryGetValue(m_parent, out Transform parent))
                {
                    m_sleepingEffect = Instantiate(m_effectAssetPrefab, parent);

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
            m_sleepingEffect = Instantiate(m_effectAssetPrefab, rootTrans);
        }

    }

    private void Update()
    {
        if (!m_damaged) return;
        if (m_animator == null)
        {
            m_wakeUpTime = 0.0f;
            return;
        }

        m_wakeUpTime -= Time.deltaTime;
    }

    private void OnDestroy()
    {
        if (m_animator != null) m_animator.SetBool("IsSleeping", false);
        Destroy(m_sleepingEffect);
        if (transform.root.TryGetComponent(out m_arbor))
        {
            m_arbor.Resume();
        }
    }
}
