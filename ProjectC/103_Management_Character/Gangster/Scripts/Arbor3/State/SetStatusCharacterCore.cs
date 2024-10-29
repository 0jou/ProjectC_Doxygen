/*
 * @file SetStatusCharacterCore.cs
 * @brief CharatcerStatus の 任意の値を設定するステート
 * 現在はHPの設定のみ対応
 * @author 上甲
 * @todo 他のステータスの設定に対応する
 */

using Arbor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arbor.Extensions;
using Newtonsoft.Json.Bson;

/// <summary>
/// @brief CharatcerStatus の 任意の値を設定するステート
/// </summary>
public class SetStatusCharacterCore : BaseGangsterStateBehaviour
{
    [SerializeField] FlexibleFloat m_hpValue;

    private CharacterCore m_characterCore;

    private void SetValue()
    {
        if(m_characterCore == null)
        {
            Debug.LogError("CharacterCore is null");
            return;
        }
        m_characterCore.Status.m_hp.Value = m_hpValue.value;
    }

    public override void OnStateBegin()
    {
        m_characterCore = GetGangsterData().GetComponent<CharacterCore>();

        SetValue();
    }
}
