/*!
 * @file BaseManagementEvent.cs
 * @brief 経営イベントの基底クラス
 * @author 田内
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @brief 経営イベントの基底クラス
/// マネージャーから管理される想定 (ManagementEventManager.cs)
/// </summary>
public class BaseManagementEvent : MonoBehaviour
{


    [Header("実行される確率")]
    [Range(0, 100)]
    [SerializeField]
    //! 実行される確率 インスペクター側で操作される想定
    private int m_probability = 50;

    //! 確率の参照用
    public int Probability
    {
        get { return m_probability; }
        set { m_probability = value; }
    }


    //! 動作が終了したかどうか
    private bool m_isEventEnd = false;

    //! 動作終了かどうかの参照用
    public bool IsEventEnd
    {
        get { return m_isEventEnd; }
    }

    //! イベントの開始 継承先でオーバーライドする
    virtual public void OnStart()
    {
        // 開始処理はここで行う
    }


    virtual public void OnUpdate()
    {
        m_isEventEnd = true;
        Debug.LogError(this.GetType().Name + " クラスで OnUpdate がオーバーライドされていません");
    }


    virtual public void OnExit()
    {
        // 終了処理があればここで行う
    }

    protected void SetEventEnd()
    {
        m_isEventEnd = true;
    }

}
