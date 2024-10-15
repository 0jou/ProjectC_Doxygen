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

    [Foldout("アイテム画像")]
    [Header("アイテムの見た目を表示するImage")]
    [SerializeField]
    protected Image m_itemImage = null;


    //==============================================

    [Foldout("アイテム名")]
    [Header("アイテムの名前を表示するTextMeshPro(UI)")]
    [SerializeField]
    protected TextMeshProUGUI m_nameText = null;

    //==============================================

    [Foldout("アイテム説明文")]
    [Header("説明文を表示するTextMeshPro(UI)")]
    [SerializeField]
    protected TextMeshProUGUI m_descriptionText = null;


    //==============================================

    [Foldout("アイテム効果")]
    [Header("アイテムの効果を表示するImage")]
    [SerializeField]
    protected Image m_conditionImage = null;


    //==============================================

    [Foldout("アイテム所持数")]
    [Header("アイテム所持数を表示するTextMeshPro(UI)")]
    [SerializeField]
    protected TextMeshProUGUI m_numText = null;



    //==============================================
    // 割り当てられているアイテムの種類

    protected ItemTypeID m_itemTypeID = ItemTypeID.Ingredient;

    virtual public ItemTypeID ItemTypeID { get { return m_itemTypeID; } }


    //==============================================
    // 割り当てられているアイテムの種類→種類

    protected uint m_itemID = new();

    virtual public uint ItemID { get { return m_itemID; } }

    //==============================================
    // ポケットの種類

    [Header("ポケット種類")]
    [SerializeField]
    protected PocketType m_pocketType = PocketType.Inventory;

    public PocketType PocketType
    {
        get { return m_pocketType; }
    }

    //==========================================================

    // 追加（吉田）
    [Foldout("アイテム状態 背景色変更")]
    [Header("状態 背景色変更 Image")]
    [SerializeField]
    protected Image m_conditionBackColor = null;

    [Foldout("アイテム状態 背景色変更")]
    [Header("状態 背景色 透明度")]
    [Range(0, 1)]
    [SerializeField]
    protected float m_conditionBackAlpha = 0.2f;

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

        // 状態をセット
        SetConditionImage(_itemData);

        // 所持数テキストをセット
        SetItemNum();

        // 状態の背景色を変更（吉田）
        SetConditionBackColor(_itemData);

    }

    // 初期化する
    virtual public void InitializeSlotData()
    {
        // 名前をセット
        SetItemName("", false);

        // アイテム画像をセット
        SetItemImage(null, false);

        // 説明文をセット
        SetDescription("", false);

        // 状態をセット
        SetConditionImage(null, false);

        // 数テキストをセット
        SetItemNum(false);

        // 状態の背景色をセット（吉田）
        SetConditionBackColor(null, false);

    }

    //======================================================================


    // アイテムの画像をセット
    virtual protected void SetItemImage(Sprite _sprite, bool _active = true)
    {
        if (m_itemImage == null)
        {
            Debug.Log("アイテムの見た目を表示するイメージが存在しません");
            return;
        }

        // 非表示
        m_itemImage.gameObject.SetActive(false);

        // 初期化用
        if (!_active) return;

        if (_sprite == null)
        {
            Debug.Log("引数Spriteが存在しません");
            return;
        }

        // 表示
        m_itemImage.gameObject.SetActive(true);

        // 画像をセット
        m_itemImage.sprite = _sprite;

    }


    // 名前をセット
    virtual protected void SetItemName(string _nameText, bool _active = true)
    {
        if (m_nameText == null)
        {
            Debug.Log("アイテムの見た目を表示するイメージが存在しません");
            return;
        }

        // 表示
        m_nameText.gameObject.SetActive(false);

        // 初期化用
        if (!_active) return;

        // 表示
        m_nameText.gameObject.SetActive(true);

        // 名前テキストをセット
        m_nameText.text = _nameText;

    }


    virtual protected void SetDescription(string _description, bool _active = true)
    {
        if (m_descriptionText == null)
        {
            Debug.Log("アイテムの説明文を表示するテキストが存在しません");
            return;
        }

        // 非表示
        m_descriptionText.gameObject.SetActive(false);

        // 初期化用
        if (!_active) return;

        m_descriptionText.gameObject.SetActive(true);

        // 説明文をセット
        m_descriptionText.text = _description;

    }


    // 保持数をセット
    virtual protected void SetItemNum(bool _active = true)
    {
        if (m_numText == null)
        {
            Debug.Log("アイテムの所持数を表示するテキストが存在しません");
            return;
        }

        // 表示
        m_numText.gameObject.SetActive(false);

        // 初期化用
        if (!_active) return;

        // 所持数
        var num = m_pocketType.GetPocketItemDataManager().GetItemNum(m_itemTypeID, m_itemID);

        // 0を表示しない設定であれば

        if (num < 1)
        {
            return;
        }

        // 表示
        m_numText.gameObject.SetActive(true);


        m_numText.text = num.ToString();

    }


    // 状態をセット(材料・料理のみ)
    virtual protected void SetConditionImage(BaseItemData _itemData, bool _active = true)
    {
        if (m_conditionImage == null)
        {
            Debug.Log("アイテムの状態を表示するイメージが存在しません");
            return;
        }

        // 非表示
        m_conditionImage.gameObject.SetActive(false);

        // 初期化用
        if (!_active) return;

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
            // 表示
            m_conditionImage.gameObject.SetActive(true);

            // 画像をセット
            m_conditionImage.sprite = sprite;
        }

    }

    // 状態の背景色を変更（吉田）
    virtual protected void SetConditionBackColor(BaseItemData _itemData, bool _active = true)
    {
        if (m_conditionBackColor == null)
        {
            Debug.Log("アイテムの状態の色を変更する背景imgaeがシリアライズされていません");
            return;
        }

        m_conditionBackColor.gameObject.SetActive(false);

        // 初期化用
        if (!_active) return;


        if (_itemData == null)
        {
            Debug.Log("引数アイテムデータが存在しません");
            return;
        }

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
