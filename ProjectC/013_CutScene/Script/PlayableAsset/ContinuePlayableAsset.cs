using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[System.Serializable]
public class ContinuePlayableAsset : PlayableAsset
{
    //プレイヤーコンティニュー用のPlayableAsset（山本）
    //GameOver用のスクリーンのマテリアルをセット
    [SerializeField] ExposedReference<Image> m_image;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        //ContinuePlayableBehaviourクラスを生成
        ContinuePlayableBehaviour behaviour = new ContinuePlayableBehaviour();

        behaviour.Image = m_image.Resolve(graph.GetResolver());



        return ScriptPlayable<ContinuePlayableBehaviour>.Create(graph, behaviour);
    }
}
