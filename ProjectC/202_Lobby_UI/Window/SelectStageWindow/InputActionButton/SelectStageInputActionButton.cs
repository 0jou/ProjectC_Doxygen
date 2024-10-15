using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStageInputActionButton :InputActionButton
{
    // 制作者 田内
    // ステージ選択用のInputActionButton



    [Header("ステージ選択コントローラー")]
    [SerializeField]
    private SelectStageController m_selectStageController = null;

    private enum SelectStageButtonType
    {
        Next,
        Back,
        Start,
    }

    [Header("チュートリアルボタン種類")]
    [SerializeField]
    private SelectStageButtonType m_selectStageButtonType = SelectStageButtonType.Next;


    //================================================
    //                 実行処理
    //================================================


    protected override bool IsPress()
    {

        #region nullチェック
        if (m_selectStageController == null)
        {
            Debug.LogError("SelectStageControllerがシリアライズされていません");
            return false;
        }
        #endregion

        switch (m_selectStageButtonType)
        {

            case SelectStageButtonType.Next:
                {
                    return m_selectStageController.IsGoNext();
                }
            case SelectStageButtonType.Back:
                {
                    return m_selectStageController.IsGoBack();
                }
            case SelectStageButtonType.Start:
                {
                    return m_selectStageController.IsGoStart();
                }
            default:
                {
                    return false;
                }
        }
    }


}
