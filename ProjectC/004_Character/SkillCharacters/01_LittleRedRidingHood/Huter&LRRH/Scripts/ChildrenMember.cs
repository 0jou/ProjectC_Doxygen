using Arbor;
using Arbor.Calculators;
using nTools.PrefabPainter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ChildrenMember : MonoBehaviour
{
    [SerializeField] public CharacterCore m_akazukin;
    [SerializeField] public Vector3 m_akazukinPosition;

    [SerializeField] public CharacterCore m_hunter;
    [SerializeField] public Vector3 m_hunterPosition;
   
    [SerializeField] public CharacterCore m_oldWoman;
    [SerializeField] public Vector3 m_oldWomanPosition;
   

    [SerializeField] public float m_akazukinHigh = 0.0f;
    [SerializeField] public float m_hunterHigh = 0.0f;
    [SerializeField] public float m_oldWomanHigh = 0.0f;


    public void SetPosition()
    {

        Vector3 skillCharacterPos = Vector3.zero;

        skillCharacterPos = transform.position + m_akazukinPosition;
        m_akazukin.CharaCtrl.SetPositionMotor(skillCharacterPos + (Vector3.up * m_akazukinHigh));
        m_akazukin.PlayerSkillsParameters.SwitchPathfinding(false);
       
        skillCharacterPos = transform.position + m_hunterPosition;
        m_hunter.CharaCtrl.SetPositionMotor(skillCharacterPos + (Vector3.up * m_hunterHigh));
        m_hunter.PlayerSkillsParameters.SwitchPathfinding(false);
       
        skillCharacterPos = transform.position + m_oldWomanPosition;
        m_oldWoman.CharaCtrl.SetPositionMotor(skillCharacterPos + (Vector3.up * m_oldWomanHigh));
        m_oldWoman.PlayerSkillsParameters.SwitchPathfinding(false);
       
        
    }


}
