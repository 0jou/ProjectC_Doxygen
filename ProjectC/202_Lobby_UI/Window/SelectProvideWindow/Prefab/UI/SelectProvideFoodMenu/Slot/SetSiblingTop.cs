using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSiblingTop : MonoBehaviour
{
    // 制作者 田内
    // 常に一番最後(上)に表示する

    [Header("自身のRectTransform")]
    [SerializeField]
    private RectTransform m_rectTransform = null;

    [Header("親のRectTransform")]
    [SerializeField]
    private RectTransform m_parent = null;

    //===========================================
    // 実行処理
    //===========================================

    private void Update()
    {
        Set();
    }


    private void Set()
    {
        #region nullチェック

        if (m_rectTransform==null)
        {
            Debug.LogError("自身のRectTransformがシリアライズされていません");
            return;
        }


        if (m_parent== null)
        {
            Debug.LogError("親のRectTransformがシリアライズされていません");
            return;
        }
        #endregion

        m_rectTransform.SetSiblingIndex(m_parent.childCount - 1);

    }

}
