using StorySkillInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimatorEventCreateStorySkillEffect :AnimatorEventCreateObjectInParent
{
    //童話スキルのエフェクト関係を生成する派生クラス（山本）
    public override void OnEvent(Animator animator)
    {
        base.OnEvent(animator);

        if(m_createdInstance.TryGetComponent(out StopEffect stopEffect))
        {
            var charaCore = animator.transform.GetComponentInParent<CharacterCore>();
            if(charaCore)
            {
                StorySkill_ID id = charaCore.PlayerSkillsParameters.GetStorySkill_ID;
                StorySkillData storySkillData= StorySkillDataBaseManager.instance.GetStorySkillData(id);
                stopEffect.WaitTime = storySkillData.StayTime;

                //プレイヤーの監視用の変数に代入する
                if (charaCore.PlayerSkillsParameters)
                charaCore.PlayerSkillsParameters.ObserveEffect = m_createdInstance;

            }
        }
        
    }


}
