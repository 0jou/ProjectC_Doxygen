using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeCreateFoodControllerDescription : MonoBehaviour
{
    // 制作者 田内
    // 料理作成コントローラーの説明文

    [Header("コントローラー")]
    [SerializeField]
    private CreateFoodController m_createFoodController = null;

    [Header("現在の作成数")]
    [SerializeField]
    private TextMeshProUGUI m_currentCreateNumText = null;

    [Header("最大作成可能数")]
    [SerializeField]
    private TextMeshProUGUI m_maxCreateNumText = null;

    [Header("最小作成可能数")]
    [SerializeField]
    private TextMeshProUGUI m_minCreateNumText = null;

    //==============================================
    //              実行処理
    //==============================================

    virtual public void OnInitialize()
    {
        // 初期化
        SetDescription();
    }



    virtual public void OnUpdate()
    {
        if (IsChangeDescription())
        {
            SetDescription();
        }
    }



    private void SetDescription()
    {
        SetCurrentCreateNumText();

        SetMaxCreateNumText();

        SetMinCreateNumText();

    }


    private void SetCurrentCreateNumText()
    {
        #region nullチェック
        if (m_createFoodController == null)
        {
            return;
        }
        if (m_currentCreateNumText == null)
        {
            // シリアライズされていません
            return;
        }
        #endregion

        m_currentCreateNumText.gameObject.SetActive(false);

        m_currentCreateNumText.text = m_createFoodController.CurrentCreateNum.ToString();

        m_currentCreateNumText.gameObject.SetActive(true);
    }


    private void SetMaxCreateNumText()
    {
        #region nullチェック
        if (m_createFoodController == null)
        {
            return;
        }
        if (m_maxCreateNumText == null)
        {
            // シリアライズされていません
            return;
        }
        #endregion

        m_maxCreateNumText.gameObject.SetActive(false);

        m_maxCreateNumText.text = m_createFoodController.MaxCreateNum.ToString();

        m_maxCreateNumText.gameObject.SetActive(true);
    }


    private void SetMinCreateNumText()
    {
        #region nullチェック
        if (m_createFoodController == null)
        {
            return;
        }
        if (m_minCreateNumText == null)
        {
            // シリアライズされていません
            return;
        }
        #endregion

        m_minCreateNumText.gameObject.SetActive(false);

        m_minCreateNumText.text = m_createFoodController.MinCreateNum.ToString();

        m_minCreateNumText.gameObject.SetActive(true);

    }

    // 説明文を変更できるか確認
    public bool IsChangeDescription()
    {
        // コントローラーがなければ
        if (m_createFoodController == null)
        {
            Debug.LogError("コントローラーが登録されていません");
            return false;
        }

        if (m_createFoodController.IsSelectChangeFlg)
        {
            // 説明文を変更する
            return true;
        }

        return false;
    }



}
