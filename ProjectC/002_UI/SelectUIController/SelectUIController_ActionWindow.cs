using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class SelectUIController : MonoBehaviour
{

    //アクションアイテムウィンドウ用のパーティアルクラス（山本）

    ////========================================================
    //// ボタンを押したか

    //private bool m_isPut = false;
    //public bool IsPut { get { return m_isPut; } }

    //private bool m_isUse = false;
    //public bool IsUse { get { return m_isUse; } }

    //private bool m_isThrow = false;
    //public bool IsThrow { get { return m_isThrow; } }

    ////==========================================================
    ////==========================================================


   //ActionWindow用のUpdate
    public void OnUpdateActionWindow()
    {
        SelectActionWindowUI();
        OnUse();
        OnPut();
    }

    //ActionWindow用のLateUpdate
    public void OnLateUpdateActionWindow()
    {
        OnLateUpdate();
        //m_isPut = false;
        //m_isThrow = false;
        //m_isUse = false;
    }

    //ActionWindow用のSelectUI
    private void SelectActionWindowUI()
    {
        if (Keyboard.current?.anyKey.wasPressedThisFrame == false && Gamepad.current?.wasUpdatedThisFrame == false) return;

        if (m_uiList.Count <= 0)
        {
            return;
        }

        bool flg = false;

        // 左に進む
        if (PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.UI, "SelectLeftActionItem"))
        {
            flg = Left();
        }


        // 右に進む
        if (PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.UI, "SelectRightActionItem"))
        {
            flg = Right();
        }

        if (flg)
        {
           m_currentSelectUIData =m_uiList[m_currentHeight].List[m_currentWidth];

            m_isSelectChangeFlg = true;

            //SE音追加（山本）
            SoundManager.Instance.StartPlayback("Select");
        }

    }


    private void OnPut()
    {
        if (PlayerInputManager.instance.GetInputAction(InputActionMapTypes.UI,"PutItem") == null)
        {
            Debug.LogError("InputActionが存在しません");
            return;
        }


        if (PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.UI, "PutItem") == true)
        {
            //m_isPut = true;
        }
    }

    private void OnUse()
    {
        if (PlayerInputManager.instance.GetInputAction(InputActionMapTypes.UI, "UseItem") == null)
        {
            Debug.LogError("InputActionが存在しません");
            return;
        }


        if (PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.UI, "UseItem")== true)
        {

            //m_isUse = true;
        }
    }


    //一個ずつ追加した際に横方向へと追加できるように改良した関数（山本）
    //public void AddHolizenUI(GameObject _ui)
    //{

    //    // 行数を超えたら
    //    if (m_constraintCount <= m_currentConstraintCount)
    //    {
    //        m_currentConstraintCount = 0;
    //        m_listCount++;
    //    }

    //    // リストを作成
    //    if (m_uiList.Count <= m_listCount)
    //    {
    //        m_uiList.Add(new UIGameObject());
    //    }

    //    // リストに追加
    //    m_uiList[m_listCount].m_uiGameObjactList.Add(_ui);

    //    // 行数を加算
    //    m_currentConstraintCount++;


    //    // コンポーネントを追加
    //    AddMouseSelectUIComponent(_ui);

    //    SetHeadUIGameObject();

    //}


}
