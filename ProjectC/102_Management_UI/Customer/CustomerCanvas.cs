/*!
 * @file CustomerCanvas.cs
 * @brief 客UIを管理するクラス 頭上に怒りゲージと求めている料理を表示する
 * @author 田内
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using CustomerStateInfo;

/// <summary>
/// @brief 客UIを管理するクラス
/// @detail 頭上に怒りゲージと求めている料理を表示する
/// </summary>
public class CustomerCanvas : MonoBehaviour
{
    //! @brief 怒りゲージ表現管理スクリプト
    [Header("怒りゲージ確認")]
    [SerializeField]
    private ImageProgressController m_imageProgressController = null;

    //! @brief 求めている料理の画像
    [Header("求めている料理画像")]
    [SerializeField]
    private Image m_foodImage = null;

    //! @brief 客情報
    private CustomerData m_targetCustomerData = null;

    public CustomerData TargetCustomerData
    {
        set { m_targetCustomerData = value; }
        get { return m_targetCustomerData; }
    }


    //=================================================
    //                  実行処理
    //=================================================

    private void Start()
    {
        m_targetCustomerData = GetComponentInParent<CustomerData>();
        if (m_targetCustomerData == null)
        {
            Debug.LogError("客コンポーネントが存在しません。親オブジェクトは客ですか？");
            Destroy(gameObject);
            return;
        }

        SetFoodImage();
    }


    private void Update()
    {
        Angry();
        Check();
    }

    private void SetFoodImage()
    {
        #region nullチェック
        if (m_targetCustomerData == null)
        {
            Debug.LogError("ターゲットの客が存在しません");
            return;
        }

        if (m_foodImage == null)
        {
            Debug.LogError("Imageがシリアライズされていません");
            return;
        }
        #endregion

        // ターゲットにしている料理が無ければ無視
        if (m_targetCustomerData.TargetOrderFoodData == null) return;

        var data = ItemDataBaseManager.instance.GetItemData(ItemInfo.ItemTypeID.Food, (uint)m_targetCustomerData.TargetOrderFoodData.FoodID);
        if (data == null) return;

        m_foodImage.sprite = data.ItemSprite;

    }

    /// <summary>
    /// @brief 怒りゲージの表示情報を更新する
    /// </summary>
    private void Angry()
    {
        #region nullチェック
        if (m_targetCustomerData == null)
        {
            Debug.LogError("ターゲットの客が存在しません");
            return;
        }

        if (m_imageProgressController == null)
        {
            Debug.LogError("ImageProgressControllerがシリアライズされていません");
            return;
        }
        #endregion

        float angryTime = m_targetCustomerData.AngryTime;
        float angryCount = m_targetCustomerData.AngryCount;

        m_imageProgressController.Progress = angryCount / angryTime;
    }

    /// <summary>
    /// @brief ターゲットの客が料理を待っている状態( CustomerState.WaitFood )でなければ削除する
    /// </summary>
    private void Check()
    {
        #region nullチェック
        if (m_targetCustomerData == null)
        {
            Debug.LogError("ターゲットの客が存在しません");
            return;
        }
        #endregion

        if(m_targetCustomerData.CurrentCustomerState!=CustomerState.WaitFood)
        {
            Destroy(gameObject);
        }
    }

}
