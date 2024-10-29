using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using TMPro;
using UnityEngine.UI;
using ConditionInfo;
using PocketItemDataInfo;
using NaughtyAttributes;

public class ItemSlotData : MonoBehaviour
{

    // 制作者 田内


    //==============================================
    // ポケットの種類

    [Header("ポケット種類")]
    [SerializeField]
    protected PocketType m_pocketType = PocketType.Inventory;

    public PocketType PocketType
    {
        get { return m_pocketType; }
    }


    //==============================================

    [Foldout("共通")]
    [Header("=====================================")]
    [Header("アイテム画像")]
    [SerializeField]
    protected Image m_itemImage = null;


    //==============================================

    [Foldout("共通")]
    [Header("=====================================")]
    [Header("アイテム名")]
    [SerializeField]
    protected TextMeshProUGUI m_nameText = null;

    //==============================================

    [Foldout("共通")]
    [Header("=====================================")]
    [Header("説明文")]
    [SerializeField]
    protected TextMeshProUGUI m_descriptionText = null;

    //==============================================

    [Foldout("共通")]
    [Header("=====================================")]
    [Header("アイテム種類Image")]
    [SerializeField]
    protected Image m_typeImage = null;

    //==============================================

    [Foldout("共通")]
    [Header("=====================================")]
    [Header("アイテム所持数")]
    [SerializeField]
    protected TextMeshProUGUI m_numText = null;



    //==============================================

    [Foldout("アイテム効果")]
    [Header("=====================================")]
    [Header("アイテムの効果を表示するImage")]
    [SerializeField]
    protected Image m_conditionImage = null;



    //==========================================================

    // 追加（吉田）
    [Foldout("アイテム状態 背景色変更")]
    [Header("=====================================")]
    [Header("状態 背景色変更 Image")]
    [SerializeField]
    protected Image m_conditionBackColor = null;

    [Foldout("アイテム状態 背景色変更")]
    [Header("状態 背景色 透明度")]
    [Range(0, 1)]
    [SerializeField]
    protected float m_conditionBackAlpha = 0.2f;



    //=================================
    // 割り当てられているアイテムの種類

    protected ItemTypeID m_itemTypeID = ItemTypeID.Ingredient;

    virtual public ItemTypeID ItemTypeID { get { return m_itemTypeID; } }


    //=======================================
    // 割り当てられているアイテムID

    protected uint m_itemID = new();

    virtual public uint ItemID { get { return m_itemID; } }

    //======================================================================
    //                          実行処理
    //======================================================================


    #region メソッド説明
    /// <summary>
    // アイテムスロットのデータをセットする
    /// </summary>
    #endregion
    // アイテムスロットのデータをセットする
    virtual public void SetItemSlotData(BaseItemData _itemData, PocketType _pocketType)
    {
        if (_itemData == null)
        {
            Debug.LogError("このアイテムは登録されていません");
            return;
        }

        m_itemTypeID = _itemData.ItemTypeID;

        m_itemID = _itemData.ItemID;

        m_pocketType = _pocketType;

        // 名前をセット
        SetItemName(_itemData.ItemName);

        // 説明文をセット
        SetDescription(_itemData.ItemDescriptionText);

        // アイテム画像をセット
        SetItemImage(_itemData.ItemSprite);

        // アイテム種類画像をセット
        SetItemTypeImage();

        // 状態をセット
        SetConditionImage(_itemData);

        // 所持数テキストをセット
        SetItemNum();

        // 状態の背景色を変更（吉田）
        SetConditionBackColor(_itemData);

    }


    virtual public void InitializeSlotData()
    {
        // 名前をセット
        SetItemName("", false);

        // アイテム画像をセット
        SetItemImage(null, false);

        // アイテム種類画像をセット
        SetItemTypeImage(false);

        // 説明文をセット
        SetDescription("", false);

        // 状態をセット
        SetConditionImage(null, false);

        // 数テキストをセット
        SetItemNum(false);

        // 状態の背景色をセット（吉田）
        SetConditionBackColor(null, false);

    }


