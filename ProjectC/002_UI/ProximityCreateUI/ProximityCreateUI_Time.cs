using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public partial class ProximityCreateUI : MonoBehaviour
{
    // 時間経過で削除
    // 制作者　田内

    [EnableIf("m_type", ProximityCreateUIDestroyType.Time)]
    [Header("削除にかかる時間")]
    [SerializeField]
    private float m_hideDelay = 10.0f;

    // 経過時間
    private float m_currentDelay = 0.0f;


    private void InitTime()
    {
        m_currentDelay = 0.0f;
    }

    private void UpdateTime()
    {
        if (m_type != ProximityCreateUIDestroyType.Time) return;
        if (!m_isCreate || m_createUI == null) return;

        m_currentDelay += Time.deltaTime;
        if (m_currentDelay >= m_hideDelay)
        {
            // 作成不可能状態に
            m_isCreate = false;

            // 初期化する
            InitTime();

            // 非表示
            HideUI();
        }
    }
}
