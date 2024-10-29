using StorySkillInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharacterCore : MonoBehaviour, IDamageable
{
    //---------------------------------------
    //ブレーメンの音楽隊スキルのState(山本)
    //---------------------------------------

    //登場シーン演出待機中State
    [System.Serializable]
    [AddTypeMenu("Player/SkillBremen/WaitAppearScene")]
    public class ActionState_SkillBremen_WaitAppear : CharacterCore.ActionState_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var parameter = Core.PlayerSkillsParameters;

            if (parameter == null)
            {
                return;
            }

            //出現用Timelineを再生
            parameter.StorySkillAppear();

            //ルートオブジェクトにSetTimeLineコンポーネントがあれば呼ぶ
            if (Core.transform.root.TryGetComponent(out SetTimeLineSpeed setTimeLineSpeed))
            {
                setTimeLineSpeed.SetPlayableDirectorSpeed();
            }

            StorySkill_ID storySkill_ID = parameter.GetStorySkill_ID;
            StorySkillData storySkillData = StorySkillDataBaseManager.instance.GetStorySkillData(storySkill_ID);

            //データベースからスキルの滞在時間を取得する
            Core.PlayerSkillsParameters.DisappearTime = storySkillData.StayTime;

        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            //TimeLine再生終了したら次のStateへと移行
            if (Core.PlayerSkillsParameters.m_playableDirector)
            {
                if (Core.IsDoneTimeLine() == false)
                {
                    Core.m_animator.SetTrigger("Attack");

                    //SetTriggerAnimatorListコンポーネントにアクセスし、その他のAnimatorにトリガーを送る
                    if (Core.transform.TryGetComponent(out SetTriggerInAnimatorList set))
                    {
                        set.SetTriggerAnimatorInList("Attack");
                    }

                }


            }

        }

        public override void OnExit()
        {
            base.OnExit();
        }

    }

    //攻撃&待機状態
    [System.Serializable]
    [AddTypeMenu("Player/SkillBremen/Attack&Idle")]
    public class ActionState_SkillBremen_Attack_Wait : CharacterCore.ActionState_Base
    {
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Core.PlayerSkillsParameters.MinusStayStorySkillTime())
            {
                Core.m_animator.SetTrigger("Finish");

                //SetTriggerAnimatorListコンポーネントにアクセスし、その他のAnimatorにトリガーを送る
                if (Core.transform.TryGetComponent(out SetTriggerInAnimatorList set))
                {
                    set.SetTriggerAnimatorInList("Finish");
                }

            }
        }
    }

    //消滅
    [System.Serializable]
    [AddTypeMenu("Player/SkillBremen/Disappear")]
    public class ActionState_SkillBremen_Disappear : CharacterCore.ActionState_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var parameter = Core.PlayerSkillsParameters;

            if (parameter == null)
            {
                return;
            }

            parameter.StorySkillDisappear();

            //ルートオブジェクトにSetTimeLineコンポーネントがあれば呼ぶ
            if (Core.transform.root.TryGetComponent(out SetTimeLineSpeed setTimeLineSpeed))
            {
                setTimeLineSpeed.SetPlayableDirectorSpeed();
            }


        }


        public override void OnUpdate()
        {
            base.OnUpdate();

            var skillParam = Core.PlayerSkillsParameters;

            if (skillParam)
            {
                if (skillParam.ObserveEffect == null)
                {
                    Destroy(Core.gameObject.transform.root.gameObject);
                }

            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            Core.m_charaCtrl.AddVelocity(Core.transform.forward);


        }


    }



    public bool IsDoneTimeLine()
    {
        var playableDirector = PlayerSkillsParameters.m_playableDirector;

        if (playableDirector == null)
        {
            Debug.LogError("PlayableDirectorが設定されていないです");
            return false;
        }

        bool playFlg = false;

        if (playableDirector.state != UnityEngine.Playables.PlayState.Playing)
        {
            playFlg = false;
        }
        else
        {
            playFlg = true;
        }


        return playFlg;



    }

}



