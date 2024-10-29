using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StaminaController : MonoBehaviour, ISerializationCallbackReceiver
{
    [Tooltip("値変えると長さ変わる")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_value = new();

    [Tooltip("色の変化の設定")]
    [SerializeField] private List<HPBarColor> m_barColorList = new List<HPBarColor>();

    [Tooltip("色や長さ変更するImageを設定")]
    [SerializeField] private Image m_image;

    private bool m_isView = true;

    [SerializeField] private float m_showDuration = 0.1f;
    [SerializeField] private float m_hideDuration = 1.3f;

    [SerializeField] private List<Image> m_viewImageList = new();


   private float m_max = 50.0f;
    public float Max { set { m_max = value; } }

    // Start is called before the first frame update
    void Start()
    {
        if (m_image == null)
        {
            Debug.LogError("StaminaController Imageが設定されていません");
        }

        foreach (var image in m_viewImageList)
        {
            image.gameObject.SetActive(false);
        }
        m_isView = false;
    }

    /// <summary>
    /// インスペクターで値を変更した時に呼ばれる（保存前）
    /// </summary>
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        UpdateFillAmount();
    }

    /// <summary>
    /// インスペクターでの値の変更後、保存された値を読み込む際に呼ばれる
    /// </summary>
    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
    }

    private void UpdateFillAmount()
    {
        if (m_image == null) return;

        // 値→長さ
        m_image.fillAmount = m_value;

        // 値→色
        for (int i = 0; i < m_barColorList.Count; i++)
        {
            if (m_value > m_barColorList[i].m_value) continue;

            m_image.color = m_barColorList[i].m_color;
            break;
        }
    }

    // 現在、最大値のセット
    public void SetValue(float _current, float _max)
    {
        m_max = _max;
        SetValue(_current);
    }
    // 現在のHPをセット
    public void SetValue(float _current)
    {
        m_value = _current / m_max;

        // HPバーの更新
        UpdateFillAmount();

    }


    public void OnShow()
    {
        if (m_isView) return;

        foreach (var image in m_viewImageList)
        {
            image.gameObject.SetActive(true);
            image.DOKill(); // すでにアニメーションが再生されていたらキル
            image.DOFade(endValue: 1f, duration: m_showDuration)
                .SetUpdate(true);   //TimeScaleを無視する;

        }
        m_isView = true;

    }

    public void OnHide()
    {
        if (!m_isView) return;

        foreach (var image in m_viewImageList)
        {
            image.DOKill(); // すでにアニメーションが再生されていたらキル
            image.DOFade(endValue: 0f, duration: m_hideDuration)
                .OnComplete(() => image.gameObject.SetActive(false))
                .SetUpdate(true);
        }
        m_isView = false;

    }

}
