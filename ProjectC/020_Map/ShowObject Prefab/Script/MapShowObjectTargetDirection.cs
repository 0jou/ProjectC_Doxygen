using System.Collections;
using System.Collections.Generic;
using SaintsField.Playa;
using UnityEngine;

/// <summary>
/// 常にターゲットの方向と同じ回転をする(吉田)
/// ターゲットは、3Dオブジェクト
/// （アタッチしてる）オブジェクトも、3Dオブジェクト
/// 
/// </summary>

// 更新順　遅い
[DefaultExecutionOrder(100)]
public class MapShowObjectTargetDirection : MonoBehaviour
{
    [SerializeField]
    private bool m_isTargetMainCamera = false;

    [SerializeField]
    private Transform m_target;

    [ContextMenu("メインカメラを ターゲットにする")]
    private void SetMainCameraToTarget()
    {
        m_target = Camera.main.transform;
    }

    [Button]
    [ContextMenu("TargetDirection")]
    private void TargetDirection()
    {
        if (m_target == null) return;

        Vector3 targetRot = m_target.rotation.eulerAngles;
        Vector3 objRot = transform.rotation.eulerAngles;
        objRot.y = targetRot.y;

        transform.rotation = Quaternion.Euler(objRot);
    }

    private void Start()
    {
        if (m_isTargetMainCamera)
        {
            SetMainCameraToTarget();
        }
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
