using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;


// BaseDoTweenUIより後に実行
[DefaultExecutionOrder(100)]
public class EndToDestroyDoTweenUI : MonoBehaviour
{
    // 制作者 田内
    // DOTweenの動作が全て完了した時にアタッチされたUIを削除する

    private List<Sequence> m_sequencesList = new();

    //============================================
    //              実行処理
    //============================================

    private void Start()
    {
        AddSequence();
    }


    private void Update()
    {
        CheckDestroy();
    }


    private void OnDestroy()
    {
        // メモリリーク対策

        for (int i = 0; i < m_sequencesList.Count; ++i)
        {
            if (m_sequencesList[i] == null) continue;

            m_sequencesList[i].Kill();
            m_sequencesList[i] = null;
        }

        m_sequencesList.Clear();
    }


    private void AddSequence()
    {
        var list = gameObject.GetComponents<BaseDoTweenUI>();

        foreach (var dotween in list)
        {
            if (dotween.Sequence == null) continue;

            m_sequencesList.Add(dotween.Sequence);
        }
    }

    private void CheckDestroy()
    {
        // 再生していなければ取り除く
        m_sequencesList.RemoveAll(_ =>
        {
            if (_ == null) return true;
            if (_.IsActive() == false) return true;
            if (_.IsPlaying() == false) return true;

            return false;
        });


        // 全て終了すれば
        if (m_sequencesList.Count <= 0)
        {
            Destroy(gameObject);
        }
    }


}
