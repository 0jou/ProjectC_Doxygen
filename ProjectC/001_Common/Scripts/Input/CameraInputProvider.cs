using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraInputProvider : ICameraInputProvider
{
    public Vector2 CameraXY
    {
        get
        {
            return PlayerInputManager.instance.GetInputAction(InputActionMapTypes.Camera, "CameraXY").ReadValue<Vector2>();
        }
    }
}
