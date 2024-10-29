using StorySkillInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//早めに生成する（山本）
[DefaultExecutionOrder(-100)]
public class StorySkillDataBaseManager : BaseManager<StorySkillDataBaseManager>
{
    //童話スキルデータベースのマネージャー（山本）

    [Header("童話スキルのデータベース")]
    [SerializeField] private StorySkillDataBase m_storySkillDataBase;

    public StorySkillDataBase DataBase { get {return m_storySkillDataBase; } }

    //-------------------------------------------------------------------------

    //童話スキルIDの対応データを返す

    public StorySkillData GetStorySkillData(StorySkill_ID _id)
    {
        if(m_storySkillDataBase==null)
        {
            Debug.LogError("童話スキルのデータベースが空です！");
            return null;
        }

        //返すデータ
        StorySkillData storySkillData = null;


        foreach (var data in m_storySkillDataBase.StorySkillDataList)
        {
            
            //データのIDと引数のIDが一致すればそれを返す
            if(data.StorySkill_ID==_id)
            {
                storySkillData= data;
                break;
            }

            
        }

        //ストーリースキル空だったら（探して見つからなかったら）
        if(storySkillData==null)
        {
            Debug.LogError("指定したIDの童話スキルは登録されてません。");
            return null;
        }


        return storySkillData;

    }

}
