using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public partial class ProximityCreateUI : MonoBehaviour
{

    // 当たり判定で削除
    // 制作者　田内

    private void OnTriggerExitCollider(Collider other)
    {
        if (m_type != ProximityCreateUIDestroyType.Collider) return;
        if (!m_isCreate || m_createUI == null) return;

        if (other.CompareTag(m_tag))
        {
            // 作成不可能状態
            m_isCreate = false;

            // 非表示
            HideUI();
        }
    }


    private void UpdateCollider()
    {
        if (m_type != ProximityCreateUIDestroyType.Collider) return;
        if (m_createUI == null) return;

        // 独自の処理はここ
    }
}
