using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public partial class SelectUIController : MonoBehaviour
{
    // 制作者 田内
    // 選択したUIの大きさを調整する

    private Tweener m_tweener = null;
    private RectTransform m_scaleRect = null;
    private Vector3 m_defaultScale = Vector3.one;

    private void OnDestroy()
    {
        if (m_tweener != null)
        {
            m_tweener.Kill();
            m_tweener = null;
        }
    }

    private void DoScale()
    {
        if (m_tweener != null)
        {
            m_tweener.Kill();
            m_tweener = null;
        }

        // デフォルトの大きさに変更
        if (m_scaleRect != null)
        {
            m_scaleRect.localScale = m_defaultScale;
            m_scaleRect = null;
        }


        if (m_currentSelectUIData?.UI == null) return;
        if (m_currentSelectUIData.UI.TryGetComponent<RectTransform>(out var rect) == false) return;


        m_scaleRect = rect;
        m_defaultScale = rect.localScale;


        // 大きさ調整
        m_tweener = rect.DOPunchScale(
            punch: Vector3.one * 0.1f,
            duration: 0.2f,
            vibrato: 1
        ).SetEase(Ease.OutExpo).SetUpdate(true).SetLink(m_currentSelectUIData.UI);
    }

}
