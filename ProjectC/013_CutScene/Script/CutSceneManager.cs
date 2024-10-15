using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public enum CutSceneNumber
{
    none,
    gameOver,
    reStart,
    max
}

public class CutSceneManager : BaseManager<CutSceneManager>
{
    //カットシーンにアクセスするためのマネージャークラス（山本）
    [SerializeField] private PlayableDirector m_playableDirector = null;
    [SerializeField] private SerializableDictionary<CutSceneNumber, TimelineAsset> m_dictionaryTimeline;


    public bool PlayCutScene(CutSceneNumber _cutSceneNumber)
    {
        if (!m_playableDirector)
        {
            Debug.LogError("PlayableDirectorが登録されてない");
            return false;
        }

        switch (_cutSceneNumber)
        {
            case CutSceneNumber.gameOver:
                {
                    if (m_dictionaryTimeline.TryGetValue(_cutSceneNumber, out TimelineAsset asset))
                    {
                        m_playableDirector.Play(asset);
                        //WrapModeをHoldに変更
                        m_playableDirector.extrapolationMode = DirectorWrapMode.Hold;

                        return true;
                    }
                    else
                    {
                        Debug.LogError("タイムラインアセットが登録されてない");
                    }
                }
                break;

            case CutSceneNumber.reStart:
                {
                    if (m_dictionaryTimeline.TryGetValue(_cutSceneNumber, out TimelineAsset asset))
                    {
                        m_playableDirector.Play(asset);
                        //WrapModeをNoneに変更
                        m_playableDirector.extrapolationMode = DirectorWrapMode.None;

                        return true;
                    }
                    else
                    {
                        Debug.LogError("タイムラインアセットが登録されてない");
                    }
                }
                break;

            default:
                break;

        }

        return false;
    }

    public bool IsCutScenePlay()
    {
        if (!m_playableDirector)
        {

            Debug.LogError("PlayableDirectorがセットされてないです。");
            return false;
        }

        //シーンマネージャーにセットされたディレクターが再生中ならTrue
        if (m_playableDirector.state == PlayState.Playing)
        {
            return true;
        }
        else { return false; }

    }

}
