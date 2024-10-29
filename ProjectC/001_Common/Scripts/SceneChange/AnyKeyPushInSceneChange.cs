using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnyKeyPushInSceneChange : MonoBehaviour
{

    // シーン遷移用のスクリプトコンポーネント
    [SerializeField] SceneTransitionManager m_sceneChange = null;

    // キーが押されたかどうか 連打防止用
    private bool m_isPressed = false;

    // 何らかのキーが押されたかどうかを判定するためのアクションコンポーネント
    [SerializeField] private InputAction m_pressAnyKeyAction = null;

    private void OnEnable() => m_pressAnyKeyAction.Enable();
    private void OnDisable() => m_pressAnyKeyAction.Disable();

    private void Start()
    {
        m_isPressed = false;

        if (m_sceneChange == null) { m_sceneChange = GetComponent<SceneTransitionManager>(); }

        if (m_pressAnyKeyAction == null)
        {
            m_pressAnyKeyAction = new InputAction(type: InputActionType.PassThrough, binding: "*/<Button>", interactions: "Press");
        }
    }
    void Update()
    {
        if (m_sceneChange == null) return;

        // 何らかのキーが押されたら次のシーンへ 現在なんでも受け付けてしまうのでesc->メニューなどへの対応がいるかも
        if (m_pressAnyKeyAction.triggered && !m_isPressed)
        {
            m_isPressed = true;
            _ = m_sceneChange.SceneChange();
        }
    }
}
