using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareNodes : MonoBehaviour
{
    //[SerializeField] SerializedDictionary<string, Transform> m_nodes;
    //public SerializedDictionary<string, Transform> Nodes { get { return m_nodes; } }

    //インスペクター上で追加できるように変更した。（山本）
    [SerializeField] SerializableDictionary<string, Transform> m_nodes;
    public SerializableDictionary<string, Transform> Nodes { get { return m_nodes; } }

}
