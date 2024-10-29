using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ConditionInfo;

using NaughtyAttributes;

public partial class ChangeItemDescription : MonoBehaviour
{
    // 制作者 田内
    // 食料系アイテム


    //==========================================================

    [Foldout("食料")]
    [Header("-------------------------------------------------------")]
    [Header("回復量Text")]
    [SerializeField]
    protected TextMeshProUGUI m_healingValueTextMeshPro = null;

    [Foldout("食料")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_healingValueList = new();


    //==========================================================

    [Foldout("食料")]
    [Header("-------------------------------------------------------")]
    [Header("状態画像Image")]
    [SerializeField]
    protected Image m_conditionImage = null;

    [Foldout("食料")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_conditionImageList = new();

    //==========================================================

    [Foldout("食料")]
    [Header("-------------------------------------------------------")]
    [Header("状態画像CreateConditionImage")]
    [SerializeField]
    protected CreateConditionImage m_createConditionImage = null;

    [Foldout("食料")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_createConditionImageList = new();

    //==========================================================

    [Foldout("食料")]
    [Header("-------------------------------------------------------")]
    [Header("状態説明文Text")]
    [SerializeField]
    protected TextMeshProUGUI m_conditionText = null;

    [Foldout("食料")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_conditionTextList = new();

    //==========================================================

    // 追加（吉田）

    [Foldout("食料")]
    [Header("-------------------------------------------------------")]
    [Header("状態 背景色変更 Image")]
    [SerializeField]
    protected Image m_conditionBackColor = null;

    [Foldout("食料")]
    [Header("状態 背景色 透明度")]
    [Range(0, 1)]
    [SerializeField]
    protected float m_conditionBackAlpha = 0.2f;


    //==========================================================
    //                      実行処理
    //==========================================================


    private void SetEdibleItemDescription(BaseItemData _itemData)
    {
        // 回復量テキスト
        SetHealingValueText(_itemData);

        // 状態画像
        SetConditionImage(_itemData);

        // 状態画像(複数)
        CreateConditionImage(_itemData);

        // 状態テキスト
        SetConditionText(_itemData);

        // 状態背景色要素
        SetConditionBackColor(_itemData);
    }

    private void InitilizeEdibleItemDescription()
    {

        // 回復量テキスト
        SetHealingValueText(null, false);

        // 状態画像
        SetConditionImage(null, false);

        // 状態画像(複数)
        CreateConditionImage(null, false);

        // 状態テキスト
        SetConditionText(null, false);

        // 状態背景色要素
        SetConditionBackColor(null, false);
    }

    private void SetEdibleItemActiveList()
    {
        CheckToSetActiveGameObjectList(m_healingValueTextMeshPro, m_healingValueList);

        CheckToSetActiveGameObjectList(m_createConditionImage, m_createConditionImageList);

        CheckToSetActiveGameObjectList(m_conditionImage, m_conditionImageList);

        CheckToSetActiveGameObjectList(m_conditionText, m_conditionTextList);

    }





    // 回復量をセット(材料・料理のみ)
    virtual protected void SetHealingValueText(BaseItemData _itemData, bool _active = true)
    {
        if (m_healingValueTextMeshPro == null) return;

        // 非表示
        m_healingValueTextMeshPro.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;

        // 食料アイテムデータを取得
        var edibleData = CastEdibleItem(_itemData);
        if (edibleData == null) return;

        switch (edibleData.ConditionID)
        {
            case ConditionID.Normal:
                {
                    m_healingValueTextMeshPro.text = edibleData.HealingValue.ToString();
                    break;
                }
            default:
                {
                    m_healingValueTextMeshPro.text = "ー";
                    break;
                }
        }

        // 表示
        m_healingValueTextMeshPro.gameObject.SetActive(true);

    }


    // 状態をセット
    virtual protected void SetConditionImage(BaseItemData _itemData, bool _active = true)
    {
        if (m_conditionImage == null) return;

        // 非表示
        m_conditionImage.gameObject.SetActive(false);

        // 初期化用
        if (_active==false) return;
        if (_itemData == null) return;

        // 食料アイテムデータを取得
        var edibleData = CastEdibleItem(_itemData);
        if (edibleData == null) return;


        var data= ConditionDataBaseManager.instance.GetConditionData(edibleData.ConditionID);
        if (data == null) return;

        // 画像をセット
        m_conditionImage.sprite = data.ConditionSprite;

        // 表示
        m_conditionImage.gameObject.SetActive(true);

    }


    // 状態をセット(材料・料理のみ)
    virtual protected void CreateConditionImage(BaseItemData _itemData, bool _active = true)
    {
        if (m_createConditionImage == null) return;

        // 非表示
        m_createConditionImage.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (_itemData == null) return;


        // 画像を作成
        if (m_createConditionImage.CreateImage(_itemData))
        {
            // 表示
            m_createConditionImage.gameObject.SetActive(true);
        }

    }


    // 状態の説明文をセット(材料・料理のみ)
    virtual protected void SetConditionText(BaseItemData _itemData, bool _active = true)
    {
        if (m_conditionText == null) return;

        // 非表示
        m_conditionText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;

        // 食料アイテムデータを取得
        var edibleData = CastEdibleItem(_itemData);
        if (edibleData == null) return;

        switch (edibleData.ConditionID)
        {
            case ConditionID.Normal:
                {
                    // 表示しない
                    return;
                }

            default:
                {
                    var conditionData = ConditionDataBaseManager.instance.GetConditionData(edibleData.ConditionID);
                    if (conditionData == null) return;

                    // テキストをセット
                    m_conditionText.text = conditionData.ConditionDescriptionText;

                    // 表示
                    m_conditionText.gameObject.SetActive(true);

                    break;
                }
        }
    }


    // 状態の背景色を変更（吉田）
    virtual protected void SetConditionBackColor(BaseItemData _itemData, bool _active = true)
    {
        if (m_conditionBackColor == null) return;

        // 非表示
        m_conditionBackColor.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;

        // 食料アイテムデータを取得
        var edibleData = CastEdibleItem(_itemData);
        if (edibleData == null) return;


        var conditionData = ConditionDataBaseManager.instance.GetConditionData(edibleData.ConditionID);
        if (conditionData == null) return;


        if (conditionData.ConditionColor == Color.white)
        {
            Debug.LogError("この状態の色は初期値と同じ 白 です。処理しません。");
            return;
        }

        // 色をセット
        Color backColor = new(
            conditionData.ConditionColor.r,
            conditionData.ConditionColor.g,
            conditionData.ConditionColor.b,
            m_conditionBackAlpha);
        m_conditionBackColor.color = backColor;

        // 表示
        m_conditionBackColor.gameObject.SetActive(true);
    }



    /// <summary>
    /// 引数アイテムデータを食料データにキャストする
    /// </summary>
    protected BaseEdibleItem CastEdibleItem(BaseItemData _itemData)
    {
        if (_itemData == null) return null;
        if (_itemData is BaseEdibleItem == false) return null;

        // 食料アイテムデータを取得
        return _itemData as BaseEdibleItem;
    }




}
