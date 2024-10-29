using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public class PlayAnimationEventSound : MonoBehaviour
{
    // 制作者 田内
    // 足音を再生する
    // モーションデータのイベントで再生する

    //==============================================================

    [Header("再生するSoundName")]
    [SerializeField]
    protected string m_playSound = "";

    //==================================

    [BoxGroup("Volume")]
    [Header("ボリューム")]
    [SerializeField]
    protected float m_volume = 1.0f;

    protected enum VolumeType
    {
        Disable,
        Enable,
    }

    [BoxGroup("Volume")]
    [Header("ピッチ変更")]
    [SerializeField]
    protected VolumeType m_volumeType = VolumeType.Disable;

    [BoxGroup("Volume")]
    [EnableIf("m_volumeType", VolumeType.Enable)]
    [SerializeField]
    protected Vector2 m_randomVolume = new Vector2(-0.1f, 0.1f);


    //==================================

    [BoxGroup("Pitch")]
    [Header("ピッチ")]
    [SerializeField]
    protected float m_pitch = 0.0f;

    protected enum PitchType
    {
        Disable,
        Enable,
    }

    [BoxGroup("Pitch")]
    [Header("ピッチ変更")]
    [SerializeField]
    protected PitchType m_pitchType = PitchType.Disable;

    [BoxGroup("Pitch")]
    [EnableIf("m_pitchType", PitchType.Enable)]
    [SerializeField]
    protected Vector2 m_randomPitch = new Vector2(-2400.0f, 2400.0f);

    //========================================
    //              実行処理
    //========================================


    protected float GetVolume()
    {

        float volume = m_volume;

        switch (m_volumeType)
        {
            case VolumeType.Enable:
                {
                    volume += Random.Range(m_randomVolume.x, m_randomVolume.y);
                    break;
                }
            default:
                {
                    break;
                }

        }

        return volume;
    }


    protected float GetPitch()
    {
        float pitch = m_pitch;

        switch (m_pitchType)
        {
            case PitchType.Enable:
                {
                    pitch += Random.Range(m_randomPitch.x, m_randomPitch.y);
                    break;
                }
            default:
                {
                    break;
                }

        }

        return pitch;
    }


}
