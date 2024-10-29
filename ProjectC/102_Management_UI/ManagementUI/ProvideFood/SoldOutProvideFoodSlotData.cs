using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using UniRx;

public class SoldOutProvideFoodSlotData : ManagementProvideFoodSlotData
{
    // 制作者　田内
    // 提供料理スロット


    [Header("売り切れカラー")]
    [SerializeField]
    private Color m_soldOutColor = Color.gray;

    //================================================
    //               実行処理
    //================================================

    public void Start()
    {

        base.Start();

        // 売り切れイベントを受信
        MessageBroker.Default.Receive<GlobalSoldOutProvideFoodEvent>().Subscribe(_ =>
        {
            if (m_itemID == (uint)_.ProvideFoodData.FoodID)
            {
                SoldOut();
            }
        }).AddTo(this);

    }



    // 売り切れ処理
    private void SoldOut()
    {
        var images = gameObject.GetComponentsInChildren<Image>();

        foreach (var image in images)
        {
            image.color = m_soldOutColor;
        }
    }

}
