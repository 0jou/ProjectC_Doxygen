using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingEffectByLRRH : MonoBehaviour
{
    //赤ずきんスキルで使用するEffectプレハブを登録するコーポネント（仮）
    [SerializeField] GameObject AuraEffectPrefab;
    public GameObject AuraEffect => AuraEffectPrefab;
    public GameObject m_auraEffect { get; set; }


    [SerializeField] GameObject TrailEffectPrefab;
    public GameObject TrailEffect => TrailEffectPrefab;
    public GameObject m_trailEffect { get; set; }



    [SerializeField] GameObject SmokeEffectPrefab;
    public GameObject SmokeEffect => SmokeEffectPrefab;
    public GameObject m_smokeEffect { get; set; }



    public bool DesroyAuraEffect()
    {
        if(AuraEffectPrefab == null)
        {
            return false;
        }

        Destroy(AuraEffectPrefab);
        return true;
    }

    public void Awake()
    {
        
    }


}
