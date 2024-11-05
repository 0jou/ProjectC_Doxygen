using SaintsField.Playa;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 常にターゲットの方向と同じ回転をする(吉田)
/// ターゲットは、3Dオブジェクト
/// （アタッチしてる）オブジェクトは、2Dオブジェクト
/// 
/// </summary>
public class MiniMapTargetDirection : MonoBehaviour
{
    [SerializeField]
    private Transform m_target;

    [Button]
    [ContextMenu("TargetDirection")]
    private void TargetDirection()
    {
        if (m_target == null) return;

        Vector3 targetRot = m_target.rotation.eulerAngles;
        Vector3 objRot = transform.rotation.eulerAngles;
        objRot.z = targetRot.y;

        transform.rotation = Quaternion.Euler(objRot);
    }

    private void Update()
    {
        TargetDirection();
    }

    private void OnDisable()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
