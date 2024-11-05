using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BPSkill_1Presenter : MonoBehaviour
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
        status.m_bpSkill_1.Subscribe(x => BarUpdate(status));
    }

    private void BarUpdate(PlayerStatus status)
    {
        m_barController.SetValue(status.m_bpSkill_1.Value);

        if (m_colorChanger == null) return;
        // スキルが使えるかどうかで
        // ・色を変える
        // ・エフェクトの表示非表示を変える
        var parameter = m_characterCore.PlayerParameters;
        if (parameter)
        {
            if (parameter.UseSkill1Flg)
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

    }

    [ContextMenu("BarSetting")]
    private void BarSetting(PlayerStatus status)
    {
        // BPの初期値を設定
        m_barController.SetValue(
            status.m_bpSkill_1.Value,
            status.MaxBPSkill_1);

        m_colorChanger.ChangeUseColor();
    }
}
