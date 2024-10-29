using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class CameraLayreMaskChangePlayableAsset : PlayableAsset
{
    //カメラのレイヤーマスクを変更する処理（山本）
    [Header("写す対象")]
    [SerializeField]
    LayerMask m_layerMask;




    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        //レイヤーマスク変更
        Camera.main.cullingMask = m_layerMask;

        return Playable.Create(graph);
    }
}
