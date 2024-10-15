using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorySkillUIChangeColor : MonoBehaviour
{
    //ActionUISkillが使用できない際に色を暗くする為の処理（山本）
    [SerializeField] Image[] m_imagesList;
    private CharacterCore m_playerCharacterCore = null;

    private Color m_originColor = Color.clear;

    [SerializeField]
    private Color m_dontUseSkillColor = Color.gray;

    void Start()
    {
        // ここでm_playerCharacterCore探してたんですが、
        // CharacterCoreへの登録ができていないので、Updateで探すように変更しました（吉田）

        // 色情報はシリアライズされている物を使うように修正しました（吉田）
        foreach (var imag in m_imagesList)
        {
            m_originColor = imag.color;
            m_originColor.a = 0.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /* スキルが使えるかどうかで色を変える処理はBPが回復した時に処理するようにしました（吉田）
         * 実際の処理は BPSkill_1Presenter.cs と BPSkill_2Presenter.cs に移動しました

        // TODO：Startで探したい。
        if (m_playerCharacterCore == null)
        {
            foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (chara.InputType == CharacterCore.InputTypes.Player)
                {
                    m_playerCharacterCore = chara;
                }
            }
            if (m_playerCharacterCore == null)// nullチェック入れました（吉田）
            {
                Debug.LogError("プレイヤーが見つかりませんでした");
                return;
            }
        }
        var parameter = m_playerCharacterCore.PlayerParameters;

        if (parameter)
        {
            if (parameter.UseSkill1Flg)
            {
                foreach (var imag in m_imagesList)
                {
                    imag.color = m_originColor;
                }

            }
            else
            {
                foreach (var imag in m_imagesList)
                {
                    imag.color = m_dontUseSkillColor;
                }
            }

        }

        */
    }

    [Button("ChangeNoUseColor")]
    //グレーアウト処理（吉田）
    public void ChangeNoUseColor()
    {
        foreach (var imag in m_imagesList)
        {
            imag.color = m_dontUseSkillColor;
        }
    }

    [Button("ChangeUseColor")]
    //グレーアウト解除処理（吉田）
    public void ChangeUseColor()
    {
        foreach (var imag in m_imagesList)
        {
            imag.color = m_originColor;
        }
    }

}
