using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSetData : MonoBehaviour
{
    // 客が座ったりするテーブル・椅子をまとめたデータ
    // 制作者　田内


    //==============================================

    [Header("テーブル(料理を設置する)ポイント")]
    [SerializeField]
    private Transform m_tablePoint = null;

    public Transform TablePoint
    {
        get { return m_tablePoint; }
    }


    //==============================================

    [Header("椅子ポイント")]
    [SerializeField]
    private Transform m_chairPoint = null;

    public Transform ChairPoint
    {
        get { return m_chairPoint; }
    }

    //==============================================

    [Header("目的地ポイント")]
    [SerializeField]
    private Transform m_destinationPoint = null;

    public Transform DestinationPoint
    {
        get { return m_destinationPoint; }
    }

    //=================================
    // 座られているか

    private GameObject m_sitObject = null;

    public GameObject SitObject
    {
        get { return m_sitObject; }
        set { m_sitObject = value; }
    }

}
