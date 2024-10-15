using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class StaminaPresenter : MonoBehaviour
{
    [SerializeField] private CharacterCore m_characterCore;
    [SerializeField] private StaminaController m_staminaController;

    public void Start()
    {
        if (m_staminaController == null)
        {
            m_staminaController = GetComponent<StaminaController>();
            if (m_characterCore == null)
            {
                Debug.LogError("StaminaControllerが見つかりませんでした");
            }
            else
            {
                // インスペクターでシリアライズ設定してください。
                Debug.LogWarning("StaminaControllerが設定されていません。");
            }
        }
        if(m_characterCore == null)
        {
            m_characterCore =
                GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterCore>();
            if(m_characterCore == null)
            {
                Debug.LogError("CharacterCoreが見つかりませんでした");
            }
            else
            {                 
                // インスペクターでシリアライズ設定してください。
                Debug.LogWarning("CharacterCoreが設定されていません。");
            }
        }

        // 初期設定
        BarSetting();

        // 変わったら実行する処理を登録
        m_characterCore.Status.m_stamina.Subscribe(x =>
        {
            BarUpdate();

            if(m_characterCore.Status.m_stamina.Value < m_characterCore.Status.MaxStamina)
            {
                m_staminaController.OnShow();
            }

            if(m_characterCore.Status.m_stamina.Value >= m_characterCore.Status.MaxStamina)
            {
                m_staminaController.OnHide();
            }
        });
    }

    private void BarUpdate()
    {
        if (m_staminaController == null |
            m_characterCore == null) return;

        m_staminaController.SetValue(m_characterCore.Status.m_stamina.Value);
    }

    [ContextMenu("BarSetting")]
    private void BarSetting()
    {
        if (m_staminaController == null |
            m_characterCore == null) return;
        // HPの初期値を設定
        m_staminaController.SetValue(
            m_characterCore.Status.m_stamina.Value,
            m_characterCore.Status.MaxStamina);
    }
}
