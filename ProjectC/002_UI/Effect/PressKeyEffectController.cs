using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressKeyEffectController : MonoBehaviour
{

    [SerializeField]
    private Image m_image = null;
    private Material m_material = null;

    [SerializeField]
    private float m_duration = 1.0f;

    [SerializeField]
    private Vector3 m_minScale = new Vector3(0.9f, 0.9f, 0.9f);

    [SerializeField]
    private Vector3 m_maxScale = new Vector3(1.1f, 1.1f, 1.1f);


    private bool m_playing = false;

    // Start is called before the first frame update
    void Start()
    {
        m_material = m_image.material;

        End();
    }

    [Button("Play")]
    public void Play()
    {
        if (m_playing) return;

        m_playing = true;
        m_image.color = new Color(1, 1, 1, 1);
        transform.localScale = m_minScale;

        // Alpha
        m_image.DOFade(0, m_duration)
            .OnComplete(End);

        // Scale
        transform.DOScale(m_maxScale, m_duration);

    }

    private void End()
    {
        m_playing = false;
        m_image.color = new Color(1, 1, 1, 0);
        transform.localScale = m_minScale;
    }
}
