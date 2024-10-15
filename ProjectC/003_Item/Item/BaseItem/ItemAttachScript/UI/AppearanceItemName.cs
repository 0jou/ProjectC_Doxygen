using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class AppearanceItemName : MonoBehaviour
{

    [Header("変更するTextMeshPro")]
    [SerializeField]
    private TextMeshProUGUI m_textMeshPro = null;

    // 以下の変数はGetItemObjectで同様の処理を行っているため、
    // このスクリプトの変数はシリアライズフィールドしない（吉田）

    //[Header("DOスピード")]
    //[SerializeField]
    [Range(0.0f, 1.0f)]
    protected float m_doSpead = 0.5f;

    //[SerializeField]
    private GetItemObject m_getItemObject = null;


    private void Start()
    {
        if (m_textMeshPro == null)
        {
            Debug.LogError("テキストが存在しません");
            return;
        }
       
        m_textMeshPro.alpha = 0.0f;
    }


    private void Update()
    {
        // GetItemObjectで同様の処理を行っているため、ここでの処理は不要（吉田）
        // Fade();
    }



    private void Fade()
    {
        if (m_getItemObject == null)
        {
            Debug.LogError("GetItemObjectコンポーネントが登録されていません");
            return;
        }

        if (m_getItemObject.IsGet)
        {

            if (m_textMeshPro.alpha < 1.0f)
            {
                m_textMeshPro.DOFade(endValue: 1.0f, duration: m_doSpead);
            }
        }
        else
        {
            if (m_textMeshPro.alpha > 0.0f)
            {
                m_textMeshPro.DOFade(endValue: 0.0f, duration: m_doSpead);
            }
        }



    }


    public void OnShow(float _duration)
    {
        m_textMeshPro.DOFade(endValue: 1.0f, duration: _duration);
    }

    public void OnHide(float _duration)
    {
        m_textMeshPro.DOFade(endValue: 0.0f, duration: _duration);
    }


}