    // アイテムの画像をセット
    virtual protected void SetItemImage(Sprite _sprite, bool _active = true)
    {
        if (m_itemImage == null) return;

        // 非表示
        m_itemImage.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;

        if (_sprite == null)
        {
            Debug.Log("引数Spriteが存在しません");
            return;
        }

        // 画像をセット
        m_itemImage.sprite = _sprite;

        // 表示
        m_itemImage.gameObject.SetActive(true);

    }


    // 名前をセット
    virtual protected void SetItemName(string _nameText, bool _active = true)
    {
        if (m_nameText == null) return;

        // 表示
        m_nameText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;

        // 名前テキストをセット
        m_nameText.text = _nameText;

        // 表示
        m_nameText.gameObject.SetActive(true);

    }


    virtual protected void SetDescription(string _description, bool _active = true)
    {
        if (m_descriptionText == null) return;

        // 非表示
        m_descriptionText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;

        // 説明文をセット
        m_descriptionText.text = _description;

        m_descriptionText.gameObject.SetActive(true);

    }


    // 保持数をセット
    virtual protected void SetItemNum(bool _active = true)
    {
        if (m_numText == null) return;

        // 表示
        m_numText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;

        // 所持数
        var num = m_pocketType.GetPocketItemDataManager().GetItemNum(m_itemTypeID, m_itemID);

        // 0を表示しない設定であれば

        if (num < 1)
        {
            return;
        }

        m_numText.text = num.ToString();

        // 表示
        m_numText.gameObject.SetActive(true);

    }


    // アイテム種類
    virtual protected void SetItemTypeImage(bool _active = true)
    {
        if (m_typeImage == null) return;

        // 非表示
        m_typeImage.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;

        var data = ItemTypeDataBaseManager.instance.GetItemTypeData(m_itemTypeID);
        if (data == null) return;

        // 画像をセット
        m_typeImage.sprite = data.ItemTypeSprite;

        // 表示
        m_typeImage.gameObject.SetActive(true);
    }


    // 状態をセット(材料・料理のみ)
    virtual protected void SetConditionImage(BaseItemData _itemData, bool _active = true)
    {
        if (m_conditionImage == null) return;

        // 非表示
        m_conditionImage.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;

        if (_itemData == null)
        {
            Debug.Log("引数アイテムデータが存在しません");
            return;
        }

        if (_itemData is BaseEdibleItem == false)
        {
            Debug.Log("食料アイテムにキャスト出来ません");
            return;
        }
        var edibleData = _itemData as BaseEdibleItem;
        ConditionID conditionID = edibleData.ConditionID;



        Sprite sprite = ConditionDataBaseManager.instance.GetConditionData(conditionID).ConditionSprite;

        if (sprite != null)
        {
            // 画像をセット
            m_conditionImage.sprite = sprite;

            // 表示
            m_conditionImage.gameObject.SetActive(true);

        }

    }

    // 状態の背景色を変更（吉田）
    virtual protected void SetConditionBackColor(BaseItemData _itemData, bool _active = true)
    {
        if (m_conditionBackColor == null) return;

        m_conditionBackColor.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (_itemData == null) return;

        // キャストできるか確認
        if (_itemData is BaseEdibleItem == false)
        {
            Debug.Log("食料アイテムにキャスト出来ません");
            return;
        }

        var edibleData = _itemData as BaseEdibleItem;
        ConditionID conditionID = edibleData.ConditionID;
        var data = ConditionDataBaseManager.instance.GetConditionData(conditionID);
        if (data == null)
        {
            Debug.LogError("この状態データは存在しません");
            return;
        }

        if (data.ConditionColor == Color.white)
        {
            Debug.LogError("この状態の色は初期値と同じ 白 です。処理しません。");
            return;
        }

        // 色をセット
        Color backColor = new(
            data.ConditionColor.r,
            data.ConditionColor.g,
            data.ConditionColor.b,
            m_conditionBackAlpha);
        m_conditionBackColor.color = backColor;

        // 表示
        m_conditionBackColor.gameObject.SetActive(true);
    }


}
