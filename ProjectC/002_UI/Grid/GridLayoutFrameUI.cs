using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(100)]
public class GridLayoutFrameUI : MonoBehaviour
{

    // 制作者 田内
    // GridLayout Groupに合わせて拡縮率を変更する

    public RectTransform m_frameUIRectTransform;


    public GridLayoutGroup m_gridLayoutGroup;

    //===============================================
    //                  実行処理
    //===============================================

    void Start()
    {
        m_frameUIRectTransform = gameObject.GetComponent<RectTransform>();
        m_gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
    }


    /// <summary>
    /// recttransformに何かしらの変更が加わった時にのみ更新
    /// </summary>
    private void OnRectTransformDimensionsChange()
    {
        ChangeScale();
    }
    private void OnTransformChildrenChanged()
    {
        ChangeScale();
    }


    private void ChangeScale()
    {
        #region nullチェック
        if (m_gridLayoutGroup == null)
        {
            Debug.LogError("GridLayoutGroupを取得できませんでした");
        }

        if (m_frameUIRectTransform == null)
        {
            Debug.LogError("RectTransformを取得できませんでした");
        }
        #endregion

        float width = 0.0f;
        float height = 0.0f;

        width += m_gridLayoutGroup.padding.left + m_gridLayoutGroup.padding.right + m_gridLayoutGroup.spacing.x;
        height += m_gridLayoutGroup.padding.top + m_gridLayoutGroup.padding.bottom + m_gridLayoutGroup.spacing.y;

        int heightCellNum = 0;
        int widthCellNum = 0;

        switch (m_gridLayoutGroup.constraint)
        {

            case GridLayoutGroup.Constraint.FixedColumnCount:
                {
                    heightCellNum = (int)System.Math.Ceiling((float)m_gridLayoutGroup.transform.childCount / m_gridLayoutGroup.constraintCount);
                    if (heightCellNum >= 1)
                    {
                        widthCellNum = m_gridLayoutGroup.transform.childCount;
                    }
                    else
                    {
                        widthCellNum = m_gridLayoutGroup.transform.childCount;
                    }
                    break;
                }

            case GridLayoutGroup.Constraint.FixedRowCount:
                {
                    widthCellNum = (int)System.Math.Ceiling((float)m_gridLayoutGroup.transform.childCount / m_gridLayoutGroup.constraintCount);
                    if (widthCellNum >= 1)
                    {
                        heightCellNum = m_gridLayoutGroup.transform.childCount;
                    }
                    else
                    {
                        heightCellNum = m_gridLayoutGroup.transform.childCount;
                    }
                    break;
                }
            default:
                {
                    Debug.LogError("対応できていません");
                    break;
                }



        }

        if (widthCellNum <= 0) widthCellNum = 1;
        if (heightCellNum <= 0) heightCellNum = 1;


        width += m_gridLayoutGroup.cellSize.x * widthCellNum;
        height += m_gridLayoutGroup.cellSize.y * heightCellNum;


        m_frameUIRectTransform.sizeDelta = new Vector2(width, height);

    }

}
