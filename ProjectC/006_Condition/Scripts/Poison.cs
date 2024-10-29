using Arbor;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

// 毒の状態異常処理(伊波)

public class Poison : MonoBehaviour, ICondition
{
    [Header("一回ごとのダメージ割合")]
    [SerializeField, EnumIndex(typeof(ConditionInfo.ResistanceID))]
    public float[] m_poisonDamageRate = new float[(int)ConditionInfo.ResistanceID.ResistanceTypeNum];
    [Header("毒効果の回数")]
    [SerializeField] private int m_poisonCount;
    [Header("毒ダメージの間隔")]
    [SerializeField] private float m_poisonInterval;

    [Header("エフェクト")]
    [SerializeField] private GameObject m_effectAssetPrefab;
    private GameObject m_posionEffect;

    //ShareNode存在時、指定名称の部位からエフェクトを発生させる（山本）
    [Header("ShareNodeに登録された部位の名称")]
    [SerializeField] private string m_parent;


    private ConditionInfo.ConditionID m_conditionID = ConditionInfo.ConditionID.Poison;
    public ConditionInfo.ConditionID ConditionID => m_conditionID;

    private float m_timer = 0.0f;

    public GameObject Owner { get; set; }
    private ConditionManager m_conditionManager;

    public bool IsEffective() { return m_poisonCount > 0; }
    public float DamageMulti(ConditionInfo.ResistanceID[] resistances) { return 1.0f; }
    public void ReplaceCondition(ICondition newCondition, ConditionInfo.ResistanceID[] resistances)
    {
        Poison newPoison = newCondition as Poison;
        if (newPoison == null) return;
        if ((int)m_conditionID < resistances.Length)
        {
            float nowSumDamage = m_poisonCount * m_poisonDamageRate[(int)m_conditionManager.Resistances[(int)m_conditionID]];
            float newSumDamage = newPoison.m_poisonCount * newPoison.m_poisonDamageRate[(int)m_conditionManager.Resistances[(int)m_conditionID]];
            if (nowSumDamage < newSumDamage)
            {
                m_poisonCount = newPoison.m_poisonCount;
                m_poisonDamageRate = newPoison.m_poisonDamageRate;
                m_poisonInterval = newPoison.m_poisonInterval;
            }
        }
    }

    void Start()
    {
        Owner = gameObject;
        gameObject.transform.parent.TryGetComponent(out m_conditionManager);

        
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
                    m_posionEffect = Instantiate(m_effectAssetPrefab, parent);

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
            m_posionEffect = Instantiate(m_effectAssetPrefab, rootTrans);
        }
    }

    public void Update()
    {
        if (!IsEffective())
        {
            return;
        }

        m_timer -= Time.deltaTime;
        if (m_timer > 0.0f) return;

        if (!transform.root.TryGetComponent(out IDamageable damageAble))
        {
            m_poisonCount = 0;
            return;
        }
        if (!transform.root.TryGetComponent(out CharacterCore characterCore))
        {
            m_poisonCount = 0;
            return;
        }
        DamageNotification damageNotification = new DamageNotification();
        damageNotification.m_damage = characterCore.Status.MaxHP * m_poisonDamageRate[(int)m_conditionManager.Resistances[(int)m_conditionID]];

        if (characterCore.Status.m_hp.Value <= damageNotification.m_damage)
        {
            damageNotification.m_damage = characterCore.Status.m_hp.Value - 1.0f;
        }

        //Instantiate(m_effectAssetPrefab, transform.root);
        damageAble.Damaged(damageNotification, null, 0.0f, false);
        m_timer = m_poisonInterval;
        m_poisonCount--;

    }

    private void OnDestroy()
    {
        //毒エフェクトを残さないように削除（山本）
        if (m_posionEffect)
        {
            Destroy(m_posionEffect);
        }

    }
}
