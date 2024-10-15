using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIconElement : MonoBehaviour
{
    [SerializeField] private Image m_image;
    [SerializeField] private Image m_backImage;

    [SerializeField] private float m_maxShowTime = 1f;

    [SerializeField] private PlaySEOnEnable m_playSE;

    private float m_maxFillTime;
    private float m_fillTime;
    private float m_showTime;

    public void Show(float _fillTime)
    {
        if (m_backImage) m_backImage.fillAmount = 1.0f;
        m_maxFillTime = _fillTime;
        m_fillTime = 0.0f;
        m_showTime = 0.0f;
    }

    public void Hide()
    {
        m_showTime = m_maxShowTime + m_maxFillTime;
        if (m_playSE) m_playSE.enabled = false;
        SetImageFill(0.0f);
    }


    void Start()
    {
        SetImageFill(0.0f);
        m_showTime = m_maxShowTime;

        if (m_playSE) m_playSE.enabled = false;
    }

    void Update()
    {
        if (m_showTime >= m_maxShowTime + m_maxFillTime)
        {
            SetImageFill(0.0f);
            if (m_playSE) m_playSE.enabled = false;
            return;
        }

        m_showTime += Time.deltaTime;
        m_fillTime += Time.deltaTime;

        float rate = m_fillTime / m_maxFillTime;
        float fill = Mathf.Clamp(rate, 0.0f, 1.0f);
        if (rate >= 1.0f)
        {
            if (m_playSE) m_playSE.enabled = true;
        }
        if (m_image) m_image.fillAmount = fill;
    }

    private void SetImageFill(float _fillAmount)
    {
        if (m_image) m_image.fillAmount = _fillAmount;
        if (m_backImage) m_backImage.fillAmount = _fillAmount;
    }
}
