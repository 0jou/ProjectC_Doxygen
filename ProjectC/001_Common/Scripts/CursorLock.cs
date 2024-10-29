using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorLock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Mathf.Abs(Screen.width / 2 - Mouse.current.position.ReadValue().x) > 50
            || Mathf.Abs(Screen.height / 2 - Mouse.current.position.ReadValue().y) > 50)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {

            Cursor.lockState = CursorLockMode.None;
        }

    }
}
