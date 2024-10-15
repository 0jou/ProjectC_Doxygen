using UnityEngine;
using CriWare;

public class CuePlay : MonoBehaviour
{
    [SerializeField] string m_keyWord = string.Empty;

    [ContextMenu("Play")]
    void Play()
    {
        SoundManager.Instance.StartPlayback(m_keyWord);
    }

    [ContextMenu("3DPlay")]
    void Play3D()
    {
        SoundManager.Instance.Start3DPlayback(m_keyWord,transform.position);
    }

    [ContextMenu("BGMPlay")]
    void PlayBGM()
    {
        SoundManager.Instance.StartBGM(m_keyWord);
    }

}
