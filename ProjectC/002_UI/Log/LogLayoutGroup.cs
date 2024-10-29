using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class LogLayoutGroup : MonoBehaviour
{
    // 制作者 田内
    // ログ用、目的地へ滑らかに移動するLayoutGroup
    // 子オブジェクトが追加されるたびに更新される

    [Header("UIサイズ")]
    [SerializeField]
    private Vector2 m_cellSize = new Vector2(100, 100);

    [Header("間隔")]
    [SerializeField]
    private Vector2 m_spaceSize = new Vector2(0, 0);

    private enum DirectionType
    {
        Width,
        Height,
    }

    [Header("向き")]
    [SerializeField]
    private DirectionType m_directionType = DirectionType.Height;

    private enum ShiftType
    {
        Top_Left,
        Bottom_Right,
    }

    [Header("座標タイプ")]
    [SerializeField]
    private ShiftType m_shiftType = ShiftType.Top_Left;

    private enum StandardType
    {
        Old,    // 古い
        New,    // 新しい
    }

    [Header("基準タイプ")]
    [SerializeField]
    private StandardType m_standardType = StandardType.New;


    //===================================
    // DoTween

    [BoxGroup("DoTween")]
    [SerializeField]
    private Ease m_ease = Ease.OutCubic;

    [BoxGroup("DoTween")]
    [SerializeField]
    private float m_duration = 0.5f;

    //====================================
    // 実行中のDoTween

    private List<Tween> m_tweenList = new();

    //========================================
    //              実行処理
    //========================================


    // 子オブジェクトが追加されたときに更新
    void OnTransformChildrenChanged()
    {
        Layout();
    }

    private void OnDestroy()
    {
        // メモリリーク対策
        KillTween();
    }

    private void KillTween()
    {
        for (int i = 0; i < m_tweenList.Count; ++i)
        {
            m_tweenList[i].Kill();
            m_tweenList[i] = null;
        }
        m_tweenList.Clear();
    }

    /// <summary>
    /// レイアウトを操作
    /// </summary>
    private void Layout()
    {

        // 停止する
        KillTween();

        // 子オブジェクト分回す
        for (int i = 0; i < transform.childCount; ++i)
        {
            Vector2 shiftPos = Vector2.zero;
            int posNumber = i;

            // 基準を決める
            switch (m_standardType)
            {
                // 古いものを基準にするか
                case StandardType.Old:
                    {
                        break;
                    }
                // 新しいものを基準にするか
                case StandardType.New:
                    {
                        posNumber = Mathf.Abs(posNumber - (transform.childCount - 1));
                        break;
                    }
            }

            // ずらす座標
            shiftPos = (m_cellSize + m_spaceSize) * posNumber;

            // 向きを変更
            switch (m_directionType)
            {
                case DirectionType.Height:
                    {
                        shiftPos.x = 0.0f;
                        break;
                    }
                case DirectionType.Width:
                    {
                        shiftPos.y = 0.0f;
                        break;
                    }
            }

            switch (m_shiftType)
            {
                // 基準UIの位置が上/左
                case ShiftType.Top_Left:
                    {
                        shiftPos = new Vector2(Mathf.Abs(shiftPos.x), Mathf.Abs(shiftPos.y));
                        shiftPos = new Vector2(0.0f, 0.0f) - shiftPos;
                        break;
                    }
                // 基準UIの位置が下/右
                case ShiftType.Bottom_Right:
                    {
                        shiftPos = new Vector3(Mathf.Abs(shiftPos.x), Mathf.Abs(shiftPos.y), 0.0f);
                        break;
                    }
            }

            // DoTweenで移動
            var obj = transform.GetChild(i);
            if (obj.TryGetComponent<RectTransform>(out var rect))
            {
                var tween = rect.DOAnchorPos(shiftPos, m_duration).
                  SetEase(m_ease).SetLink(rect.gameObject);

                m_tweenList.Add(tween);
            }
        }
    }

}