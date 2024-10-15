using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIconController : MonoBehaviour
{
    // Inspectorに表示させたいEnum
    public enum EnemyIconType
    {
        DiscoverIcon,
        LostSightIcon,
        None
    }

    // EnumIndex属性で表示したいEnumを設定することでInspector上に反映されます
    [SerializeField, EnumIndex(typeof(EnemyIconType))]
    private EnemyIconElement[] m_icons = new EnemyIconElement[(int)EnemyIconType.None];

    private EnemyIconType m_type = EnemyIconType.None;
    public EnemyIconType NowShowType { get { return m_type; } }

    private TargetIndicator m_indicator;

    public void ShowIcon(EnemyIconType _type, float _fillTime)
    {
        if (m_type != EnemyIconType.None)
        {
            m_icons[(int)m_type].Hide();
        }

        if (_type != EnemyIconType.None)
        {
            m_type = _type;
            m_icons[(int)m_type].Show(_fillTime);
        }

        switch (_type)
        {
            case EnemyIconType.DiscoverIcon:
                {
                    var UIObjects = GameObject.FindGameObjectsWithTag("UI");
                    foreach (var UI in UIObjects)
                    {
                        if (UI.TryGetComponent(out TargetIndicatorController controller))
                        {
                            m_indicator = controller.AddIndicator(transform.root, TargetIndicatorController.IndicatorType.enemy);
                        }
                    }
                }
                break;
            case EnemyIconType.LostSightIcon:
                {
                    if (m_indicator)
                    {
                        m_indicator.Target = null;
                        m_indicator = null;
                    }
                }
                break;
        }
    }

    public void HideIcon(EnemyIconType type)
    {
        if (m_type != EnemyIconType.None && m_type == type)
        {
            m_icons[(int)m_type].Hide();
            m_type = EnemyIconType.None;
        }
    }

    private void OnDestroy()
    {
        if (m_indicator)
        {
            m_indicator.Target = null;
            m_indicator = null;
        }
    }
}
