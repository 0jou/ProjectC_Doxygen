using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using SelectUIInfo;

public class SelectUICursorPress : MonoBehaviour
{
    // 制作者 田内

    [Header("UI管理するコントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;


    [Header("ゲージ画像")]
    [SerializeField]
    private Image m_gaugeImage = null;

    //========================================
    //              実行処理
    //========================================


    private void Update()
    {
        Select();
    }

    private void Select()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }
        #endregion

        // 選択中のUIを取得
        var uiData = m_selectUIController.CurrentSelectUIData;
        if (uiData == null) return;

        switch (uiData.SelectUIType)
        {
            case SelectUIType.Press:
                {
                    break;
                }

            case SelectUIType.Hold:
                {
                    Hold();
                    break;
                }
        }
    }


    private void Hold()
    {
        if (m_gaugeImage == null)
        {
            Debug.LogError("ゲージ用画像がシリアライズされていません");
            return;
        }


        var action = PlayerInputManager.instance.GetInputAction(InputActionMapTypes.UI, m_selectUIController.HoldInputAction);
        if (action == null) return;

        action.Enable();
        float progress = action.GetTimeoutCompletionPercentage();

        // 進捗
        m_gaugeImage.fillAmount = progress;

    }



}
