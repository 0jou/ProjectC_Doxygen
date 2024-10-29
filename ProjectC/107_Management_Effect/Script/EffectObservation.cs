using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.VFX;

//エフェクト監視コンポーネント（山本）
//指定されたエフェクトを読み取り、終了しているかを検知する

public class EffectObservation : MonoBehaviour
{
    //監視用エフェクト
    private GameObject m_observEffect = null;

    public void SetObservationEffect(GameObject _effect)
    {
        if(_effect)
        m_observEffect= _effect;
    }

    public GameObject GetObservationEffect()
    {
       return m_observEffect;
    }

    public bool IsFinishedEffect()
    {
        if(m_observEffect.TryGetComponent(out VisualEffect _effect))
        {
            if (_effect.HasAnySystemAwake() == false)
            {

                return true;
            }
            else 
            {
                return false; 
            }  
                
        }
        else
        {
            Debug.LogWarning("VisualEffectが存在しません");
            return false;
        }
    }

}
