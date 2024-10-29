using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundWidthController : MonoBehaviour
{

    [SerializeField]
    [Range(0, 2000)]
    [Label("背景の幅")]
    private float m_width = 100;

    [SerializeField]
    [Label("左側")]
    private RectTransform m_left;

    [SerializeField]
    [Label("真ん中")]
    private RectTransform m_mid;

    private float m_midMinWidth = 0.0f;

    [SerializeField]
    [Label("右側")]
    private RectTransform m_right;

    [SerializeField]
    private Image m_leftImage;// 小さくなった時に使う

    [SerializeField]
    private Image m_rightImage;// 小さくなった時に使う


    // Start is called before the first frame update
    void Start()
    {
        // nullがあったら enabled = false
        if (NullCheck())
        {
            enabled = false;
            return;
        }

        m_midMinWidth = m_left.rect.width + m_right.rect.width;

        Center();
    }

    private void Center()
    {
        float midWidthRate = 0;

        // 真ん中がない時
        if (m_width < m_midMinWidth)
        {
            // 真ん中　幅を設定 0に
            m_mid.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);

            midWidthRate = Mathf.Abs(m_width / m_midMinWidth);

            // 右側　位置を設定
            m_rightImage.fillAmount = midWidthRate;
            float rightPosX =
                (m_right.rect.width / 2.0f) - m_right.rect.width * (1 - midWidthRate);
            m_right.anchoredPosition = new Vector2(rightPosX, m_right.anchoredPosition.y);

            // 左側　位置を設定
            m_leftImage.fillAmount = midWidthRate;
            float leftPosX =
                -(m_left.rect.width / 2.0f) + m_left.rect.width * (1 - midWidthRate);
            m_left.anchoredPosition = new Vector2(leftPosX, m_left.anchoredPosition.y);
        }
        // 真ん中がある時
        else
        {
            // 真ん中　幅を設定 場所固定
            float midWidth = m_width - m_left.rect.width - m_right.rect.width;
            m_mid.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, midWidth);

            // 右側　位置を設定
            float rightPosX =
                m_mid.anchoredPosition.x + midWidth / 2.0f + m_right.rect.width / 2.0f;
            m_right.anchoredPosition = new Vector2(rightPosX, m_right.anchoredPosition.y);
            m_rightImage.fillAmount = 1;

            // 左側　位置を設定
            float leftPosX =
                m_mid.anchoredPosition.x - midWidth / 2.0f - m_left.rect.width / 2.0f;
            m_left.anchoredPosition = new Vector2(leftPosX, m_left.anchoredPosition.y);
            m_leftImage.fillAmount = 1;

        }
    }

    // nullがあったらtrueを返す
    private bool NullCheck()
    {

        // nullチェック
        bool isNull = false;
        if (m_left == null)
        {
            UnityEngine.Debug.LogError("左側のRectTransformが設定されていません");
            isNull = true;
        }
        if (m_leftImage == null)
        {
            UnityEngine.Debug.LogError("左側のImageが設定されていません");
            isNull = true;
        }

        if (m_mid == null)
        {
            UnityEngine.Debug.LogError("真ん中のRectTransformが設定されていません");
            isNull = true;
        }

        if (m_right == null)
        {
            UnityEngine.Debug.LogError("右側のRectTransformが設定されていません");
            isNull = true;
        }
        if (m_rightImage == null)
        {
            UnityEngine.Debug.LogError("右側のImageが設定されていません");
            isNull = true;
        }

        return isNull;
    }

}
