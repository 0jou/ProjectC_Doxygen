using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Value に応じて色を変更する
/// </summary>
public class ChangeColorListForValue : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_value = 0.0f;

    [System.Serializable]
    private class ValueColor
    {
        public float m_value;
        public Color m_color;
    }

    [SerializeField]
    private List<ValueColor> m_colorList = new List<ValueColor>();



}
