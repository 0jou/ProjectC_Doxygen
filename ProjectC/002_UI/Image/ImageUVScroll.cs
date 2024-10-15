using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class ImageUVScroll : MonoBehaviour
{
    // 制作者 田内
    // ImageをUVスクロールさせる

    // ※上手く動かない場合
    // 使用する画像にWrapModeが「Repeat」になっているか確認してください


    [Header("スクロールさせるImage")]
    [SerializeField]
    private Image m_image = null;

    [Header("移動量")]
    [SerializeField]
    private Vector2 m_step = new Vector2(1.0f, 1.0f);

    [Header("時間")]
    [SerializeField]
    [Min(0.0f)]
    private float m_duration = 1.0f;

    [Header("EaseType")]
    [SerializeField]
    private Ease m_ease = Ease.Linear;

    [Header("LoopType")]
    [SerializeField]
    private LoopType m_loopType = LoopType.Incremental;

    // 作成したマテリアル(メモリリーク用)
    private Material m_instanceMaterial = null;


    //========================================================
    //                       実行処理
    //========================================================

    void Start()
    {
        UVScroll();
    }

    private void OnDestroy()
    {
        if (m_instanceMaterial != null)
        {
            Destroy(m_instanceMaterial);
            m_instanceMaterial = null;
        }
    }

    private void UVScroll()
    {
        if (m_image == null)
        {
            Debug.LogError("スクロールするImageがシリアライズされていません");
            return;
        }

        // 現在のマテリアルのインスタンスを複製
        m_instanceMaterial = new Material(m_image.material);

        // マテリアルを作成したマテリアルに変更
        m_image.material = m_instanceMaterial;

        // UVScroll開始
        m_image.material.DOOffset(m_step, m_duration).
            SetLoops(-1, m_loopType).
            SetEase(m_ease).
            SetUpdate(true).
            SetLink(m_image.gameObject);
    }

}
