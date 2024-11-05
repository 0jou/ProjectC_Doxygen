using StorySkillInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaintsField;

[System.Serializable]
public class SkillAnimeEventAttck : AnimatorEvents.EventNodeBase
{
    //スキルアニメ用（山本作成）
    // 発生させるプレハブ
    [Header("発生させるプレハブ")]
    [SerializeField] GameObject m_assetAttack;

    [SerializeField]
    private StorySkill_ID m_storySkillID;

    [SerializeField, Tooltip("何番目の攻撃か")]
    private int m_attackType;


    // 時間が来たときに実行
    public override void OnEvent(Animator animator)
    {
        var charaCore = animator.transform.GetComponentInParent<CharacterCore>();
        var targetVec = charaCore.PlayerSkillsParameters.TargetPosition - charaCore.PlayerSkillsParameters.StartSkillPos;

        Quaternion targetQua = Quaternion.LookRotation(targetVec, charaCore.transform.up);

        //新しいオブジェクトをシーン内に配置する（親子関係だと親の回転に影響してしまう）
        //登録したスキル開始地点から発射
        var newObj = GameObject.Instantiate(m_assetAttack, charaCore.PlayerSkillsParameters.StartSkillPos, targetQua);

        // 作成者情報を記憶
        var ownerInfo = newObj.AddComponent<OwnerInfoTag>();
        ownerInfo.GroupNo = charaCore.GroupNo;
        ownerInfo.Characore = charaCore;

        var attackApplicant = newObj.AddComponent<AttackApplicant>();
        attackApplicant.SetAttackData(StorySkillDataBaseManager.instance.GetStorySkillData(m_storySkillID).AttackDamageDatas[m_attackType - 1]);
    }

    //public override void OnExit(Animator animator)
    //{
    //}
}
