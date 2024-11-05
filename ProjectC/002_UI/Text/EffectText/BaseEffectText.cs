using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;

using UniRx;
using UnityEngine.Events;

public class BaseEffectText : MonoBehaviour
{
    // 制作者　田内
    // テキストエフェクト
    // OnEnableを時や、PlayEffectメソッドを呼び出すことでエフェクトが再生されます

    //=========================================================

    // 対象のテキスト
    [Header("対象テキスト")]
    [SerializeField]
    protected TextMeshProUGUI m_text = null;

    //=========================================================

    [Header("表示間隔(ミリ秒)")]
    [SerializeField]
    protected int m_delay = 2;

    //=========================================================

    private enum PlayEffectType
    {
        Disable,
        Enable,
    }

    [Header("エフェクト再生タイプ")]
    [SerializeField]
    private PlayEffectType m_enableType = PlayEffectType.Enable;

    //=========================================================

    // キャンセル用
    protected CancellationTokenSource m_cancellationToken = null;

    //==================================================
    //                  実行処理
    //==================================================


    private void OnEnable()
    {
        if (m_enableType == PlayEffectType.Enable)
        {
            // 初期エフェクトを再生
            PlayEffect().Forget();
        }
    }

    // 表示処理
    virtual public async UniTask PlayEffect()
    {
        Debug.LogError("オーバーライドしてください");
        await UniTask.CompletedTask;
    }

}
