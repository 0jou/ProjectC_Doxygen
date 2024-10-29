using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class SelectUICursorPosition : MonoBehaviour
{
    // 制作者 田内
    // UI選択コントローラー用カーソル


    [Header("UI管理するコントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    private enum SelectUICursorType
    {
        Middle,
        Top,
        Left,
    }

    [Header("カーソルタイプ")]
    [SerializeField]
    private SelectUICursorType m_cursorType = SelectUICursorType.Middle;


    //===========================
    // 保存用
    private Vector3 m_keepPos = Vector3.zero;

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
        UpdatePosition();
    }

    // カーソルを変更
    private void UpdatePosition()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }
        #endregion

        // 現在選択中のUI
        var ui = m_selectUIController.CurrentSelectUI;

        // 選択しているUIが無ければ
        if (ui == null)
        {
            // 見えない場所へ移動させる
            Vector2 pos = new Vector2(10000, 10000);
            gameObject.transform.position = pos;

        }
        // 選択しているUIがあれば
        else
        {
            // ターゲットのUI座標が変更されれば
            var pos = ui.gameObject.transform.position;
            if (pos == m_keepPos) return;

            // 保存
            m_keepPos = pos;

            switch (m_cursorType)
            {
                case SelectUICursorType.Middle:
                    {
                        Middle(ui);
                        break;
                    }
                case SelectUICursorType.Top:
                    {
                        Top(ui);
                        break;
                    }
                case SelectUICursorType.Left:
                    {
                        Left(ui);
                        break;
                    }
            }


        }
    }



    // 中央に移動
    private void Middle(GameObject _ui)
    {

        #region nullチェック
        if (m_myRectTransform == null)
        {
            Debug.LogError("アタッチしているオブジェクトにRectTransformが存在しません");
            return;
        }
        #endregion

        m_myRectTransform.position = _ui.transform.position;
    }


    private void Top(GameObject _ui)
    {
        #region nullチェック
        if (m_myRectTransform == null)
        {
            Debug.LogError("アタッチしているオブジェクトにRectTransformが存在しません");
            return;
        }
        #endregion

        RectTransform rect;
        if (_ui.TryGetComponent(out rect) == false) return;

        Vector3 pos = rect.position;

        pos.y += (rect.sizeDelta.y * rect.localScale.y) / 2.0f;
        pos.y += (m_myRectTransform.sizeDelta.y * m_myRectTransform.localScale.y) / 2.0f;

        m_myRectTransform.position = pos;

    }

    private void Left(GameObject _ui)
    {
        #region nullチェック
        if (m_myRectTransform == null)
        {
            Debug.LogError("アタッチしているオブジェクトにRectTransformが存在しません");
            return;
        }
        #endregion

        RectTransform rect;
        if (_ui.TryGetComponent(out rect) == false) return;

        Vector3 pos = rect.position;

        pos.x -= (rect.sizeDelta.x * rect.localScale.x) / 2.0f;
        pos.x -= (m_myRectTransform.sizeDelta.x * m_myRectTransform.localScale.x) / 2.0f;

        m_myRectTransform.position = pos;

    }

}

