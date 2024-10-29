using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationEvent_FootStepSound : PlayAnimationEventSound
{
    // 制作者 田内
    // 足音用

    //=======================================
    //          実行処理
    //=======================================

    public void PlayFootSound()
    {
        var volume = GetVolume();
        var pitch = GetPitch();

        SoundManager.Instance.StartPlayback(m_playSound, volume, pitch);
    }


}
