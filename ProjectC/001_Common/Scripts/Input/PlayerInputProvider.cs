using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// PlayerInputProvider (濱口)

// PlayerInputを使用した操作キャラクターへのInput処理
public class PlayerInputProvider : IInputProvider
{
    Camera m_camera;

    public Vector3 LookVector { get; } = Vector3.zero;

    //　カメラの向きに合った方向を返す(倉田)
    public Vector3 MoveVector
    {
        get
        {
            if (m_camera == null) m_camera = Camera.main;

            // カメラの向き
            Vector3 vForward = m_camera.transform.forward;
            vForward.y = 0;
            // 入力ベクトル
            var axis = PlayerInputManager.instance.GetInputAction(InputActionMapTypes.Player, "Move").ReadValue<Vector2>();

            //Normarizeしないように修正。（山本）
            return Quaternion.LookRotation(vForward) *　new Vector3(axis.x, 0, axis.y);
        }
    }

    // int型で、攻撃の種類を決定する
    public int AttackType
    {
        get
        {
            //IsPressed->triggeredに変更(山本)
            if (PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "Attack"))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        set { }
    }

    public bool DoDush { get { return PlayerInputManager.instance.IsInputActionPressed(InputActionMapTypes.Player, "Dush"); } }
    public bool OnPressedDush { get { return PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.Player, "Dush"); } }
    public bool OnReleasedDush { get { return PlayerInputManager.instance.IsInputActionWasReleased(InputActionMapTypes.Player, "Dush"); } }


    //ローリングトリガー（山本）
    //WasPressedThisFrameに変更（山本）
    public bool DoRolling { get { return PlayerInputManager.instance.IsInputActionWasReleased(InputActionMapTypes.Player, "Rolling"); } }

    public bool SelectLeftItem { get { return PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "SelectLeftItem"); } }
    public bool SelectRightItem { get { return PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "SelectRightItem"); } }
    public bool UseItem { get { return PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "UseItem"); } }

    //童話スキル1使用トリガー
    //IsPressed->triggeredに変更(山本)
    public bool UseStorySkill_1 { get { return PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "UseStorySkill_1"); } }
    //童話スキル2使用トリガー
    public bool UseStorySkill_2 { get { return PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "UseStorySkill_2"); } }


    // アイテム採取
    public bool Gathering { get { return PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "Gathering"); } }

    // アイテム設置
    public bool PutItem { get { return PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "PutItem"); } }

    // アイテム投げる準備
    public bool ReadyThrowItem { get { return PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "ReadyThrowItem"); } }

    // アイテム投げる
    public bool ThrowItem { get { return PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "ThrowItem"); } }

    // アイテム投げる準備時、投げる方向（マウス座標）
    public Vector2 ThrowAim
    {
        get
        {
            var mouseAction = PlayerInputManager.instance.GetInputAction(InputActionMapTypes.Player, "ThrowAim");
            return mouseAction.ReadValue<Vector2>();
        }
    }

    //キャンセル
    public bool Cancel { get { return PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.Player, "Cancel"); } }

    //食べる
    public bool Eat { get { return PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "Eat"); } }

    // 敵側
    public bool IsArrive { get; } = false;

    public Vector3 Destination { get; } = Vector3.zero;

}
