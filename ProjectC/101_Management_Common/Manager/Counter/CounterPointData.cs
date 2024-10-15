using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterPointData : MonoBehaviour
{
    [Header("設置ポイント")]
    [SerializeField]
    private Transform m_setPoint = null;

    public Transform SetPoint
    {
        get { return m_setPoint; }
    }

    [Header("目的地ポイント")]
    [SerializeField]
    private Transform m_destinationPoint = null;

    public Transform DestinationPoint
    {
        get { return m_destinationPoint; }
    }

    //=============================
    // 設置されているか

    private bool m_isSet = false;

    public bool IsSet
    {
        get { return m_isSet; }
        set { m_isSet = value; }
    }
}
