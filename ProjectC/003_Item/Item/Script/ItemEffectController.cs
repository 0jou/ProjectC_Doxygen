using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class ItemEffectController : MonoBehaviour
{
    //素材に近づいた際にエフェクトを表示させる（山本）

    [Header("キラキラエフェクト")]
    [SerializeField] private GameObject m_sparklingEffect = null;
    [Header("オーラエフェクト")]
    [SerializeField] private GameObject m_auraEffect = null;
    [Header("光柱エフェクト")]
    [SerializeField] private GameObject m_lightPillarEffect = null;

    [Header("キラキラエフェクト表示するか")]
    [SerializeField] private bool m_bSparklingEffect = false;
    [Header("オーラエフェクト表示するか")]
    [SerializeField] private bool m_bAuraEffect = false;
    [Header("光柱エフェクト表示するか")]
    [SerializeField] private bool m_bLightPillarEffect = false;

    [Header("エフェクトが表示される範囲")]
    [SerializeField] private float m_sensingRange = 10.0f;

    void Start()
    {
        if (!m_bSparklingEffect && m_sparklingEffect)
        {
            m_sparklingEffect.SetActive(false);
            m_sparklingEffect = null;
        }

        if (!m_bAuraEffect && m_auraEffect)
        {
            m_auraEffect.SetActive(false);
            m_auraEffect = null;
        }

        if (!m_bLightPillarEffect && m_lightPillarEffect)
        {
            m_lightPillarEffect.SetActive(false);
            m_lightPillarEffect = null;
        }

    }

    void Update()
    {
        //プレイヤーが指定範囲内にいないか確認する
        foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (chara.GroupNo != CharacterGroupNumber.player) continue;

            float dist = Vector3.Distance(chara.transform.position, this.transform.position);

            if (dist <= m_sensingRange)
            {
                OnItemEffect();
            }
            else
            {
                OffItemEffect();
            }

        }

    }

    public void OnItemEffect()
    {

        if (m_sparklingEffect && m_bSparklingEffect)
        {
            m_sparklingEffect.SetActive(true);

        }

        if (m_auraEffect && m_bAuraEffect)
        {
           m_auraEffect.SetActive(true);
        }

        if (m_lightPillarEffect && m_bLightPillarEffect)
        {
           m_lightPillarEffect.SetActive(true);
        }

    }

    public void OffItemEffect()
    {
        if (m_sparklingEffect && m_bSparklingEffect)
        {
            m_sparklingEffect.SetActive(false);

        }

        if (m_auraEffect && m_bAuraEffect)
        {
            m_auraEffect.SetActive(false);
        }

        if (m_lightPillarEffect && m_bLightPillarEffect)
        {
            m_lightPillarEffect.SetActive(false);
        }

    }

}
