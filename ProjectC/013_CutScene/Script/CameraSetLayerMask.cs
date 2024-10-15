using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetLayerMask : MonoBehaviour
{
    //必ずレイヤーマスクを初回にセットする
    [SerializeField]
   　private LayerMask m_layerMask;

    [Header("カメラで写す対象をまとめたリスト")]
    [SerializeField]
    private SerializableDictionary<string, LayerMask>
        m_dictionaryCameraMasklList;

    void Start()
    {
        SetLayerMask("All");
    }

    public void SetLayerMask(string _name)
    {
        if(m_dictionaryCameraMasklList.TryGetValue(_name, out LayerMask _layer))
        Camera.main.cullingMask = _layer;
        Camera.main.Render();
    }
 
}
