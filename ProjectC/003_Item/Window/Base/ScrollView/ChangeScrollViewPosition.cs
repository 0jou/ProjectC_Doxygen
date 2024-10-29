using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;
using DG.Tweening;

public class ChangeScrollViewPosition : MonoBehaviour
{

    // 制作者　田内
    // スクロールビューをSelectUIControllerを基にスクロールさせる
    // スクロールを行うのは子オブジェクトのUIが選択された場合のみ


    enum ChangeScrollViewPositionType
    {
        Center = 0, // 真ん中を比較基準にする
        Edge = 1,   // 端を比較基準にする
    }



    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;


    [Header("グリッドレイアウト")]
    [SerializeField]
    private GridLayoutGroup m_gridLayoutGroup = null;


    [Header("ScrollViewのScrollRect")]
    [SerializeField]
    private ScrollRect m_scrollRect = null;


    [Header("ScrollViewのRectTransform")]
    [SerializeField]
    private RectTransform m_scrollRectTransform = null;


    [Header("ゲームがストップ状態でも動くか")]
    [SerializeField]
    protected bool m_isStop = true;



    [Header("基準タイプ")]
    [SerializeField]
    private ChangeScrollViewPositionType m_type;


    [Header("縦の制御")]
    [SerializeField]
    private bool m_isVertical = true;


    [Header("横の制御")]
    [SerializeField]
    private bool m_isHorizontal = false;

