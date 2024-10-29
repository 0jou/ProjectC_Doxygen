/*!
 * @file IsDeadGangster.cs
 * @brief コア又はステートが死亡状態であればステートを遷移させるステート
 * @author 田内
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

/// <summary>
/// @brief コア又はステートが死亡状態であればステートを遷移させるステート
/// @details GangsterState.Dead/HP0以下の条件下で遷移
/// </summary>
[AddComponentMenu("Gangseter/IsDestroyGangster")]
public class IsDeadGangster : BaseGangsterStateBehaviour
{
    // 制作者　田内
    // ヤンキーを削除するかどうか

    //=============================
    // 監視するキャラクターコア

    private CharacterCore m_characterCore = null;

    //=============================
    // ステートリンク

    [SerializeField]
    private StateLink m_deadLink = null;

    //==========================================================


    private void SetCharacterCore()
    {
        var data = GetGangsterData();
        if (data == null) return;

        m_characterCore = data.GetComponent<CharacterCore>();

        if (m_characterCore == null)
        {
            Debug.LogError("CharacterCoreが存在しません");
            SetTransition(m_deadLink);
        }

    }

    //! @brief 死亡状態か確認 Updateで回す
    private void DeadCheck()
    {
        if(IsStateDead()|| IsHPDead())
        {
            SetTransition(m_deadLink);
        }
    }

    //! @brief ステート(GangsterState)が死亡状態か確認
    private bool IsStateDead()
    {
        var data = GetGangsterData();
        if (data == null) return false;

        if (data.CurrentGangsterState == GangsterStateInfo.GangsterState.Dead)
        {
            return true;
        }

        return false;
    }

    //! @brief HPが0以下か確認
    private bool IsHPDead()
    {
        if (m_characterCore == null) return false;

        if (m_characterCore.Status.m_hp.Value <= 0.0f)
        {
            return true;
        }

        return false;
    }


    // Use this for enter state
    public override void OnStateBegin()
    {
        // セットする
        SetCharacterCore();
    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        DeadCheck();
    }
}
