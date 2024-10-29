using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicatorController : MonoBehaviour
{
    public enum IndicatorType
    {
        enemy,
        IndicatorTypeCount,
    }

    [SerializeField, EnumIndex(typeof(IndicatorType))]
    private GameObject[] m_indicatorPrefabs;

    private List<TargetIndicator>[] m_indicators = new List<TargetIndicator>[(int)IndicatorType.IndicatorTypeCount];

    [SerializeField] private Vector2 m_drawSize = new(200f, 100f);
    [SerializeField] private Vector2 m_drawCenterPos = new(0f, -300f);

    private void Awake()
    {
        for (int i = 0; i < (int)IndicatorType.IndicatorTypeCount; ++i)
        {
            m_indicators[i] = new List<TargetIndicator>();
            if (!m_indicatorPrefabs[i])
            {
                Debug.LogError("TargetIndicatorControllerにセットされていないIndicatorPrefabがあります。", gameObject);
            }
        }
    }

    public TargetIndicator AddIndicator(Transform _target, IndicatorType _addType)
    {
        if (!m_indicatorPrefabs[(int)_addType]) return null;

        for (int i = 0; i < m_indicators[(int)_addType].Count; ++i)
        {
            if (!m_indicators[(int)_addType][i].Target)
            {
                m_indicators[(int)_addType][i].Target = _target;
                return m_indicators[(int)_addType][i];
            }
        }

        GameObject gameObject = Instantiate(m_indicatorPrefabs[(int)_addType], transform);
        if (gameObject.TryGetComponent(out TargetIndicator newIndicator))
        {
            newIndicator.Target = _target;
            newIndicator.SetDrawData(m_drawSize, m_drawCenterPos);
            m_indicators[(int)_addType].Add(newIndicator);
            return newIndicator;
        }
        return null;
    }
}
