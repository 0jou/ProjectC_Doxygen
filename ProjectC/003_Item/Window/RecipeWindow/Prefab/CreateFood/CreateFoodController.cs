using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;
using FoodInfo;
using NaughtyAttributes;
using PocketItemDataInfo;

public class CreateFoodController : MonoBehaviour
{
    // 制作者 田内
    // 料理を作成するコントローラー

    [Header("操作する料理ID")]
    [SerializeField]
    private FoodID m_foodID = FoodID.Omelette;

    [Header("ポケット種類")]
    [SerializeField]
    private PocketType m_pocketType = PocketType.Inventory;
    public PocketType PocketType
    {
        get { return m_pocketType; }
    }

    // 作成数
    private int m_currentCreateNum = 0;
    public int CurrentCreateNum
    {
        get { return m_currentCreateNum; }
    }


    // 最小作成数
    private int m_minCreateNum = 1;
    public int MinCreateNum
    {
        get { return m_minCreateNum; }
    }

    // 最大作成数
    private int m_maxCreateNum = 1;
    public int MaxCreateNum
    {
        get { return m_maxCreateNum; }
    }

    //====================
    // 変更されたかどうか

    bool m_isSelectChangeFlg = false;

    public bool IsSelectChangeFlg { get { return m_isSelectChangeFlg; } }


    //==================
    // ボタン

    [BoxGroup("Button")]
    [Header("増やすボタン")]
    [SerializeField]
    private InputActionButton m_incrementInputActionButton = null;

    [BoxGroup("Button")]
    [Header("減らすボタン")]
    [SerializeField]
    private InputActionButton m_decrementInputActionButton = null;

    [BoxGroup("Button")]
    [Header("作成ボタン")]
    [SerializeField]
    private InputActionButton m_createInputActionButton = null;

    //==============================================
    //              実行処理
    //==============================================


    private void Start()
    {
        SetFoodData();
    }


    /// <summary>
    /// 作成する料理のデータをセットする
    /// </summary>
    public void SetFoodData(PocketType _pocketType, FoodID _id)
    {
        m_pocketType = _pocketType;
        m_foodID = _id;

        // 最大作成数を更新
        m_maxCreateNum = FoodData.GetCreateNum(m_pocketType, m_foodID);
    }

    private void SetFoodData()
    {
        // 最小作成数を更新
        if (FoodData.IsCreate(m_pocketType, m_foodID))
        {
            m_minCreateNum = 1;
        }
        else
        {
            m_minCreateNum = 0;
        }

        // 最大作成数を更新
        m_maxCreateNum = FoodData.GetCreateNum(m_pocketType, m_foodID);
    }


    /// <summary>
    /// 実行処理
    /// </summary>
    public void OnUpdate()
    {
        Select();

        CreateFood();
    }


    /// <summary>
    /// 後実行処理
    /// </summary>
    public void OnLateUpdate()
    {
        m_isSelectChangeFlg = false;
    }


    /// <summary>
    /// 料理を作成するボタンが押されたか
    /// </summary>
    public bool IsPressCreateButton()
    {
        if (m_createInputActionButton == null) return false;
        return m_createInputActionButton.IsInputActionTrriger();
    }


    /// <summary>
    /// 足せるか
    /// </summary>
    public bool IsIncrement()
    {
        if (m_currentCreateNum < m_maxCreateNum)
        {
            return true;
        }

        return false;

    }


    /// <summary>
    /// 減らせるか
    /// </summary>
    public bool IsDecrement()
    {
        if (m_minCreateNum < m_currentCreateNum)
        {
            return true;
        }

        return false;
    }


    /// <summary>
    /// 料理が作成可能か
    /// </summary>
    public bool IsCreate()
    {
        return true;
    }


    private void Select()
    {
        // 足す
        Increment();

        // 減らす
        Decrement();
    }


    private void Increment()
    {
        if (m_incrementInputActionButton == null) return;

        if (m_incrementInputActionButton.IsInputActionTrriger())
        {
            if (IsIncrement())
            {
                m_currentCreateNum++;
                m_isSelectChangeFlg = true;
            }
        }
    }



    private void Decrement()
    {
        if (m_decrementInputActionButton == null) return;

        if (m_decrementInputActionButton.IsInputActionTrriger())
        {
            if (IsDecrement())
            {
                m_currentCreateNum--;
                m_isSelectChangeFlg = true;
            }
        }
    }


    private void CreateFood()
    {
        if (m_createInputActionButton == null) return;

        if (m_createInputActionButton.IsInputActionTrriger())
        {

            for (int i = 0; i < m_currentCreateNum; ++i)
            {
                // 料理を作成(素材は使用する)
                FoodData.CreateFood(m_pocketType, m_foodID, true);
            }

            // 初期化
            SetFoodData();
        }
    }
}
