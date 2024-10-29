using CriWare;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlayVolumeTest : MonoBehaviour
{
    public CriAtomSource m_bgmCriAtomSource;
    public string m_categoryName;
    private Slider m_slider;

    private void Start()
    {
        m_slider=GetComponentInChildren<Slider>();

        if (m_slider)
        {
            m_slider.value = PlayerPrefs.GetFloat(m_categoryName+"Vol");
        }
    }

    public void PlaySound()
    {
        if (m_bgmCriAtomSource == null) return;
        if(m_bgmCriAtomSource.status==CriAtomSource.Status.Playing)
        {
            m_bgmCriAtomSource.Stop();
        }
        else
        {
            m_bgmCriAtomSource.Play();
        }
    }

    public void SetVolume()
    {
        CriAtom.SetCategoryVolume(m_categoryName,m_slider.value);
    }

}
