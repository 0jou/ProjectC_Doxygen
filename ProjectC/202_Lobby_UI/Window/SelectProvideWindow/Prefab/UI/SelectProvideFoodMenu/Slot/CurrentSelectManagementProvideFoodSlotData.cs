using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;
using PocketItemDataInfo;
using FoodInfo;
using TMPro;
using NaughtyAttributes;

public class CurrentSelectManagementProvideFoodSlotData : FoodSlotData
{
    // 制作者　田内
    // 提供料理(確認)スロット

    [Header("SelectManagementProvideFoodController")]
    [SerializeField]
    protected SelectManagementProvideFoodController m_managementProvideFoodController = null;

    [Header("レシピウィンドウのUISelectコントローラー")]
    [SerializeField]
    protected SelectUIController m_recipeSelectUIController = null;

    [Header("キャンバスグループ")]
    [SerializeField]
    protected CanvasGroup m_canvasGroup = null;

    [Header("表示時の透明度")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    protected float m_alpha = 0.5f;

    //===================================================

    [Foldout("Color")]
    [Header("色を変更したいUIList")]
    [SerializeField]
    private List<TextMeshProUGUI> m_colorTextList = new();

    [Foldout("Color")]
    [Header("作成可能状態のカラー")]
    [SerializeField]
    protected Color m_possibleColor = Color.white;

    [Foldout("Color")]
    [Header("作成不可能状態のカラー")]
    [SerializeField]
    protected Color m_inpossibleColor = Color.red;


    //============================================================================
    // 実行処理
    //============================================================================


    public void Start()
    {
        m_pocketType = ManagementProvideFoodManager.instance.PocketType;
    }

    public void OnUpdate()
    {
        Check();
    }


    // 存在するか確認
    private void Check()
    {
        #region nullチェック

        if (m_managementProvideFoodController == null)
        {
            Debug.LogError("ManagementProvideFoodControllerがシリアライズされていません");
            return;
        }

        if (m_recipeSelectUIController == null)
        {
            Debug.LogError("SelectUIコントローラーがシリアライズされていません");
            return;
        }

        #endregion

        // UI選択コントローラーまたは提供料理操作コントローラーに変更が加われば更新
        if (m_recipeSelectUIController.IsSelectChangeFlg ||
            m_managementProvideFoodController.IsListChange)
        {
            // 初期化
            InitSlot();

            // 更新
            SetSlot();
        }
    }


    // 初期化
    private void InitSlot()
    {
        #region nullチェック

        if (m_recipeSelectUIController == null)
        {
            Debug.LogError("SelectUIコントローラーがシリアライズされていません");
            return;
        }

        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupがシリアライズされていません");
            return;
        }

        #endregion

        // 非表示に
        m_canvasGroup.alpha = 0.0f;
    }

    private void SetSlot()
    {
        #region nullチェック

        if(m_managementProvideFoodController==null)
        {
            Debug.LogError("ManagementProvideFoodControllerがシリアライズされていません");
            return;
        }

        if (m_recipeSelectUIController == null)
        {
            Debug.LogError("SelectUIコントローラーがシリアライズされていません");
            return;
        }

        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupがシリアライズされていません");
            return;
        }

        #endregion

        // 追加できなければ
        if (m_managementProvideFoodController.IsAddList() == false) return;

        // 選択中のUIから取得
        if (m_recipeSelectUIController.CurrentSelectUI.TryGetComponent<RecipeItemSlotData>(out var itemSlotData))
        {
            // 提供料理に既に追加されていれば
            if (m_managementProvideFoodController.IsAddedProvideFood((FoodID)itemSlotData.ItemID) == true) return;

            // 表示
            m_canvasGroup.alpha = m_alpha;

            // 更新する
            var itemData = ItemDataBaseManager.instance.GetItemData(ItemTypeID.Food, itemSlotData.ItemID);
            SetItemSlotData(itemData, m_pocketType);

            // 色をセット
            SetColor(itemSlotData);
        }
    }


    // 色要素を変更
    private void SetColor(RecipeItemSlotData _slotData)
    {

        if (_slotData.IsCreate == false)
        {
            // 作成できなければ
            foreach (var ui in m_colorTextList)
            {
                ui.color = m_inpossibleColor;
            }
        }
        else
        {
            // 作成できれば
            foreach (var ui in m_colorTextList)
            {
                ui.color = m_possibleColor;
            }
        }
    }

}
