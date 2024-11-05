using Arbor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneBGM : MonoBehaviour
{
    [SerializeField] private SoundData m_harvestBGM;
    [SerializeField] private SoundData m_battleBGM;

    [SerializeField] private Transform m_player;

    [Header("ボスのキャンバス")]
    [SerializeField] private GameObject m_bossCanvas = null;

    private bool m_isBossBattle = false;

    void Start()
    {
        SoundManager.Instance.StartBGM(m_harvestBGM);
    }

    private void Update()
    {
        bool findPlayer = false;
        CharacterMeta meta = IMetaAI<CharacterCore>.Instance as CharacterMeta;
        if (meta == null)
        {
            Debug.LogError("CharacterMetaが見つかりません");
            return;
        }
        foreach (var boss in meta.BossList)
        {
            if (!boss.TryGetComponent(out EnemyParameters parameters)) return;
            if (parameters.Target == null) continue;

            if (parameters.Target.GetInstanceID() == m_player.GetInstanceID())
            {
                findPlayer = true;
                break;
            }
        }

        if (m_isBossBattle != findPlayer)
        {
            m_isBossBattle = findPlayer;
            if (m_isBossBattle)
            {
                SoundManager.Instance.StartBGM(m_battleBGM);
                if (m_bossCanvas)
                    m_bossCanvas.SetActive(true);
            }
            else
            {
                SoundManager.Instance.StartBGM(m_harvestBGM);
                if (m_bossCanvas)
                    m_bossCanvas.SetActive(false);
            }
        }
    }
}
