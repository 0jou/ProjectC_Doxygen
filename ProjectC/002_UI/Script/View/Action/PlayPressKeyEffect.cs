using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayPressKeyEffect : MonoBehaviour
{

    [SerializeField]
    [ReadOnly]
    private InputActionReference m_inputActionReference = null;

    [SerializeField]
    [ReadOnly]
    private PressKeyEffectController m_pressKeyEffectController = null;


    private void Update()
    {
        // 押したら、エフェクト再生
        if (m_inputActionReference.action.triggered)
        {
            m_pressKeyEffectController.Play();
        }
    }

    [Button("変数セット")]
    private void SerializeFieldSetting()
    {
        m_inputActionReference = GetComponentInChildren<InputActionButton>().InputActionReference;
        if (m_inputActionReference == null)
        {
            Debug.LogError("m_inputActionが設定されていません");
            return;
        }

        m_pressKeyEffectController = GetComponentInChildren<PressKeyEffectController>();
        if (m_pressKeyEffectController == null)
        {
            Debug.LogError("m_pressKeyEffectControllerが設定されていません");
            return;
        }
    }

}
