using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySEOnEnable : MonoBehaviour
{
    [SerializeField] string m_keyWord = string.Empty;
    [SerializeField] bool m_play3D = false;
    [SerializeField, Range(0.0f, 5.0f)] float m_volume=1.0f;

    private void OnEnable()
    {
        if(m_play3D)
        {
            SoundManager.Instance.Start3DPlayback(m_keyWord, transform.position, m_volume);
        }
        else
        {
            SoundManager.Instance.StartPlayback(m_keyWord, m_volume);
        }
    }
}
