using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

public class TypeWriteEffectText : BaseEffectText
{
    // 徐々にテキストを表示していく
    // 制作者　田内

    //==================================================
    //                  実行処理
    //==================================================


    override public async UniTask PlayEffect()
    {
        if (m_text == null)
        {
            Debug.LogError("対象のテキストが存在しません");
            return;
        }

        if (m_cancellationToken != null)
        {
            // 操作中に再度この処理が通った場合既に通っている処理をキャンセルする
            m_cancellationToken.Cancel();
        }

        m_cancellationToken = new();
        var ct = m_cancellationToken.Token;

        try
        {
            // 一度保持
            string displayText = m_text.text;

            // 初期化
            m_text.text = "";

            for (int i = 0; i < displayText.Length; ++i)
            {
                // テキストを一文ずつ追加
                string text = m_text.text + displayText[i];

                m_text.text = text;

                // 待機
                await UniTask.DelayFrame(m_delay);
                ct.ThrowIfCancellationRequested();
            }

            ct.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }


}
