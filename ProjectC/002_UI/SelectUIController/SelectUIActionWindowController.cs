using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using SelectUIInfo;

public class SelectUIActionWindowController : SelectUIController
{


    //========================================================
    //ActionItemUI用派生クラス（山本）
    //========================================================

    // ボタンを押したか

    private bool m_isPut = false;
    public bool IsPut { get { return m_isPut; } }

    private bool m_isUse = false;
    public bool IsUse { get { return m_isUse; } }

    private bool m_isThrow = false;
    public bool IsThrow { get { return m_isThrow; } }

    //ウィンドウのサイズ等

    [Header("選択時のウィンドウのサイズ（Max）")]
    [SerializeField]
    private Vector3 m_selectScale = Vector3.one;
    [Header("選択外のウィンドウのサイズ（Min）")]
    [SerializeField]
    private Vector3 m_notSelectScale = Vector3.one * 0.7f;
    [Header("選択→選択外へサイズが変化するまでの期間")]
    [SerializeField]
    private float m_duration = 0.5f;

    //==========================================================
    //==========================================================

    public override void OnUpdate()
    {
        if (m_alwaysCreateType == AlwaysCreateType.Enable)
        {
            NullCheck();
        }

        SelectWindowUI();
        SetSelectUIScale();
        OnUse();
        OnPut();
        OnThrow();

    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        m_isPut = false;
        m_isThrow = false;
        m_isUse = false;

    }

    public void CheckWithChange()
    {
        if (m_currentSelectUIData == null)
        {
            m_currentWidth--;

            if (m_currentWidth <= 0)
            {
                m_currentWidth = 0;
            }
        }

    }

    private void SelectWindowUI()
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
            m_currentSelectUIData = m_uiList[0].List[m_currentWidth];

            m_isSelectChangeFlg = true;

            //SE音追加（山本）
            SoundManager.Instance.StartPlayback("Select");
        }

    }


    private void OnPut()
    {
        if (PlayerInputManager.instance.GetInputAction(InputActionMapTypes.UI, "PutItem") == null)
        {
            Debug.LogError("InputActionが存在しません");
            return;
        }


        if (PlayerInputManager.instance.GetInputAction(InputActionMapTypes.UI, "PutItem").triggered == true)
        {
            m_isPut = true;
        }
    }

    private void OnUse()
    {
        if (PlayerInputManager.instance.GetInputAction(InputActionMapTypes.UI, "UseItem") == null)
        {
            Debug.LogError("InputActionが存在しません");
            return;
        }


        if (PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.UI, "UseItem") == true)
        {

            m_isUse = true;
        }
        else
        {
            m_isUse = false;
        }

    }

    private void OnThrow()
    {
        if (PlayerInputManager.instance.GetInputAction(InputActionMapTypes.UI, "ThrowItem") == null)
        {
            Debug.LogError("InputActionが存在しません");
            return;
        }


        if (PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.UI, "ThrowItem") == true)
        {

            m_isThrow = true;
        }
    }

    //選択したUIのScale値を変更する(選択したものだけScale値を変更する)
    private void SetSelectUIScale()
    {
        //UIリストなければ早期リターン
        if (m_uiList.Count <= 0.0f)
        {
            return;
        }

        foreach (var uiObj in m_uiList[0].List)
        {
            if (uiObj?.UI == null) continue;

            //選択したUIObjをScale値1.0に
            if (uiObj == m_currentSelectUIData)
            {
                uiObj.UI.transform.DOScale(m_selectScale, m_duration);
            }
            else//選択した物以外を少し小さめに
            {
                uiObj.UI.transform.DOScale(m_notSelectScale, m_duration);
            }

        }
    }


    //一個ずつ追加した際に横方向へと追加できるように改良した関数（山本）
    public override void AddUI(GameObject _ui, SelectUIType _type, int _lineBreak)
    {
        base.AddUI(_ui, _type, _lineBreak);

        SetUIActionWindowGameObject();

    }

}
