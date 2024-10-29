using CriWare;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// インスタントで再生するサウンドを再生するクラス
/// 再生終了を検知して自動的にオブジェクトを破棄する
/// 主に座標指定での鳴らすだけのサウンドに対して使用する想定。
/// </summary>
public class Instant3DSoundPlayer : MonoBehaviour
{
    private CriAtomSource m_criSource = null;
    public CriAtomSource CriSource { set { m_criSource = value; } }

    // Update is called once per frame
    void Update()
    {
        DestroyOnSEEnd();
    }

    /// <summary>
    /// サウンドの再生終了時自動でオブジェクトを破棄する
    /// </summary>
    private void DestroyOnSEEnd()
    {
        if (m_criSource == null)
        {
            m_criSource = gameObject.GetComponent<CriAtomSource>();
            return;
        }
        if (m_criSource.source.IsDestroyable())
        {
            m_criSource.source.Dispose();
            Destroy(gameObject);
        }
    }

}
