using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

using PocketItemDataInfo;

using NaughtyAttributes;

public partial class ChangeItemDescription : MonoBehaviour
{
    // 制作者 田内
    // 共通


    //==========================================================


    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("アイテム画像Image")]
    [SerializeField]
    private Image m_itemImage = null;


    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_itemImageList = new();


    //==============================================================


    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("アイテム名Text")]
    [SerializeField]
    protected TextMeshProUGUI m_nameTextMeshPro = null;


    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_nameList = new();

    //==========================================================

    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("説明文Text")]
    [SerializeField]
    protected TextMeshProUGUI m_descriptionTextMeshPro = null;


    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_descriptionList = new();



    //===========================================================


    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("アイテム種類画像")]
    [SerializeField]
    protected Image m_typeImage = null;

    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_typeImageList = new();

    //==========================================================

    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("アイテム種類Text")]
    [SerializeField]
    protected TextMeshProUGUI m_typeTextMeshPro = null;

    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_typeList = new();

    //==========================================================

    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("最大保持数")]
    [SerializeField]
    protected TextMeshProUGUI m_maxNumText = null;


    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_maxNumTextList = new();

    //==========================================================

    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("レベルText")]
    [SerializeField]
    protected TextMeshProUGUI m_levelTextMeshPro = null;


    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_levelList = new();


    //==========================================================

    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("総所持数Text")]
    [SerializeField]
    protected TextMeshProUGUI m_numTextMeshPro = null;


    protected enum DisplayOneType
    {
        Disable,
        Enable,
    }


    [Foldout("共通")]
    [Header("0を表示するか")]
    [SerializeField]
    protected DisplayOneType m_displayOneType = DisplayOneType.Enable;


    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_numList = new();

    //======================================================================
    //                          実行処理
    //======================================================================

    private void SetCommonDescription(BaseItemData _itemData)
    {
        // アイテムの画像をセット
        SetItemImage(_itemData);

        // 種類画像をセット
        SetTypeImage(_itemData);

        // 種類テキストをセット
        SetTypeText(_itemData);

        // 名前テキストをセット
        SetNameText(_itemData);

        // 説明文テキストをセット
        SetDescriptionText(_itemData);

        // 最大所持数
        SetMaxNumText(_itemData);

        // レベルテキストをセット
        SetLevelText(_itemData);

        // 所持数を表示
        SetNumText(_itemData);

    }

    private void InitilizeCommonDescription()
    {
        // アイテム画像
        SetItemImage(null, false);

        // 種類画像
        SetTypeImage(null, false);

        // 種類テキスト
        SetTypeText(null, false);

        // 名前
        SetNameText(null, false);

        // 説明
        SetDescriptionText(null, false);

        // 最大所持数
        SetMaxNumText(null, false);

        // レベル
        SetLevelText(null, false);

        // 所持数
        SetNumText(null, false);
    }

    private void SetCommonActiveList()
    {
        // アイテム画像
        CheckToSetActiveGameObjectList(m_itemImage, m_itemImageList);

        // 種類
        CheckToSetActiveGameObjectList(m_typeTextMeshPro, m_typeList);

        // 種類画像
        CheckToSetActiveGameObjectList(m_typeImage, m_typeImageList);

        // 名前
        CheckToSetActiveGameObjectList(m_nameTextMeshPro, m_nameList);

        // 説明
        CheckToSetActiveGameObjectList(m_descriptionTextMeshPro, m_descriptionList);

        // 最大所持数
        CheckToSetActiveGameObjectList(m_maxNumText, m_maxNumTextList);

        // レベル
        CheckToSetActiveGameObjectList(m_levelTextMeshPro, m_levelList);

        // 所持数
        CheckToSetActiveGameObjectList(m_numTextMeshPro, m_numList);

    }



    // アイテム画像
    virtual protected void SetItemImage(BaseItemData _itemData, bool _active = true)
    {
        if (m_itemImage == null) return;

        // 非表示
        m_itemImage.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (_itemData == null) return;

        // 画像セット
        m_itemImage.sprite = _itemData.ItemSprite;

        // 表示
        m_itemImage.gameObject.SetActive(true);

    }


    // アイテム種類テキスト
    virtual protected void SetTypeText(BaseItemData _itemData, bool _active = true)
    {
        if (m_typeTextMeshPro == null) return;

        // 非表示
        m_typeTextMeshPro.gameObject.SetActive(false);

        // 初期化用
        if (!_active) return;

        if (_itemData == null)
        {
            Debug.Log("引数アイテムデータが存在しません");
            return;
        }

        var data = ItemTypeDataBaseManager.instance.GetItemTypeData(_itemData.ItemTypeID);
        if (data == null) return;


        // 名前テキストをセット
        m_typeTextMeshPro.text = data.ItemTypeName;

        // 表示
        m_typeTextMeshPro.gameObject.SetActive(true);

    }


    // アイテム種類画像
    virtual protected void SetTypeImage(BaseItemData _itemData, bool _active = true)
    {
        if (m_typeImage == null) return;

        // 非表示
        m_typeImage.gameObject.SetActive(false);

        // 初期化用
        if (!_active) return;

        if (_itemData == null)
        {
            Debug.Log("引数アイテムデータが存在しません");
            return;
        }

        var data = ItemTypeDataBaseManager.instance.GetItemTypeData(_itemData.ItemTypeID);
        if (data?.ItemTypeSprite == null) return;


        // 名前テキストをセット
        m_typeImage.sprite = data.ItemTypeSprite;

        // 表示
        m_typeImage.gameObject.SetActive(true);

    }


    // 最大保持数テキスト
    virtual protected void SetMaxNumText(BaseItemData _itemData, bool _active = true)
    {
        if (m_maxNumText == null) return;

        // 非表示
        m_maxNumText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (_itemData == null) return;

        // 名前テキストをセット
        m_maxNumText.text = _itemData.MaxNum.ToString();

        // 表示
        m_maxNumText.gameObject.SetActive(true);

    }

    // アイテム名テキスト
    virtual protected void SetNameText(BaseItemData _itemData, bool _active = true)
    {
        if (m_nameTextMeshPro == null) return;

        // 非表示
        m_nameTextMeshPro.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (_itemData == null) return;

        // 名前テキストをセット
        m_nameTextMeshPro.text = _itemData.ItemName;

        // 表示
        m_nameTextMeshPro.gameObject.SetActive(true);

    }



    // 説明文テキスト
    virtual protected void SetDescriptionText(BaseItemData _itemData, bool _active = true)
    {
        if (m_descriptionTextMeshPro == null) return;

        // 非表示
        m_descriptionTextMeshPro.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (_itemData == null) return;

        // 説明文をセット
        m_descriptionTextMeshPro.text = _itemData.ItemDescriptionText;

        // 表示
        m_descriptionTextMeshPro.gameObject.SetActive(true);

    }



    // 総所持数テキスト
    virtual protected void SetNumText(BaseItemData _itemData, bool _active = true)
    {
        if (m_numTextMeshPro == null) return;

        // 非表示
        m_numTextMeshPro.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (_itemData == null) return;

        var num = m_pocketType.GetPocketItemDataManager().GetItemNum(_itemData.ItemTypeID, _itemData.ItemID);
       

        // 0を表示しない設定であれば
        if (m_displayOneType == DisplayOneType.Disable)
        {
            if (num < 1) return;
        }

        // 保持数をセット
        m_numTextMeshPro.text = num.ToString();

        // 表示
        m_numTextMeshPro.gameObject.SetActive(true);

    }


    // レベルテキスト
    virtual protected void SetLevelText(BaseItemData _itemData, bool _active = true)
    {
        if (m_levelTextMeshPro == null) return;

        // 非表示
        m_levelTextMeshPro.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (_itemData == null) return;
        if (_itemData.ItemLevel <= 0) return;

        // レベルをセット
        m_levelTextMeshPro.text = _itemData.ItemLevel.ToString();

        // 表示
        m_levelTextMeshPro.gameObject.SetActive(true);
    }


}
