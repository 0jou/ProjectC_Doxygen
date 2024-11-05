using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OrderFoodInfo;

public class CounterPointData : MonoBehaviour
{
    // 制作者
    // カウンター用ポイント

    [Header("設置ポイント")]
    [SerializeField]
    private Transform m_setPoint = null;

    public Transform SetPoint
    {
        get
        {
            if (m_setPoint != null)
            {
                return m_setPoint;
            }
            else
            {
                return gameObject.transform;
            }
        }
    }

    [Header("目的地ポイント")]
    [SerializeField]
    private Transform m_destinationPoint = null;

    public Transform DestinationPoint
    {
        get
        {
            if (m_destinationPoint != null)
            {
                return m_destinationPoint;
            }
            else
            {
                return gameObject.transform;
            }
        }
    }

    //=============================
    // 設置されているか

    private OrderFoodData m_setOrderFoodData = null;

    public OrderFoodData SetOrderFoodData
    {
        get { return m_setOrderFoodData; }
        set { m_setOrderFoodData = value; }
    }

    //===========================================
    //            実行処理
    //===========================================

    private void Update()
    {
        CheckOrderFoodData();
    }


    // ターゲットの料理を手放すか確認
    private void CheckOrderFoodData()
    {
        if (m_setOrderFoodData == null) return;

        switch (m_setOrderFoodData.CurrentOrderFoodState)
        {
            // カウンターで待機状態であれば
            case OrderFoodState.WaitHallStaff:
            case OrderFoodState.WaitCarry:
                {
                    break;
                }

                // それ以外であれば料理を手放す
            default:
                {
                    m_setOrderFoodData = null;
                    break;
                }
        }
    }


}
