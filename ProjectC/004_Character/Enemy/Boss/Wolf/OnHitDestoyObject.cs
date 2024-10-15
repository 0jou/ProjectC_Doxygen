/**
* @file OnHitDestroyObject.cs
* @brief 何かにぶつかったら自分を削除する
*/

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

    private void OnCollisionEnter(Collision collision)
    {
        if(!string.IsNullOrEmpty(m_onDestroySEName))
        {
            // 効果音
            SoundManager.Instance.Start3DPlayback(m_onDestroySEName, transform.position);
        }

        Destroy(transform.root.gameObject);
    }
}