    /// <summary>
    /// 実行処理
    /// </summary>
    public void OnUpdate()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("UI選択コントローラーが登録されていません");
            return;
        }
        #endregion

        // 変更があれば
        if (!m_selectUIController.IsSelectChangeFlg) return;


        // 子オブジェクトの場合スクロール
        var currentUI = m_selectUIController.CurrentSelectUI;
        if (currentUI == null) return;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            var child = gameObject.transform.GetChild(i);
            if (currentUI.transform == child)
            {
                Scroll(currentUI);
                return;
            }
        }

    }


    //毎フレーム動かす処理（山本）
    public void OnUpdateEveryTime()
    {
        if (m_selectUIController == null)
        {
            Debug.LogError("UI選択コントローラーが登録されていません");
            return;
        }


        var currentUI = m_selectUIController.CurrentSelectUI;
        if (currentUI == null) return;

        // 選択中のUIが変更されれば更新
        Scroll(currentUI);

    }


    // 座標を変更
    void Scroll(GameObject _ui)
    {
        #region nullチェック
        if (m_scrollRect == null)
        {
            Debug.LogError("ScrollRectシリアライズされていません");
            return;
        }
        if (m_scrollRectTransform == null)
        {
            Debug.LogError("RectTransformがシリアライズされていません");
            return;
        }
        if (_ui == null) return;
        #endregion

        // UIの行列を取得
        RectTransform rect;
        if (_ui.TryGetComponent(out rect) == false) return;

        // 距離
        Vector2 dist = Vector2.zero;

        switch (m_type)
        {
            case ChangeScrollViewPositionType.Center:
                {
                    if (Center(rect, ref dist) == false) return;
                    break;
                }

            case ChangeScrollViewPositionType.Edge:
                {
                    if (Edge(rect, ref dist) == false) return;
                    break;
                }
        }

        // 現在のスクロール位置を取得
        Vector2 currentScrollPosition = m_scrollRect.content.anchoredPosition;

        // 新しいスクロール位置を計算
        // dist.y += m_scrollRectTransform.position.y;
        Vector2 newScrollPosition = currentScrollPosition - dist;

        // 移動させる
        DoMove(newScrollPosition);

    }


    //追加したObjectをEdgeに追加したいため、新たに追加した（山本）
    public void ChangePosEdge(GameObject _object)
    {

        if (m_scrollRect == null)
        {
            Debug.LogError("スクロールビューのScrollRectコンポーネントがシリアライズされていません");
            return;
        }

        if (m_scrollRectTransform == null)
        {
            Debug.LogError("スクロールビューのRectTransformコンポーネントがシリアライズされていません");
            return;
        }

        var rect = _object.GetComponent<RectTransform>();
        if (rect == null) return;

        // 距離
        Vector2 dist = new();


        if (!Center(rect, ref dist))
        {
            // 変更はなし
            return;
        }


        // 現在のスクロール位置を取得
        Vector2 currentScrollPosition = m_scrollRect.content.anchoredPosition;

        // 新しいスクロール位置を計算
        // dist.y += m_scrollRectTransform.position.y;
        Vector2 newScrollPosition = currentScrollPosition - dist;

        // 移動させる
        DoMove(newScrollPosition);

    }


    /// <summary>
    /// 真ん中に座標を変更する
    /// </summary>
    private bool Center(RectTransform _objectTransform, ref Vector2 _dist)
    {

        if (m_scrollRectTransform == null)
        {
            Debug.LogError("スクロールビューのRectTransformコンポーネントがシリアライズされていません");
            return false;
        }

        var rect = _objectTransform;
        if (rect == null) return false;


        // rectのローカル位置を取得
        Vector3 rectLocalPos = m_scrollRectTransform.InverseTransformPoint(rect.position);
        Vector3 scrollRectLocalPos = m_scrollRectTransform.InverseTransformPoint(m_scrollRectTransform.position);


        if (m_isVertical)
        {
            // 縦を中心に
            float rectPos = rectLocalPos.y;
            float scrollRectPos = scrollRectLocalPos.y;

            _dist.y = rectPos - scrollRectPos;

        }

        if (m_isHorizontal)
        {
            // 横を中心に
            float rectPos = rectLocalPos.x;
            float scrollRectPos = scrollRectLocalPos.x;

            _dist.x = rectPos - scrollRectPos;

        }

        return true;

    }


    /// <summary>
    /// 端に座標を変更する
    /// </summary>
    private bool Edge(RectTransform _objectTransform, ref Vector2 _dist)
    {

        if (m_scrollRectTransform == null)
        {
            Debug.LogError("スクロールビューのRectTransformコンポーネントがシリアライズされていません");
            return false;
        }

        var rect = _objectTransform;
        if (rect == null) return false;


        // rectのローカル位置を取得
        Vector3 rectLocalPos = m_scrollRectTransform.InverseTransformPoint(rect.position);
        Vector3 scrollRectLocalPos = m_scrollRectTransform.InverseTransformPoint(m_scrollRectTransform.position);


        // 変更したかどうか
        bool isChange = false;

        if (m_isVertical)
        {
            // rectの下端とscrollRectの下端のローカル座標を計算
            var rectDownPos = rectLocalPos.y - (rect.rect.height / 2.0f);
            var scrollRectDownPos = scrollRectLocalPos.y - (m_scrollRectTransform.rect.height / 2.0f);

            // rectの上端とscrollRectの上端のローカル座標を計算
            var rectUpPos = rectLocalPos.y + (rect.rect.height / 2.0f);
            var scrollRectUpPos = scrollRectLocalPos.y + (m_scrollRectTransform.rect.height / 2.0f);

            if (m_gridLayoutGroup)
            {
                scrollRectDownPos += m_gridLayoutGroup.padding.bottom;

                scrollRectUpPos -= m_gridLayoutGroup.padding.top;
            }

            // スロットがスクロールビューの範囲外に出ている場合の処理
            if (scrollRectUpPos < rectUpPos)
            {
                isChange = true;
                _dist.y = rectUpPos - scrollRectUpPos;
            }
            else if (scrollRectDownPos > rectDownPos)
            {
                isChange = true;
                _dist.y = rectDownPos - scrollRectDownPos;
            }

        }

        if (m_isHorizontal)
        {
            // rectの左端とscrollRectの左端のローカル座標を計算
            var rectLeftPos = rectLocalPos.x - (rect.rect.width / 2.0f);
            var scrollRectLeftPos = scrollRectLocalPos.x - (m_scrollRectTransform.rect.width / 2.0f);

            // rectの右端とscrollRectの左端のローカル座標を計算
            var rectRightPos = rectLocalPos.x + (rect.rect.width / 2.0f);
            var scrollRectRightPos = scrollRectLocalPos.x + (m_scrollRectTransform.rect.width / 2.0f);

            if (m_gridLayoutGroup)
            {
                scrollRectLeftPos += m_gridLayoutGroup.padding.left;

                scrollRectRightPos -= m_gridLayoutGroup.padding.right;
            }

            // スロットがスクロールビューの範囲外に出ている場合の処理
            if (scrollRectRightPos < rectRightPos)
            {
                isChange = true;
                _dist.x = rectRightPos - scrollRectRightPos;
            }
            else if (scrollRectLeftPos > rectLeftPos)
            {
                isChange = true;
                _dist.x = rectLeftPos - scrollRectLeftPos;
            }

        }

        return isChange;

    }


    private void DoMove(Vector2 _goalPos)
    {
        // 新しいスクロール位置を設定
        m_scrollRect.content.DOAnchorPos(_goalPos, 0.5f).
            SetUpdate(m_isStop).
          SetLink(gameObject);
    }

}
