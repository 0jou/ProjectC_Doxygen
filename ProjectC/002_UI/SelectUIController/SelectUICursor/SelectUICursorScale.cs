using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUICursorScale : MonoBehaviour
{
    // 制作者 田内
    // 選択中のUIに拡縮率を合わせる

    [Header("UI管理するコントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    //==========================
    // 自身の行列
    private RectTransform m_myRectTransform = null;

    //========================================
    //              実行処理
    //========================================

    private void Start()
    {
        m_myRectTransform = GetComponent<RectTransform>();
        if (m_myRectTransform == null)
        {
            Debug.LogError("アタッチしているオブジェクトにRectTransformが存在しません");
        }
    }


    private void Update()
    {
        UpdateScale();
    }

    // カーソルを変更
    private void UpdateScale()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }
        if (m_myRectTransform == null)
        {
            Debug.LogError("アタッチしているオブジェクトにRectTransformが存在しません");
            return;
        }
        #endregion


        // 現在選択中のUI
        GameObject ui = m_selectUIController.CurrentSelectUI;


        // 選択しているUIが無ければ
        if (ui != null)
        {
            m_myRectTransform.localScale = Vector3.zero;
        }
        // 選択しているUIがあれば
        else
        {
            RectTransform rect;
            if (ui.TryGetComponent(out rect) == false) return;

            var sizeDelta = rect.sizeDelta;
            var scale = rect.localScale;


            m_myRectTransform.sizeDelta = sizeDelta;
            m_myRectTransform.localScale = scale;

        }
    }
}
