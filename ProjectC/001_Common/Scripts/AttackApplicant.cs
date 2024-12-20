using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using FoodInfo;
using IngredientInfo;
using ItemIDEditor;
using ItemInfo;
using StorySkillInfo;


// (仮)当たったらダメージを与えるオブジェクト(倉田)
public class AttackApplicant : MonoBehaviour
{
    private AttackDamageData m_attackDamageData;

    private OwnerInfoTag m_ownerInfoTag;
    private List<Collider> m_hittedColliders = new List<Collider>();

    public void SetAttackData(AttackDamageData data)
    {
        if(data==null)
        {
            Debug.LogError("攻撃のデータが取れませんでした。AnimatorのAnimEventAttackの設定か、データベースを調べてください" + gameObject.name);
        }
        m_attackDamageData = data;
    }

    private void Start()
    {
        // オーナー情報
        m_ownerInfoTag = GetComponent<OwnerInfoTag>();
        if (m_ownerInfoTag == null) { return; }

        if (!TryGetComponent(out Collider myCol)) return;

        this.OnTriggerEnterAsObservable()
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
                !m_attackDamageData.DoFriendlyFire &&
                !damageable.IsAttackable(m_ownerInfoTag.GroupNo)) { return; }

                // 多段ヒット判断
                if (!m_attackDamageData.DoFriendlyFire && m_hittedColliders.Contains(collider)) { return; }


                DamageNotification damageNotification = new();
                // 与えるダメージ量
                damageNotification.m_damage = m_ownerInfoTag.Characore.Status.m_attack + m_attackDamageData.Attack;
                // 送信するヒットストップ時間
                damageNotification.m_hitStopTime = m_attackDamageData.HitStopTime;


                // ダメージ処理・相手側のヒットストップもこの中で
                damageable.Damaged(damageNotification, myCol, m_attackDamageData.KnockBackMultiplier, m_attackDamageData.IsStrongAttack);

                //　当たったなら、当たった際の処理を実行
                if (damageNotification.m_replyIsHit)
                {
                    AddCondition(otherObj);
                    m_hittedColliders.Add(collider);

                    // 自分にヒットストップを適用させる
                    m_ownerInfoTag.Characore.HitStopRemainingTime = m_attackDamageData.HitStopTime;

                    // SkillごとにBPを回復する（吉田）
                    if (m_ownerInfoTag.Characore.PlayerParameters)
                    {
                        PlayerStatus status = m_ownerInfoTag.Characore.PlayerParameters.PlayerStatus;
                        //BPを回復する(山本)
                        if (status.m_bp.Value < status.MaxBP)
                            status.m_bp.Value += damageNotification.m_damage * m_attackDamageData.RecoverBPMagni;

                        //Max超えてたらMaxに戻す
                        if (status.m_bp.Value >= status.MaxBP)
                        {
                            status.m_bp.Value = status.MaxBP;
                        }

                        //スキルBPを回復する(吉田)
                        status.m_bpSkill_1.Value += damageNotification.m_damage * m_attackDamageData.RecoverBPMagni;
                        status.m_bpSkill_2.Value += damageNotification.m_damage * m_attackDamageData.RecoverBPMagni;

                        //Max超えてたらMaxに戻す（吉田）
                        if (status.m_bpSkill_1.Value >= status.MaxBPSkill_1)
                        {
                            status.m_bpSkill_1.Value = status.MaxBPSkill_1;
                        }
                        if (status.m_bpSkill_2.Value >= status.MaxBPSkill_2)
                        {
                            status.m_bpSkill_2.Value = status.MaxBPSkill_2;
                        }
                    }

                    // ヒットエフェクト表示
                    if (m_attackDamageData.AssetHitEffect != null)
                    {
                        //修正：ヒットエフェクトを攻撃判定が当たった場所に表示にする(山本)
                        Instantiate(m_attackDamageData.AssetHitEffect,
                            position: collider.ClosestPoint(gameObject.transform.position),
                            rotation: Quaternion.identity,
                            parent: null);
                    }

                    if (!string.IsNullOrEmpty(m_attackDamageData.SoundData.m_soundName))
                    {
                        SoundManager.Instance.Start3DPlayback(m_attackDamageData.SoundData, transform.position);
                    }
                }
            }
            );
    }

    private void AddCondition(GameObject _enemy)
    {
        if (_enemy == null) return;

        var conditionData = ConditionDataBaseManager.instance.GetConditionData(m_attackDamageData.ConditionID);
        if (conditionData == null || conditionData.ConditionPrefab == null) return;

        Transform managerObj = _enemy.transform.root.Find("ConditionManager");
        if (managerObj == null) return;

        if (managerObj.TryGetComponent(out ConditionManager manager))
        {
            manager.AddCondition(conditionData.ConditionPrefab, true);
        }
    }
}
