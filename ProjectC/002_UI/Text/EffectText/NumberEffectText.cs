using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberEffectText : BaseEffectText
{
    // 制作者 田内
    // 徐々に値を変化させるエフェクトテキスト

    [Header("下書き文字列")]
    [SerializeField]
    private string m_draftText = "0000";

    [Header("減増値")]
    [SerializeField]
    private int m_value = 1;

    // 保存用
    [Header("初期値")]
    [SerializeField]
    private int m_initialNumber = 0;


    //========================================
    //              実行処理
    //========================================


    public override async UniTask PlayEffect()
    {
        if (m_text == null)
        {
            Debug.LogError("対象のテキストが存在しません");
            return;
        }

        if (m_cancellationToken != null)
        {
            // 操作中に再度この処理が通った場合既に通っている処理を終了
            m_cancellationToken.Cancel();
            m_cancellationToken = null;
        }

        // 新規作成
        m_cancellationToken = new();

        try
        {
            // 数字に変換
            if (int.TryParse(m_text.text, out int targetNum) == false) return;

            // 増やす場合
            if (m_initialNumber < targetNum)
            {
                for (int i= m_initialNumber;i < targetNum; i += m_value)
                {
                    // テキストを一文ずつ追加
                    string text = i.ToString(m_draftText);
                    m_initialNumber = i;

                    m_text.text = text;

                    // 待機
                    await UniTask.DelayFrame(m_delay);
                    m_cancellationToken.Token.ThrowIfCancellationRequested();
                }
            }
            // 減らす場合
            else
            {
                for (int i = m_initialNumber; targetNum < i; i -= m_value)
                {
                    // テキストを一文ずつ追加
                    m_text.text = i.ToString(m_draftText);
                    m_initialNumber = i;

                    // 待機
                    await UniTask.DelayFrame(m_delay);
                    m_cancellationToken.Token.ThrowIfCancellationRequested();
                }
            }

            // ターゲットの値をセット
            m_text.text = targetNum.ToString(m_draftText);

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;

    }


}
