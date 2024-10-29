using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using FoodInfo;
using NaughtyAttributes;
using PocketItemDataInfo;
using ItemInfo;

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

    [BoxGroup("Button")]
    [Header("作成スライダー")]
    [SerializeField]
    private Slider m_createNumSlider = null;



    [BoxGroup("Window")]
    [Header("作成料理確認ウィンドウコントローラー")]
    [SerializeField]
    private WindowController m_confirmationItemWindowController = null;


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

        SetFoodData();
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

        // 現在選択中の値を更新
        if (m_maxCreateNum < m_currentCreateNum) m_currentCreateNum = m_maxCreateNum;
        if (m_currentCreateNum < m_minCreateNum) m_currentCreateNum = m_minCreateNum;

        // スライダーの値更新
        SetSliderValue();

        m_isSelectChangeFlg = true;
    }


    /// <summary>
    /// 実行処理
    /// </summary>
    public async UniTask OnUpdate()
    {
        Select();

        await CreateFood();
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
        else
        {
            return false;
        }
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
        else
        {
            return false;
        }
    }


    /// <summary>
    /// 料理が作成可能か
    /// </summary>
    public bool IsCreate()
    {
        int freespace = m_pocketType.GetPocketItemDataManager().GetFreeSpaceNum();

        if (m_currentCreateNum <= freespace)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void Select()
    {
        // スライダー
        Slider();

        // 足す
        Increment();

        // 減らす
        Decrement();
    }


    private void Increment()
    {
        #region nullチェック
        if (m_incrementInputActionButton == null)
        {
            Debug.LogError("IncrementInputActionButtonがシリアライズされていません");
            return;
        }
        #endregion

        if (m_incrementInputActionButton.IsInputActionTrriger())
        {
            if (IsIncrement())
            {
                m_currentCreateNum++;
                m_isSelectChangeFlg = true;
                SetSliderValue();
            }
        }
    }



    private void Decrement()
    {
        #region nullチェック
        if (m_decrementInputActionButton == null)
        {
            Debug.LogError("DecrementInputActionButtonがシリアライズされていません");
            return;
        }
        #endregion

        if (m_decrementInputActionButton.IsInputActionTrriger())
        {
            if (IsDecrement())
            {
                m_currentCreateNum--;
                m_isSelectChangeFlg = true;
                SetSliderValue();
            }
        }
    }


    private void Slider()
    {
        if (m_createNumSlider == null) return;

        if (m_currentCreateNum != (int)m_createNumSlider.value)
        {
            m_currentCreateNum = (int)m_createNumSlider.value;
            m_isSelectChangeFlg = true;
        }
    }


    // スライダーの値を更新
    private void SetSliderValue()
    {
        if (m_createNumSlider == null) return;

        m_createNumSlider.minValue = m_minCreateNum;
        m_createNumSlider.maxValue = m_maxCreateNum;
        m_createNumSlider.value = m_currentCreateNum;
    }


    private async UniTask CreateFood()
    {
        #region nullチェック
        if (m_createInputActionButton == null)
        {
            Debug.LogError("CreateInputActionButtonがシリアライズされていません");
            return;
        }
        #endregion

        if (m_createInputActionButton.IsInputActionTrriger())
        {
            for (int i = 0; i < m_currentCreateNum; ++i)
            {
                // 料理を作成(素材は使用する)
                FoodData.CreateFood(m_pocketType, m_foodID, true);
            }

            // 作成料理確認ウィンドウを作成
            await CreateConfirmationItemWindow();

            // 初期化
            SetFoodData();
        }
    }


    // 作成料理確認ウィンドウを作成
    private async UniTask CreateConfirmationItemWindow()
    {
        #region nullチェック
        if (m_confirmationItemWindowController == null)
        {
            Debug.LogError("ConfirmationItemWindowControllerがシリアライズされていません");
            return;
        }
        #endregion

        // 作成料理確認ウィンドウを作成
        try
        {
            var controller = Instantiate(m_confirmationItemWindowController);

            await controller.CreateWindow<ConfirmationItemWindow>(false, async _ =>
            {
                _.SetDescription(m_pocketType, ItemTypeID.Food, (uint)m_foodID);
                await UniTask.CompletedTask;
            });

            if (controller != null) Destroy(controller.gameObject);
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

}
