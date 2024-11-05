using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StaffInfo;

namespace StaffInfo
{
    public enum HallStaffState
    {
        Default,        // 通常の状態
        MoveGetFood,    // 料理を取りに行く状態
        CarryFood,      // 料理を運んでいる状態
        MoveDefault,    // デフォルトの位置に移動する状態
        Stun,           // 攻撃されている状態
        Set,            // 料理をセットしている状態
    }

    public enum ChefStaffState
    {

        Find = 0,       // 料理探す
        Cooking = 1,    // 料理調理
    }

    public enum StaffType
    {
        Hall,
        Chef,
    }
}


public partial class StaffData : MonoBehaviour
{
    // 制作者　田内


    //================
    // 料理を持つ位置

    [Header("料理を保持する位置")]
    [SerializeField]
    private GameObject m_havePoint = null;

    public GameObject HavePoint { get { return m_havePoint; } }

    //============
    // 初期座標

    private Vector3 m_defaultPos = new();
    public Vector3 DefaultPos { get { return m_defaultPos; } }

    //=================
    // ターゲットの料理

    private OrderFoodData m_targetOrderFoodData = null;

    public OrderFoodData TargetOrderFoodData
    {
        get { return m_targetOrderFoodData; }
        set { m_targetOrderFoodData = value; }
    }

    //=========================================
    // ターゲットにしてきたオブジェクトのリスト

    private List<GameObject> m_beenTargetedObjectList = new();

    public List<GameObject> BeenTargetObjectList
    {
        get
        {
            m_beenTargetedObjectList.RemoveAll(_ => _ == null);
            return m_beenTargetedObjectList;
        }
        set { m_beenTargetedObjectList = value; }
    }


    //==============================================================
    //                  実行処理
    //==============================================================

    private void Start()
    {
        // マネージャーに登録
        StaffManager.instance.AddStaffData(this);

        SetInitializeData();
    }


    protected void SetInitializeData()
    {
        // 待つ座標をセット
        m_defaultPos = transform.position;

        // ステータスを更新
        SetStatus();

    }



    //====================================================================
    // 座標取得

    #region メソッド説明
    /// <summary>
    /// 料理を持つポイントの座標を取得
    /// </summary>
    #endregion
    public Vector3 GetHavePos()
    {
        Vector3 pos = new();

        if (m_havePoint == null)
        {
            Debug.LogError("料理をもつポイントが存在しません");
            return pos;
        }

        pos = m_havePoint.transform.position;
        return pos;
    }

}
