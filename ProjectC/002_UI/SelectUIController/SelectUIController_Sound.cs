using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public partial class SelectUIController : MonoBehaviour
{
    // 制作者 田内
    // 音声再生

    [Header("SelectSE")]
    [SerializeField]
    private string m_selectSoundName = "Select";


    //===========================================
    //               実行処理
    //===========================================

    protected void PlaySelectSound()
    {
        try
        {
            SoundManager.Instance.StartPlayback(m_selectSoundName);
        }
        catch
        {

        }
    }


    protected void PlayPressSound()
    {
        try
        {
            if (m_currentSelectUIData == null) return;
            SoundManager.Instance.StartPlayback(m_currentSelectUIData.PressSoundPath);
        }
        catch
        {

        }
    }

}
