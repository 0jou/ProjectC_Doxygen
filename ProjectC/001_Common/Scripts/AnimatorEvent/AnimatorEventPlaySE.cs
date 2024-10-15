/**
* @file AnimatorEventPlaySE.cs
* @brief AnimatorからSEを流す　伊波
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/**
* @brief AnimatorからSEを流す　伊波
*/
public class AnimatorEventPlaySE : AnimatorEvents.EventNodeBase
{
    [Header("サウンドの情報")]
    [SerializeField] private SoundData m_SEData;

    [Header("3Dサウンドかどうか")]
    [SerializeField] private bool m_is3DSound = false;

    public override void OnEvent(Animator animator)
    {
        if (!string.IsNullOrEmpty(m_SEData.m_soundName))
        {
            if(m_is3DSound)
            {
                SoundManager.Instance.Start3DPlayback(m_SEData, animator.gameObject);
            }
            else
            {
                SoundManager.Instance.StartPlayback(m_SEData);
            }
        }
    }
}