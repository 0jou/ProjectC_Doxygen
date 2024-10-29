using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class StartPlayerPlayableAsset : PlayableAsset
{
    [SerializeField]
    ExposedReference<GameObject> charaObj;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var behavior = new StartPlayerPlayableBehaviour();
        behavior.charaObj = charaObj.Resolve(graph.GetResolver());

        return ScriptPlayable<StartPlayerPlayableBehaviour>.Create(graph,behavior);
    }
}
