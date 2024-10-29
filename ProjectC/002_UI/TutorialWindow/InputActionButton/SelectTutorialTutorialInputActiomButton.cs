using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTutorialTutorialInputActiomButton : InputActionButton
{
    // 制作者 田内
    // チュートリアルボタン


  
    [Header("コントローラー")]
    [SerializeField]
    private SelectTutorialController m_selectTutorialController = null;

    private enum TutorialButtonType
    {
        Next,
        Back,
        Close,
    }

    [Header("チュートリアルボタン種類")]
    [SerializeField]
    private TutorialButtonType m_tutorialButtonType = TutorialButtonType.Next;

    //================================================
    //                 実行処理
    //================================================


    protected override bool IsPress()
    {
        #region nullチェック
        if (m_selectTutorialController == null)
        {
            Debug.LogError("SelectTutorialControllerがシリアライズされていません");
            return false;
        }
        #endregion

        switch (m_tutorialButtonType)
        {

            case TutorialButtonType.Next:
                {
                    return m_selectTutorialController.IsGoNext();
                }
            case TutorialButtonType.Back:
                {
                    return m_selectTutorialController.IsGoBack();
                }
            case TutorialButtonType.Close:
                {
                    return m_selectTutorialController.IsGoClose();
                }
            default:
                {
                    return false;
                }
        }
    }

}
