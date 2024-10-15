using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ConditionInfo;
using ItemInfo;
using PocketItemDataInfo;

using NaughtyAttributes;

public partial class ChangeItemDescription : MonoBehaviour
{

    // 制作者 田内
    // SelectUIControllerで選択中のItemSlotDataを基に説明文を変更できる他、
    // 外部から引数で対応するアイテムに変更することが可能


    [Header("ポケットマネージャーの種類")]
    [SerializeField]
    protected PocketType m_pocketType = PocketType.Inventory;

    //==========================================================


    [Header("UIを選択するコントローラー")]
    [SerializeField]
    protected SelectUIController m_selectUIController = null;


    //======================================================================
    //                          実行処理
    //======================================================================


    virtual public void OnInitialize()
    {
        // 初期化
        ChangeDesctiptionSelectUI();
    }


    virtual public void OnUpdate()
    {
        // 変更が加えられれば
        if (IsChangeDescription())
        {
            // 説明文を更新
            ChangeDesctiptionSelectUI();
        }
    }


    // 説明文を変更できるか確認
    protected bool IsChangeDescription()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return false;
        }
        #endregion

        return m_selectUIController.IsSelectChangeFlg;
    }


    /// <summary>
    /// 選択中のItemSlotDataを基に説明文を更新
    /// </summary>
    public void ChangeDesctiptionSelectUI()
    {
        // 説明文初期化
        InitDescription();
        SetActiveList();

        // 選択中のUIからItemSlotDataを取得
        ItemSlotData data = GetCurrentSelectItemSlotData();
        if (data == null) return;

        // 説明文更新
        ChangeDescription(m_pocketType, data.ItemTypeID, data.ItemID);

    }


    /// <summary>
    /// 引数アイテムIDを基に説明文を更新
    /// </summary>
    public void ChangeDescription(PocketType _pocketType, ItemTypeID _typeID, uint _itemID)
    {
        // 初期化
        InitDescription();

        // データ取得
        BaseItemData itemData = ItemDataBaseManager.instance.GetItemData(_typeID, _itemID);

        // 説明文を更新
        SetDescription(itemData);
        SetActiveList();
    }


    // 説明文のテキストをセットする
    virtual protected void SetDescription(BaseItemData _itemData)
    {
        if (_itemData == null) return;

        // 共通説明文
        SetCommonDescription(_itemData);

        // 食料説明文
        SetEdibleItemDescription(_itemData);

        // 料理説明文
        SetFoodDescription(_itemData);

        // ポケット説明文
        SetPocketDescription(_itemData);

    }


    // 説明文を初期化
    virtual protected void InitDescription()
    {
        // テキスト等を初期化

        // 共通説明文
        InitilizeCommonDescription();

        // 食料説明文
        InitilizeEdibleItemDescription();

        // 料理説明文
        InitilizeFoodDescription();

        // ポケット説明文
        InitilizePocketDescription();
    }

    virtual protected void SetActiveList()
    {
        // 共通説明文
        SetCommonActiveList();

        // 食料説明文
        SetEdibleItemActiveList();

        // 料理説明文
        SetFoodActiveList();

        // ポケット説明文
        SetPocketActiveList();

    }


    /// <summary>
    /// SelectUIControllerが選択しているItemSlotDataを取得 
    /// </summary>
    protected ItemSlotData GetCurrentSelectItemSlotData()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return null;
        }
        #endregion

        var currentSelectUI = m_selectUIController.CurrentSelectUI;
        if (currentSelectUI == null) return null;

        // アイテムデータを取得
        return currentSelectUI.GetComponent<ItemSlotData>();
    }


    /// <summary>
    /// 引数のアクティブを基にListのアクティブを更新する
    /// </summary>
    protected void CheckToSetActiveGameObjectList<T>(T _component, List<GameObject> _list) where T : MonoBehaviour
    {
        if (_component == null) return;

        bool isActive = _component.gameObject.activeSelf;

        foreach (var obj in _list)
        {
            obj.SetActive(isActive);
        }
    }


}
