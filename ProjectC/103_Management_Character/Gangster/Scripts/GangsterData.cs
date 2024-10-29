using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GangsterStateInfo;

namespace GangsterStateInfo
{
    public enum GangsterState
    {
        Default,    // 通常の状態
        Attack,     // 攻撃状態
        Dead,       // HPが0になった時
    }
}


public class GangsterData : MonoBehaviour
{
    // ヤンキーのデータ
    // 制作者　田内

    //==========================================================
    // ヤンキーの状態

    private GangsterState m_currentGangsterState = GangsterState.Default;

    public GangsterState CurrentGangsterState
    {
        set { m_currentGangsterState = value; }
        get { return m_currentGangsterState; }
    }

    //=====================================
    // 出現座標

    private Vector3 m_appearPos = new();

    public Vector3 AppearPos
    {
        get { return m_appearPos; }
    }

    //==========================================================
    // ターゲットの客データ

    private CustomerData m_targetCustomerData = null;

    public CustomerData TargetCustomerData
    {
        get { return m_targetCustomerData; }
        set { m_targetCustomerData = value; }
    }

    //==========================================================
    // ターゲットのスタッフデータ

    private StaffData m_targetStaffData = null;

    public StaffData TargetStaffData
    {
        get { return m_targetStaffData; }
        set { m_targetStaffData = value; }
    }


    private void Start()
    {
        Initialize();
    }

    // 初期値をセット
    protected void Initialize()
    {
        // 出現した位置
        m_appearPos = transform.position;
    }

}
