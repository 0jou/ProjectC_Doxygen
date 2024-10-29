using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorySkillUIController : MonoBehaviour, ISerializationCallbackReceiver
{

    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_value = new();

    [SerializeField] private Image m_image;

    public float m_max = 50.0f;

    // 現在のHP、最大値、最小値もセット
    public void SetValue(float _current, float _max)
    {
        m_max = _max;
        SetValue(_current);
    }

    // 現在のHPをセット
    public void SetValue(float _currentHealth)
    {
        m_value = _currentHealth / m_max;

        // バーの更新
        UpdateFillAmount();
    }


    // Update is called once per frame
    void Update()
    {
        UpdateFillAmount();
    }

    public void UpdateFillAmount()
    {
        if (m_image == null) return;

        m_image.fillAmount = m_value;
    }

    // インスペクターで値を変更した時に呼ばれる（保存前）
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        UpdateFillAmount();
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
    }
}
