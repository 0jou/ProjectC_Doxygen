using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SetTimeLineSpeed : MonoBehaviour
{
    //Timeline速度を変更するスクリプト（山本）

    [SerializeField]
    private PlayableDirector m_playableDirector;

    [SerializeField]
    private float m_speed = 0.0f;

    private void Awake()
    {
        //if(m_playableDirector==null)
        //{
        //    return;
        //}

        //m_playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(m_speed);  
    }

    //PlayableDirectorの再生スピードを変更する
    public void SetPlayableDirectorSpeed()
    {
        if (m_playableDirector == null)
        {
            return;
        }

        m_playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(m_speed);
    }


}
