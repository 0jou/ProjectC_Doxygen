using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

//AttckAppliciateのOnTriggerStayバージョン。多段攻撃にはこちらを使う(山本)
//追記：判定しすぎでうまくいかない。もう少し改良しないと使い物にはならなそう
public class AttackStayTrigerAppliciate : MonoBehaviour
{
    // 送信する情報
    [SerializeField] private float m_hitStopTime;   // ヒットストップの時間
    [SerializeField] private float m_attack;        // CharacterCoreのAtkに追加する攻撃力
    [SerializeField] private float m_knockBackMultiplier = 1f;  // ノックバックの強さ
    [SerializeField] private bool m_isStrongAttack = false;     // 強攻撃か否か

    // ヒットエフェクト
    [SerializeField] private GameObject m_assetHitEffect;

    // サウンド
    [SerializeField] private SoundData m_soundData;

    [SerializeField] private bool m_doFriendlyFire = false;
    [SerializeField] private bool m_doMultiHit = false;

    [Header("最大当たり判定")]
    [SerializeField]private int m_MaxHitCount = 0;

    [Header("回復するBPの倍率（ダメージにかける）")]
    [SerializeField] private float m_recoverBPMagni = 0.1f;

    [Header("相手に与える状態異常")]
    [SerializeField] private ConditionInfo.ConditionID m_condition = ConditionInfo.ConditionID.Normal;

    private OwnerInfoTag m_ownerInfoTag;
    private List<Collider> m_hittedColliders = new List<Collider>();

    private void Start()
    {
        // オーナー情報
        m_ownerInfoTag = GetComponent<OwnerInfoTag>();
        if (m_ownerInfoTag == null) { return; }

        if (!TryGetComponent(out Collider myCol)) return;

        this.OnTriggerStayAsObservable()
            .Where(_ => enabled)
            .Subscribe(collider =>
            {
                if (m_ownerInfoTag.Characore == null) { return; }
                // 自分との当たり判定なら進まない
                if (collider.transform.root.name == m_ownerInfoTag.Characore.name) return;
                //プレイヤーとストーリースキルキャラクターの攻撃は当たらない（山本）
                if (m_ownerInfoTag.transform.root.tag == "Player" && collider.transform.tag == "SkillCharacter")
                {
                    return;
                }

                GameObject otherObj = collider.gameObject;

                // ダメージをもらえるオブジェクトであれば、処理を行う
                var damageable = otherObj.GetComponent<IDamageable>();
                if (damageable == null) { return; }

                // 仮　攻撃可能でなければ終わり
                if (!m_ownerInfoTag.Characore.DoFriendlyFire &&
                !m_doFriendlyFire &&
                !damageable.IsAttackable(m_ownerInfoTag.GroupNo)) { return; }

                // 多段ヒット判断
                if (!m_doMultiHit && m_hittedColliders.Contains(collider)) { return; }

                DamageNotification damageNotification = new();
                // 与えるダメージ量
                damageNotification.m_damage = m_ownerInfoTag.Characore.Status.m_attack + m_attack;
                // 送信するヒットストップ時間
                damageNotification.m_hitStopTime = m_hitStopTime;


                // ダメージ処理・相手側のヒットストップもこの中で
                damageable.Damaged(damageNotification, myCol, m_knockBackMultiplier, m_isStrongAttack);

                //　当たったなら、当たった際の処理を実行
                if (damageNotification.m_replyIsHit)
                {
                    AddCondition(otherObj);
                    m_hittedColliders.Add(collider);

                    // 自分にヒットストップを適用させる
                    m_ownerInfoTag.Characore.HitStopRemainingTime = m_hitStopTime;

                    // SkillごとにBPを回復する（吉田）
                    {

                        //BPを回復する(山本)
                        if (m_ownerInfoTag.Characore.Status.m_bp.Value < m_ownerInfoTag.Characore.Status.MaxBP)
                            m_ownerInfoTag.Characore.Status.m_bp.Value += damageNotification.m_damage * m_recoverBPMagni;

                        //Max超えてたらMaxに戻す
                        if (m_ownerInfoTag.Characore.Status.m_bp.Value >= m_ownerInfoTag.Characore.Status.MaxBP)
                        {
                            m_ownerInfoTag.Characore.Status.m_bp.Value = m_ownerInfoTag.Characore.Status.MaxBP;
                        }

                        //スキルBPを回復する(吉田)
                        m_ownerInfoTag.Characore.Status.m_bpSkill_1.Value += damageNotification.m_damage * m_recoverBPMagni;
                        m_ownerInfoTag.Characore.Status.m_bpSkill_2.Value += damageNotification.m_damage * m_recoverBPMagni;

                        //Max超えてたらMaxに戻す（吉田）
                        if (m_ownerInfoTag.Characore.Status.m_bpSkill_1.Value >= m_ownerInfoTag.Characore.Status.MaxBPSkill_1)
                        {
                            m_ownerInfoTag.Characore.Status.m_bpSkill_1.Value = m_ownerInfoTag.Characore.Status.MaxBPSkill_1;
                        }
                        if (m_ownerInfoTag.Characore.Status.m_bpSkill_2.Value >= m_ownerInfoTag.Characore.Status.MaxBPSkill_2)
                        {
                            m_ownerInfoTag.Characore.Status.m_bpSkill_2.Value = m_ownerInfoTag.Characore.Status.MaxBPSkill_2;
                        }

                    }

                    // ヒットエフェクト表示
                    if (m_assetHitEffect != null)
                    {
                        //修正：ヒットエフェクトを攻撃判定が当たった場所に表示にする(山本)
                        Instantiate(m_assetHitEffect,
                            position: collider.ClosestPoint(gameObject.transform.position),
                            rotation: Quaternion.identity,
                            parent: null);

                    }

                    if (!string.IsNullOrEmpty(m_soundData.m_soundName))
                    {
                        SoundManager.Instance.Start3DPlayback(m_soundData, transform.parent.gameObject);
                    }
                }
            }
            );
    }

    private void AddCondition(GameObject _enemy)
    {
        if (_enemy == null) return;

        var conditionData = ConditionDataBaseManager.instance.GetConditionData(m_condition);
        if (conditionData == null) return;

        if (conditionData.ConditionPrefab == null) return;
        Transform managerObj = _enemy.transform.Find("ConditionManager");
        if (!managerObj.TryGetComponent(out ConditionManager manager)) return;

        manager.AddCondition(conditionData.ConditionPrefab);

    }

}
