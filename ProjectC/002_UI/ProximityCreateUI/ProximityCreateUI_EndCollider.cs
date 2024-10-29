using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public partial class ProximityCreateUI : MonoBehaviour
{
    // 削除用の当たり判定にあたれば削除する
    // 制作者　田内

    [EnableIf("m_type", ProximityCreateUIDestroyType.EndCollider)]
    [Header("削除用当たり判定")]
    [SerializeField]
    private ShareOnTrigger m_endTrriger = null;


    private void UpdateEndCollider()
    {
        if (m_type != ProximityCreateUIDestroyType.EndCollider) return;
        if (!m_isCreate || m_createUI == null) return;

        if (m_endTrriger==null)
        {
            Debug.LogError("削除用トリガーがシリアライズされていません");
            HideUI();
            return;
        }

        if(m_endTrriger.IsTrigger)
        {
            // 作成不可能状態
            m_isCreate = false;

            // 非表示
            HideUI();
        }

    }


}
