using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LobbyStateInfo;

public class BaseLobbyStateUpdate : BaseGameStateUpdate
{
    // 制作者 田内
    // 基底クラス

    [Header("自身のステート状態")]
    [SerializeField]
    protected LobbyState m_lobbyState = LobbyState.Normal;

    [Header("終了後に進むステート")]
    [SerializeField]
    protected LobbyState m_nextLobbyState = LobbyState.Normal;


    public override int GetState()
    {
        // ステートをセット
        return (int)m_lobbyState;
    }

    public override int GetNextState()
    {
        return (int)m_nextLobbyState;
    }


    /// <summary>
    /// ステート処理を終了する
    /// </summary>
    virtual protected void SetEnd(LobbyState _state)
    {
        m_nextLobbyState = _state;
        m_isEnd = true;
    }
}
