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

        PlayerStatus status = m_characterCore.PlayerParameters.PlayerStatus;
        // 初期設定
        BarSetting(status);
        
        // 変わったら実行する処理を登録
        status.m_stamina.Subscribe(x =>
        {
            BarUpdate(status);

            if(status.m_stamina.Value < status.MaxStamina)
            {
                m_staminaController.OnShow();
            }

            if(status.m_stamina.Value >= status.MaxStamina)
            {
                m_staminaController.OnHide();
            }
        });
    }

    private void BarUpdate(PlayerStatus status)
    {
        //if (m_staminaController == null |
        //    m_characterCore == null) return;

        m_staminaController.SetValue(status.m_stamina.Value);
    }

    [ContextMenu("BarSetting")]
    private void BarSetting(PlayerStatus status)
    {
        //if (m_staminaController == null |
        //    m_characterCore == null) return;
        // HPの初期値を設定
        m_staminaController.SetValue(
            status.m_stamina.Value,
            status.MaxStamina);
    }
}
