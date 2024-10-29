using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//スキルの発射場所やエフェクトの発生場所をを取得するスクリプト
//あまり良い案が出ないので一旦これで代用
//スキルで出現するキャラクターにアタッチする（山本）


public class SetSkillStartPos : MonoBehaviour
{
    [Header("スキルエフェクトの開始する空オブジェクト")]
    [SerializeField] GameObject m_startSkillObject;

    private CharacterCore m_myCharacterCore;

    // Start is called before the first frame update
    void Start()
    {
        //自分についているキャラクターコアを取得
        m_myCharacterCore = transform.parent.GetComponent<CharacterCore>();

        //キャラクターコアのm_startSkillPosにセット
        m_myCharacterCore.PlayerSkillsParameters.StartSkillPos = m_startSkillObject.transform.position;

    }

    private void FixedUpdate()
    {
        //キャラクターコアのm_startSkillPosにセット
        m_myCharacterCore.PlayerSkillsParameters.StartSkillPos = m_startSkillObject.transform.position;
    }


}
