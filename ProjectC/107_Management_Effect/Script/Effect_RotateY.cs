using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BillBoard系Effect方向指定用スクリプト(濱口)
/// </summary>
public class Effect_RotateY: MonoBehaviour
{
    [SerializeField, Header("対象のカメラ")]
    [Header("ChangeBillboardPosで使われる（実行中は使われない）")]
    private Camera m_targetCamera;

    public void Start()
    {
        if (m_targetCamera == null)
        {
            m_targetCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        transform.forward = forward;
    }

    // 実行していない時に位置を調節する場合
    // m_targetCamera　が使われる
    [ContextMenu("ChangeBillboardPos")]
    private void ChangeBillboardPos()
    {
        transform.forward = m_targetCamera.transform.forward;
    }
}
