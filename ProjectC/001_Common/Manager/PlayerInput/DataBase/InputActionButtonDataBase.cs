using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UniRx;
using System.Linq;

[CreateAssetMenu(fileName = "InputActionButtonDataBase", menuName = "ScriptableObjects/nputActionButton/作成 InputActionButtonDataBase")]
public class InputActionButtonDataBase : ScriptableObject
{
    // 制作者 田内
    // デバイスによるボタン画像のデータベース

    [Header("KeyboardMouse")]
    [SerializeField]
    private List<InputActionButtonData> m_keyboardMouseList = new();


    [Header("XBOX")]
    [SerializeField]
    private List<InputActionButtonData> m_xboxList = new();


    [Header("PlayStation")]
    [SerializeField]
    private List<InputActionButtonData> m_playStationList = new();

    [Header("Switch")]
    [SerializeField]
    private List<InputActionButtonData> m_switchList = new();

    //=======================================================
    //                      実行処理
    //=======================================================

    /// <summary>
    /// 画像を取得する
    /// </summary>
    public Sprite GetSprite(InputActionReference _inputActionReference, DeviceTypes _deviceType)
    {
        // アクションを取得
        if (_inputActionReference == null) return null;
        var action = _inputActionReference.action;


        for (int bindIndex = 0; bindIndex < action.bindings.Count; bindIndex++)
        {
            // パス取得
            var path = action.bindings[bindIndex].effectivePath;
            if (string.IsNullOrEmpty(path)) continue;

            // 合致するコントローラーを取得
            var matchedControls = action.controls.Where(control => InputControlPath.Matches(path, control));
            foreach (var control in matchedControls)
            {
                if (control is InputDevice) continue;

                string deviceIconGroup = GetDeviceIconGroup(control.device);

                switch (_deviceType)
                {
                    case DeviceTypes.KeyboardMouse:
                        {
                            if (deviceIconGroup != "Keyboard" && deviceIconGroup != "Mouse") break;
                            var controlPathContent = control.path.Substring(control.device.name.Length + 2);

                            foreach (var data in m_keyboardMouseList)
                            {
                                if (data.ButtonName == controlPathContent)
                                {
                                    return data.ButtonSprite;
                                }
                            }

                            break;
                        }
                    case DeviceTypes.XBOX:
                        {
                            if (deviceIconGroup != "XInputController") break;
                            var controlPathContent = control.path.Substring(control.device.name.Length + 2);

                            foreach (var data in m_xboxList)
                            {
                                if (data.ButtonName == controlPathContent)
                                {
                                    return data.ButtonSprite;
                                }
                            }
                            break;
                        }
                    case DeviceTypes.PlayStation:
                        {
                            if (deviceIconGroup != "DualShockGamePad") break;
                            var controlPathContent = control.path.Substring(control.device.name.Length + 2);

                            foreach (var data in m_playStationList)
                            {
                                if (data.ButtonName == controlPathContent)
                                {
                                    return data.ButtonSprite;
                                }
                            }

                            break;
                        }
                    case DeviceTypes.Switch:
                        {
                            if (deviceIconGroup != "SwitchProController") break;
                            var controlPathContent = control.path.Substring(control.device.name.Length + 2);

                            foreach (var data in m_switchList)
                            {
                                if (data.ButtonName == controlPathContent)
                                {
                                    return data.ButtonSprite;
                                }
                            }
                            break;
                        }
                }
            }
        }

        return null;
    }


    private static string GetDeviceIconGroup(InputDevice _inputDevice)
    {
        return _inputDevice switch
        {
            Keyboard => "Keyboard",
            Mouse => "Mouse",
            UnityEngine.InputSystem.XInput.XInputController => "XInputController",
            UnityEngine.InputSystem.DualShock.DualShockGamepad => "DualShockGamepad",
            UnityEngine.InputSystem.Switch.SwitchProControllerHID => "SwitchProController",
            _ => null
        };
    }

}
