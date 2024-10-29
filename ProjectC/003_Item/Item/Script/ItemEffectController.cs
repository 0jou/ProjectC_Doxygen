using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

[DefaultExecutionOrder(5)]
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

    [Header("Scaleの最大値")]
    [SerializeField] private float m_maxScale = 1.0f;

    [Header("Scaleの最小値")]
    [SerializeField] private float m_minScale = 0.0f;

    [Header("遷移までの時間")]
    [SerializeField] private float m_duration = 0.1f;


    private CharacterCore m_playerCore = null;

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


        //Scale値を調整-----------------------------------------------------
        if (m_sparklingEffect)
        {
            m_sparklingEffect.transform.localScale = Vector3.zero;
        }

        if (m_auraEffect)
        {
            m_auraEffect.transform.localScale = Vector3.zero;
        }

        if (m_sparklingEffect)
        {
            m_sparklingEffect.transform.localScale = Vector3.zero;
        }
        //-----------------------------------------------------------------


        //プレイヤーが指定範囲内にいないか確認する
        foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (chara.GroupNo != CharacterGroupNumber.player) continue;

            m_playerCore = chara;

        }


    }

    void Update()
    {
        ////プレイヤーが指定範囲内にいないか確認する
        //foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
        //{
        //    if (chara.GroupNo != CharacterGroupNumber.player) continue;

        //    float dist = Vector3.Distance(chara.transform.position, this.transform.position);

        //    if (dist <= m_sensingRange)
        //    {
        //        OnItemEffect();
        //    }
        //    else
        //    {
        //        OffItemEffect();
        //    }

        //}


        if (m_playerCore == null) { return; }

        float dist = Vector3.Distance(m_playerCore.transform.position, this.transform.position);

        if (dist <= m_sensingRange)
        {
            OnItemEffect();
        }
        else
        {
            OffItemEffect();
        }


    }

    public void OnItemEffect()
    {

        if (m_sparklingEffect && m_bSparklingEffect)
        {

            m_sparklingEffect.SetActive(true);
            m_sparklingEffect.transform.DOScale(m_maxScale, m_duration);

        }

        if (m_auraEffect && m_bAuraEffect)
        {
            m_auraEffect.SetActive(true);
            m_auraEffect.transform.DOScale(m_maxScale, m_duration);
        }

        if (m_lightPillarEffect && m_bLightPillarEffect)
        {
            m_lightPillarEffect.SetActive(true);
            m_lightPillarEffect.transform.DOScale(m_maxScale, m_duration);
        }

    }

    public void OffItemEffect()
    {
        if (m_sparklingEffect && m_bSparklingEffect)
        {
            m_sparklingEffect.transform.DOScale(m_minScale, m_duration).OnComplete(() =>
            {
                m_sparklingEffect.SetActive(false);
            }
                );

        }

        if (m_auraEffect && m_bAuraEffect)
        {
            m_auraEffect.transform.DOScale(m_minScale, m_duration).OnComplete(() =>
            {
                m_auraEffect.SetActive(false);
            }
                );


        }

        if (m_lightPillarEffect && m_bLightPillarEffect)
        {
            m_lightPillarEffect.transform.DOScale(m_minScale, m_duration).OnComplete(() =>
            {
                m_lightPillarEffect.SetActive(false);
            }
            );

        }

    }

}
