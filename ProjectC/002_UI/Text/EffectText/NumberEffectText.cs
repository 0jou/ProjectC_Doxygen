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

    private enum NumberEffectType
    {
        Ratio = 0,  // 割合
        Fixed = 1,  // 固定
    }

    [Header("エフェクト種類")]
    [SerializeField]
    private NumberEffectType m_numberEffectType = NumberEffectType.Fixed;

    [Header("割合(1/nで計算)")]
    [SerializeField]
    private int m_ratio = 1;

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

            switch (m_numberEffectType)
            {
                case NumberEffectType.Ratio:
                    {
                        await Ratio(targetNum);
                        break;
                    }
                case NumberEffectType.Fixed:
                    {
                        await Fixed(targetNum);
                        break;
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


    private async UniTask Ratio(int _targetNum)
    {
        float value = ((float)_targetNum - (float)m_initialNumber) / m_ratio;

        // 増やす場合
        if (m_initialNumber < _targetNum)
        {
            for (float i = m_initialNumber; i < _targetNum; i += value)
            {
                int currentValue = (int)i;

                // テキストを一文ずつ追加
                string text = currentValue.ToString(m_draftText);
                m_initialNumber = currentValue;

                m_text.text = text;

                // 待機
                await UniTask.DelayFrame(m_delay);
                m_cancellationToken.Token.ThrowIfCancellationRequested();
            }
        }
        // 減らす場合
        else
        {
            for (float i = m_initialNumber; _targetNum < i; i -= value)
            {
                int currentValue = (int)i;

                // テキストを一文ずつ追加
                string text = currentValue.ToString(m_draftText);
                m_initialNumber = currentValue;

                m_text.text = text;

                // 待機
                await UniTask.DelayFrame(m_delay);
                m_cancellationToken.Token.ThrowIfCancellationRequested();
            }
        }
    }


    private async UniTask Fixed(int _targetNum)
    {
        // 増やす場合
        if (m_initialNumber < _targetNum)
        {
            for (int i = m_initialNumber; i < _targetNum; i += m_value)
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
            for (int i = m_initialNumber; _targetNum < i; i -= m_value)
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
    }

}
