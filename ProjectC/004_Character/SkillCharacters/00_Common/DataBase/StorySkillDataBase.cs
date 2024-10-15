using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StorySkillDataBase : ScriptableObject
{
    //童話スキルのデータベース（山本）
    [Header("童話スキルの種類")]
    [SerializeField] private List<StorySkillData> m_storySkillData = new List<StorySkillData>();

    public List<StorySkillData> StorySkillDataList { get { return m_storySkillData; } }

}
