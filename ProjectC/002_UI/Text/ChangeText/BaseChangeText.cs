using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Cysharp.Threading.Tasks;

public class BaseChangeText : MonoBehaviour
{
    // テキストを動的に変更する
    // 制作者　田内

    [Header("変更するテキスト")]
    [SerializeField]
    protected TextMeshProUGUI m_text = null;

    [Header("エフェクト")]
    [SerializeField]
    protected List<BaseEffectText> m_effectList = new();

    [Header("DOTween")]
    [SerializeField]
    protected List<BaseDoTweenUI> m_baseDoTweenUIList = new();


    private void Update()
    {
        ChangeText();
    }


    virtual protected void ChangeText()
    {
        Debug.LogError("オーバーライドしてください");
    }


    // エフェクトを使用
    virtual protected void PlayEffect()
    {
        foreach (var e in m_effectList)
        {
            if (e == null) return;
            e.PlayEffect().Forget();
        }
    }

    virtual protected void PlayDoTween()
    {
        foreach(var d in m_baseDoTweenUIList)
        {
            if (d == null) return;
            d.StartDoTween();
        }
    }

}
