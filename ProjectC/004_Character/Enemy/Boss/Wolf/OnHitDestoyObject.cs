/**
* @file OnHitDestroyObject.cs
* @brief 何かにぶつかったら自分を削除する
*/

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* @brief 何かにぶつかったら自分を削除する　伊波
* @details RootごとGameObjectを消す
*/
public class OnHitDestoyObject : MonoBehaviour
{
    [SerializeField] private string m_onDestroySEName;
    [SerializeField] private GameObject m_brokenEffect;
    [Tooltip("画面揺らしたければ入れる")]
    [SerializeField] private CinemachineImpulseSource m_cameraImpulse;

    private void OnCollisionEnter(Collision collision)
    {
        if(!string.IsNullOrEmpty(m_onDestroySEName))
        {
            // 効果音
            SoundManager.Instance.Start3DPlayback(m_onDestroySEName, transform.position);
        }
        if(m_brokenEffect)
        {
            Instantiate(m_brokenEffect, transform.position, Quaternion.identity);
        }

        if(m_cameraImpulse)
        {
            m_cameraImpulse.GenerateImpulse();
        }

        Destroy(transform.root.gameObject);
    }
}
