using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 一定時間後にオブジェクトを消す(倉田)
public class DestroyOnTime : MonoBehaviour
{
    // 消える時間
    [SerializeField] float m_time;

    private void Start()
    {
        Destroy(gameObject, m_time);
    }
}
