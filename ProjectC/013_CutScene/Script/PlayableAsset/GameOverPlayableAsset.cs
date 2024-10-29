using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[System.Serializable]
public class GameOverPlayableAsset : PlayableAsset
{
    //GameOverTimelineで使用する（山本）
    //GameOver用のスクリーンのマテリアルをセット
    [SerializeField] ExposedReference<Image> m_image;
    

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        //GameOverPlayableBehaviourクラスを生成
        GameOverPlayableBehaviour behaviour = new GameOverPlayableBehaviour();

        behaviour.Image = m_image.Resolve(graph.GetResolver());

       

        return ScriptPlayable<GameOverPlayableBehaviour>.Create(graph, behaviour);
    }
}
