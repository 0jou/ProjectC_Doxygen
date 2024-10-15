using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFoodControllerInputActionButton : InputActionButton
{
    // 制作者 田内
    // 良瑛作成コントローラー用ボタン

    [Header("料理作成コントローラー")]
    [SerializeField]
    private CreateFoodController m_createFoodController = null;

    private enum CreateFoodControllerType
    {
        Increment,
        Decrement,
        Create,
    }

    [SerializeField]
    private CreateFoodControllerType m_createFoodControllerButtonType = CreateFoodControllerType.Increment;

    //================================================
    //                 実行処理
    //================================================


    protected override bool IsPress()
    {
        #region nullチェック
        if (m_createFoodController == null)
        {
            Debug.LogError("CreateFoodControllerがシリアライズされていません");
            return false;
        }
        #endregion

        switch (m_createFoodControllerButtonType)
        {

            case CreateFoodControllerType.Increment:
                {
                    return m_createFoodController.IsIncrement();
                }
            case CreateFoodControllerType.Decrement:
                {
                    return m_createFoodController.IsDecrement();
                }
            case CreateFoodControllerType.Create:
                {
                    return m_createFoodController.IsCreate();
                }
            default:
                {
                    return false;
                }
        }
    }

}
