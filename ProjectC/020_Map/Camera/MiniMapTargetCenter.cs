using SaintsField.Playa;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 常にターゲットの位置と同じ位置にする処理 （吉田）
/// マップを表示するカメラで使ってます。
/// 
/// スクリプトなくても親子関係で解決しますが、
/// ヒエラルキーのわかりやすさを考慮して、スクリプトで処理しています。
/// 
/// </summary>
public class MiniMapTargetCenter : MonoBehaviour
{
    [SerializeField]
    private Transform m_target;

    [Header("ターゲットと同じにする軸")]
    [SerializeField]
    private bool m_xAxis = true;
    [SerializeField]
    private bool m_yAxis = true;
    [SerializeField]
    private bool m_zAxis = true;

    [Button]
    [ContextMenu("TargetCenter")]
    private void TargetCenter()
    {
        if (m_target == null) return;

        Vector3 targetPos = m_target.position;
        Vector3 objPos = transform.position;

        if (m_xAxis) objPos.x = targetPos.x;
        if(m_yAxis) objPos.y = targetPos.y;
        if(m_zAxis) objPos.z = targetPos.z;

        transform.position = objPos;
    }

    private void Update()
    {
        TargetCenter();
    }
}
