using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ButtonInfo;

namespace ButtonInfo
{
    public enum ButtonID
    {
        None = -1,
        Start = 0,
        Cancel = 1,

        ProvideFood = 1000,



        ManagementStorage = 2000,
        ManagementStart = 2001,
    }


    // 使用可能かどうか
    public enum IsUseButtonID
    {
        Disable,
        Enable,
    }
}



public class ButtonData : MonoBehaviour
{
    // ボタンデータをセットする
    // 制作者　田内

    //=========================

    private SelectUIController m_selectUIController = null;

    //===========================================

    [Header("ボタンID")]
    [SerializeField]
    protected ButtonID m_buttonID = ButtonID.None;

    public ButtonID ButtonID { get { return m_buttonID; } }

    //===========================================

    [Header("使用可能")]
    [SerializeField]
    protected bool m_isUse = true;

    public bool IsUse
    {
        get { return m_isUse; }
        set { m_isUse = value; }
    }

    //=================================================
    //                  実行処理
    //=================================================


    // 押されたときに行う処理
    virtual protected void OnPressUpdate()
    {

    }
  

}
