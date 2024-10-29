using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 3D空間でもUIの大きさが一定になるようにする（吉田）
/// </summary>
public class LabelTransController : MonoBehaviour
{

    // ターゲット HPバーを持つオブジェクト（キャラクター）
    [SerializeField] private GameObject m_target;

    // 大きさを一定にするためのキャンバス
    [SerializeField] private RectTransform m_canvasRect;

    // キャンバスの基本大きさ 大きすぎたらこの値を変える
    [SerializeField] private float m_baseScale = 0.0005f;

    private void Update()
    {
        UpdateLabelTrans();
    }

    [ContextMenu("UpdateLabelTrans")]
    private void UpdateLabelTrans()
    {
        float distance = GetDistance();

        // スケール
        m_canvasRect.transform.localScale = Vector3.one * m_baseScale * distance;
    }

    private float GetDistance()
    {
        Transform cameraTrans = Camera.main.transform;
        return (m_target.gameObject.transform.position - cameraTrans.position)
            .magnitude;
    }

}