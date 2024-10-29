using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;
using FoodInfo;
using PocketItemDataInfo;
using TMPro;
using UniRx;

using Cysharp.Threading.Tasks;

public class ManagementProvideFoodSlotData : FoodSlotData
{
    // 制作者　田内
    // 提供料理スロット

    [Header("提供可能数")]
    [SerializeField]
    private TextMeshProUGUI m_provideNumText = null;

    [Header("売上数")]
    [SerializeField]
    private TextMeshProUGUI m_soldNumText = null;

    [Header("売上合計")]
    [SerializeField]
    private TextMeshProUGUI m_soldPriceText = null;

    //====================================================
    //               実行処理
    //====================================================

    public void Start()
    {
        m_pocketType = ManagementProvideFoodManager.instance.PocketType;

        // 提供料理データ変更イベントを受信
        MessageBroker.Default.Receive<GlobalChangeProvideFoodEvent>().Subscribe(_ =>
        {
            // 提供料理データに変更が加わるたびに更新する
            var data = ItemDataBaseManager.instance.GetItemData(m_itemTypeID, m_itemID);
            if (data == null) return;
            SetItemSlotData(data, m_pocketType);
        }).AddTo(this);

    }

    override public void SetItemSlotData(BaseItemData _itemData, PocketType _pocketType)
    {
        base.SetItemSlotData(_itemData, _pocketType);

        SetProvideFoodData();
    }


    override public void InitializeSlotData()
    {
        base.InitializeSlotData();

        SetSoldNumText(null, false);
        SetSoldPriceText(null, false);
        SetProvideNumText(null,false);
    }


    // 提供料理データを基に更新
    private void SetProvideFoodData()
    {
        // 提供料理マネージャーからデータを取得
        var data = ManagementProvideFoodManager.instance.GetProvideFood((FoodID)m_itemID);
        if (data == null) return;

        SetSoldNumText(data);
        SetSoldPriceText(data);
        SetProvideNumText(data);
    }


    private void SetProvideNumText(ProvideFoodData _data, bool _active = true)
    {
        if (m_provideNumText == null)
        {
            // 提供可能数がシリアライズされていません
            return;
        }

        // 非表示
        m_provideNumText.gameObject.SetActive(false);

        // 初期化用
        if (!_active) return;
        if (_data == null) return;

        m_provideNumText.text = _data.ProvideNum.ToString();

        m_provideNumText.gameObject.SetActive(true);
    }


    private void SetSoldNumText(ProvideFoodData _data, bool _active = true)
    {
        if (m_soldNumText == null)
        {
            // 売上数がシリアライズされていません
            return;
        }

        // 非表示
        m_soldNumText.gameObject.SetActive(false);

        // 初期化用
        if (!_active) return;
        if (_data == null) return;

        // 表示
        m_soldNumText.gameObject.SetActive(true);

        m_soldNumText.text = _data.SoldNum.ToString();

    }

    private void SetSoldPriceText(ProvideFoodData _data, bool _active = true)
    {
        if (m_soldPriceText == null)
        {
            // 売上がシリアライズされていません
            return;
        }

        // 非表示
        m_soldPriceText.gameObject.SetActive(false);

        // 初期化用
        if (!_active) return;
        if (_data == null) return;

        // 表示
        m_soldPriceText.gameObject.SetActive(true);


        var price = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)_data.FoodID)?.Price;

        m_soldPriceText.text = "￥ : " + (_data.SoldNum * price).ToString();

    }
}
