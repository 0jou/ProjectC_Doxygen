using StorySkillInfo;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class StorySkillData : ScriptableObject
{
    //ストーリースキルのデータ（山本）

    [Header("ストーリースキルプレハブ")]
    [SerializeField] private GameObject m_storySkillPrefab = null;
    public GameObject StorySkillPrefab { get { return m_storySkillPrefab; } }

    [Header("召喚する際のエフェクト")]
    [SerializeField] private GameObject m_spellEffect = null;
    public GameObject SpellEffectPrefab { get { return m_spellEffect; } }

    [Header("召喚する際に消費するBP")]
    [SerializeField] private float m_payBP = 0.0f;
    public float PayBP { get { return m_payBP; } }

    [Header("召喚にかかる時間(キャストタイム)")]
    [SerializeField] private float m_castTime = .0f;
    public float CastTime { get { return m_castTime; } }

    [Header("滞在時間(攻撃継続時間)")]
    [SerializeField] private float m_stayTime = .0f;
    public float StayTime { get { return m_stayTime; } }

    [Header("プレイヤーからどのくらい離れた位置に生成するか")]
    [SerializeField] private float m_distance = 0.0f;
    public float Distance { get { return m_distance; } }

    [Header("ストーリースキルのID")]
    [SerializeField] private StorySkill_ID m_storySkill_ID = StorySkill_ID.None;
    public StorySkill_ID StorySkill_ID { get { return m_storySkill_ID; } }

    [Header("ストーリースキル名")]
    [SerializeField] private string m_storySkillName = "None";
    public string StorySkillName { get { return m_storySkillName; } }

    [Header("ストーリースキルの紹介文")]
    [SerializeField] private string m_storySkillDescriptionText = "None";
    public string StorySkillDescriptionText { get {  return m_storySkillDescriptionText; } }


}
