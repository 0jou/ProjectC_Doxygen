using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareOnTrigger : MonoBehaviour
{
    // 当たり判定を共有するクラス
    // 制作者　田内

    [Header("タグ")]
    [SerializeField]
    [Tag]
    private string m_tag = "Player";

    //=====================================================
    // m_tagのオブジェクトと当たっているか

    private bool m_isTrigger = false;

    public bool IsTrigger { get { return m_isTrigger; } }

    //=====================================================

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(m_tag))
        {
            m_isTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(m_tag))
        {
            m_isTrigger = false;
        }
    }


}
