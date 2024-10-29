using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputActionMapTypes
{
    Player,
    Camera,
    UI,
}

[RequireComponent(typeof(PlayerInput))]
public partial class PlayerInputManager : BaseManager<PlayerInputManager>
{

    // 制作者 田内(他)
    // PlayerInputを管理するマネージャー

    

    // 入力感知
    PlayerInput m_playerInput = null;


    //=============
    // ActionMap

    // プレイヤー
    private InputActionMap m_playerInputActionMap = null;

    // カメラ
    private InputActionMap m_cameraInputActionMap = null;

    //UI
    private InputActionMap m_uiInputActionMap = null;


    //==============================================
    //              実行処理
    //==============================================


    private void Awake()
    {
        base.Awake();

        // PlayerInputを取得
        gameObject.TryGetComponent(out m_playerInput);


        m_playerInputActionMap = m_playerInput.actions.FindActionMap("Player");
        m_cameraInputActionMap = m_playerInput.actions.FindActionMap("Camera");
        m_uiInputActionMap = m_playerInput.actions.FindActionMap("UI");
    }


    private void Update()
    {
        // 入力デバイスの監視
        ObservationDevice();
    }

    /// <summary>
    /// 引数InputActionMapを取得
    /// </summary>
    public InputActionMap GetInputActionMap(InputActionMapTypes _type)
    {

        InputActionMap inputActionMap = null;

        switch (_type)
        {
            case InputActionMapTypes.Player:
                {
                    inputActionMap = m_playerInputActionMap;
                    break;
                }
            case InputActionMapTypes.Camera:
                {
                    inputActionMap = m_cameraInputActionMap;
                    break;
                }
            case InputActionMapTypes.UI:
                {
                    inputActionMap = m_uiInputActionMap;
                    break;
                }
        }

        if (inputActionMap == null)
        {
            Debug.LogError(_type.ToString() + "にはInputActionMapが存在しません");
        }

        inputActionMap.Enable();
        return inputActionMap;
    }


    public InputAction GetInputAction(InputActionMapTypes _type, string _action)
    {
        InputActionMap inputActionMap = GetInputActionMap(_type);
        if (inputActionMap == null) return null;

        InputAction inputAction = inputActionMap.FindAction(_action);

        if (inputAction == null)
        {
            Debug.LogError(_type.ToString() + "Mapには" + _action + "というInputActionは存在しません");
        }

        inputAction.Enable();
        return inputAction;
    }


    /// <summary>
    /// 引数InputActionのトリガー 
    /// 押された1フレームのみtrueを返す
    /// </summary>
    public bool IsInputActionTrigger(InputActionMapTypes _type, string _action)
    {
        var action = GetInputAction(_type, _action);

        if (action == null)
        {
            Debug.LogError("このアクションは存在しません");
            return false;
        }

        action.Enable();
        return action.triggered;
    }

    /// <summary>
    /// 引数InputActionが押されたか
    /// 押された1フレームのみtrueを返す
    /// </summary>
    public bool IsInputActionWasPressed(InputActionMapTypes _type, string _action)
    {

        var action = GetInputAction(_type, _action);

        if (action == null)
        {
            Debug.LogError("このアクションは存在しません");
            return false;
        }

        action.Enable();
        return action.WasPressedThisFrame();
    }


    /// <summary>
    /// 引数InputActionが押され続けているか
    /// </summary>
    public bool IsInputActionPressed(InputActionMapTypes _type, string _action)
    {
        var action = GetInputAction(_type, _action);

        if (action == null)
        {
            Debug.LogError("このアクションは存在しません");
            return false;
        }

        action.Enable();
        return action.IsPressed();
    }



    /// <summary>
    /// 引数InputActionが離されたか
    /// 離された1フレームのみtrueを返す
    /// </summary>
    public bool IsInputActionWasReleased(InputActionMapTypes _type, string _action)
    {

        var action = GetInputAction(_type, _action);

        if (action == null)
        {
            Debug.LogError("このアクションは存在しません");
            return false;
        }

        action.Enable();
        return action.WasReleasedThisFrame();
    }

}
