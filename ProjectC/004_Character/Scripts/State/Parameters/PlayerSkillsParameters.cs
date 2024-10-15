using StorySkillInfo;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayerSkillsParameters : MonoBehaviour
{
    //スキルID(山本)-----------------------------------------------
    [Header("スキルID")]
    [SerializeField]
    private StorySkill_ID m_storySkill_ID = StorySkill_ID.None;
    public StorySkill_ID GetStorySkill_ID { get { return m_storySkill_ID; } }


    //使用するPlayableDirector（山本）------------------------------
    [Header("PlayableDirector")]
    [SerializeField]
    public PlayableDirector m_playableDirector = null;


    //エフェクト監視用変数(山本)-----------------------------------
    [Header("エフェクト監視用変数")]
    [SerializeField]
    private GameObject m_observeEffect = null;
    public GameObject ObserveEffect { get { return m_observeEffect; } set { m_observeEffect = value; } }


   
    //スキル用変数(山本)-------------------------------------------
    [Header("ターゲットへ照準を合わせる際のスピード")]
    [SerializeField]
    private float m_moveLookTargetSpeed = 10.0f;
    public float MoveLookTargetSpeed { get { return m_moveLookTargetSpeed; } }

    [Header("スキルの消失時間")]
    [SerializeField]
    private float m_disapearTime = 0.0f;
    public float DisappearTime { get { return m_disapearTime; } set { m_disapearTime = value; } }
    public bool MinusStayStorySkillTime()
    {
        if (m_disapearTime > 0.0f)
        {
            m_disapearTime -= Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }

    [Header("スキル出現タイムライン")]
    [SerializeField] private TimelineAsset m_appearTimelineAsset = null;

    //出現タイムライン再生
    public bool StorySkillAppear()
    {
        if (m_playableDirector)
        {
            if (m_appearTimelineAsset)
                m_playableDirector.Play(m_appearTimelineAsset);

            return true;
        }

        return false;
    }

    [Header("スキル消失タイムライン")]
    [SerializeField] private TimelineAsset m_disappearTimelineAsset = null;

    //消失タイムライン再生
    public bool StorySkillDisappear()
    {
        if (m_playableDirector)
        {
            if (m_disappearTimelineAsset)
                m_playableDirector.Play(m_disappearTimelineAsset);

            return true;
        }

        return false;
    }


    [Header("Effect保存用オブジェクト")]
    private GameObject m_offsetEffect = null;
    public GameObject OffsetEfffect { get { return m_offsetEffect; } set { m_offsetEffect = value; } }


    [Header("追従キャラクターが走りに移行する距離")]
    [SerializeField] private float m_runDist = 10.0f;
    public float RunDist { get { return m_runDist; } }

    [Header("追従キャラクターが歩きに移行する距離")]
    [SerializeField] private float m_walkDist = 3.0f;
    public float WalkDist { get { return m_walkDist; } }

    [Header("PathFinding")]
    [SerializeField] private PathFinding m_pathFinding = null;
    public void SwitchPathfinding(bool flg)
    {
        if (m_pathFinding)
        {
            m_pathFinding.enabled = flg;
        }
    }

    public void PathfindingStop()
    {
        if (m_pathFinding)
        {
            m_pathFinding.Stop();
        }
    }

    //スキルの開始地点
    private Vector3 m_startSkillPos = Vector3.zero;
    public Vector3 StartSkillPos
    {
        get { return m_startSkillPos; }
        set { m_startSkillPos = value; }
    }

    //ターゲットのTransform情報
    private Vector3 m_targetPosition = Vector3.zero;
    public Vector3 TargetPosition
    { get { return m_targetPosition; } set { m_targetPosition = value; } }

    //スキルの攻撃回数
    private int m_attackCount = 0;
    public int AttackCount { get { return m_attackCount; } set { m_attackCount = value; } }
}
