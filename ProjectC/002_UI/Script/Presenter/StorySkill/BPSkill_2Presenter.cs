using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BPSkill_2Presenter : MonoBehaviour
{
    [SerializeField] private CharacterCore m_characterCore;
    [SerializeField] private StorySkillUIController m_barController;
    [SerializeField] private StorySkillUIChangeColor m_colorChanger;
    [SerializeField] private GameObject m_pressKeyEffectObj;

    public void Start()
    {
        if (m_barController == null)
        {
            m_barController = GetComponent<StorySkillUIController>();
        }
        if (m_colorChanger == null)
        {
            m_colorChanger = GetComponent<StorySkillUIChangeColor>();
        }

        PlayerStatus status = m_characterCore.PlayerParameters.PlayerStatus;
        // HPの初期設定
        BarSetting(status);

        // 変わったら実行する処理を登録
        status.m_bpSkill_2.Subscribe(x => BarUpdate(m_characterCore.PlayerParameters));
    }

    private void BarUpdate(PlayerParameters parameter)
    {
        if (!parameter) return;
        m_barController.SetValue(parameter.PlayerStatus.m_bpSkill_2.Value);

        if (m_colorChanger == null) return;
        // スキルが使えるかどうかで
        // ・色を変える
        // ・エフェクトの表示非表示を変える

        if (parameter.UseSkill2Flg)
        {
            m_colorChanger.ChangeUseColor();
            m_pressKeyEffectObj.SetActive(true);
        }
        else
        {
            m_colorChanger.ChangeNoUseColor();
            m_pressKeyEffectObj.SetActive(false);
        }
    }

    [ContextMenu("BarSetting")]
    private void BarSetting(PlayerStatus status)
    {
        // BPの初期値を設定
        m_barController.SetValue(
            status.m_bpSkill_2.Value,
            status.MaxBPSkill_2);

        m_colorChanger.ChangeUseColor();
    }
}
