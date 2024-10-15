using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Canvasのビルボード用スクリプト（吉田）
/// </summary>
public class Billboard : MonoBehaviour
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
        transform.forward = Camera.main.transform.forward;
    }

    // 実行していない時に位置を調節する場合
    // m_targetCamera　が使われる
    [ContextMenu("ChangeBillboardPos")]
    private void ChangeBillboardPos()
    {
        transform.forward = m_targetCamera.transform.forward;
    }
}