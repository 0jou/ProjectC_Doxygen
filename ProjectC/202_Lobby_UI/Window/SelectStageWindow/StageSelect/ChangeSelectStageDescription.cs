using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ChangeSelectStageDescription : MonoBehaviour
{
    // ステージの情報を更新する
    // 制作者　田内

    [Header("ステージ選択コントローラー")]
    [SerializeField]
    protected SelectStageController m_selectStageController = null;


    [Header("ステージImage")]
    [SerializeField]
    protected Image m_stageImage = null;

    [Header("ステージ名前")]
    [SerializeField]
    protected TextMeshProUGUI m_stageNameTextMeshProUGUI = null;

    [Header("ステージ紹介文")]
    [SerializeField]
    protected TextMeshProUGUI m_descriptionTextMeshProUGUI = null;

    //========================================================
    //                      実行処理
    //========================================================

    virtual public void OnInitialize()
    {
        // 初期化
        ChangeDescription();
    }



    virtual public void OnUpdate()
    {
        if (IsChangeDescription())
        {
            ChangeDescription();
        }
    }



    private void ChangeDescription()
    {
        // コントローラーがなければ
        if (m_selectStageController == null)
        {
            Debug.LogError("コントローラーがシリアライズされていません");
            return;
        }

        var id = m_selectStageController.CurrentSelectStageID;
        var data = StageDataBaseManager.instance.GetStageData(id);

        SetDescription(data);

    }


   

    private void SetDescription(StageData _data)
    {
        if (_data == null)
        {
            Debug.LogError("引数データが存在しません");
            return;
        }

        // ステージの画像をセット
        SetImage(_data);

        // ステージ名テキストをセット
        SetStageNameText(_data);

        // ステージ説明テキストをセット
        SetDescriptionText(_data);

    }

    private void SetStageNameText(StageData _data)
    {
        if (m_stageNameTextMeshProUGUI == null)
        {
            Debug.Log("ステージ名を表示するTextMeshProUGUIがシリアライズされていません");
            return;
        }

        m_stageNameTextMeshProUGUI.gameObject.SetActive(false);

        if (_data == null)
        {
            Debug.LogError("引数データが存在しません");
            return;
        }

        m_stageNameTextMeshProUGUI.text = _data.StageNameText;

        m_stageNameTextMeshProUGUI.gameObject.SetActive(true);
    }


    private void SetImage(StageData _data)
    {
        if (m_stageImage == null)
        {
            Debug.Log("ステージ画像を表示するImageがシリアライズされていません");
            return;
        }

        m_stageImage.gameObject.SetActive(false);

        if (_data == null)
        {
            Debug.LogError("引数データが存在しません");
            return;
        }

        m_stageImage.sprite = _data.StageSprite;

        m_stageImage.gameObject.SetActive(true);
    }

    private void SetDescriptionText(StageData _data)
    {
        if(m_descriptionTextMeshProUGUI==null)
        {
            Debug.Log("説明文を表示するTextMeshProUGUIがシリアライズされていません");
            return;
        }

        m_descriptionTextMeshProUGUI.gameObject.SetActive(false);

        if (_data == null)
        {
            Debug.LogError("引数データが存在しません");
            return;
        }

        m_descriptionTextMeshProUGUI.text = _data.StageDescriptionText;

        m_descriptionTextMeshProUGUI.gameObject.SetActive(true);
    }


    // 説明文を変更できるか確認
    public bool IsChangeDescription()
    {
        // コントローラーがなければ
        if (m_selectStageController == null)
        {
            Debug.LogError("コントローラーが登録されていません");
            return false;
        }

        if (!m_selectStageController.IsSelectChangeFlg) return false;

        // 説明文を変更する
        return true;
    }

}
