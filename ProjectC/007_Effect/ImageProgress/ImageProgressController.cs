/*!
 * @file ImageProgressController.cs
 * @brief 画像の進捗を管理するクラスシェーダーに渡す進捗度を管理する
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class ImageProgressController : MonoBehaviour
{
    [SerializeField]
    private Texture2D[] m_textures;

    [SerializeField] private Shader m_copySader;

    [Header("表示用テクスチャ")]
    [SerializeField]
    private Image m_image;

    [SerializeField, Range(0f, 1f)]
    private float m_progress = 0.0f;

    public float Progress
    {
        get { return m_progress; }
        set { SetProgress(value); }
    }


    private void Start()
    {
        if (m_image == null) return;
        m_image.material = new Material(m_copySader);
        m_progress = 0.0f;
        if (m_textures.Length == 0) return;

        m_image.material.SetFloat("_Progress", m_progress);
    }

    private void OnDestroy()
    {
        if (m_image == null) return;
        Destroy(m_image.material);
    }

    private void Update()
    {
        SetProgress();
    }
    public void SetProgress(float newProgress)
    {
        if (m_image == null) return;
        m_progress = Mathf.Clamp01(newProgress);

        int imageIndex = Mathf.FloorToInt(m_progress * m_textures.Length);
        if (imageIndex >= 0 && imageIndex < m_textures.Length)
        {
            m_image.sprite = Sprite.Create(m_textures[imageIndex], new Rect(0, 0, m_textures[imageIndex].width, m_textures[imageIndex].height), new Vector2(0.5f, 0.5f));
        }
        m_image.material.SetFloat("_Progress", m_progress);
    }

    [ContextMenu("SetProgress")]
    void SetProgress()
    {
        SetProgress(m_progress);
    }
}
