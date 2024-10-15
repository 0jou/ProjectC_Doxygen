/*!
 * @file SoundManager.cs
 * @brief サウンドの再生を管理するクラス
*/

using Arbor.Examples;
using CriWare;
using MagicaCloth2;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static CriWare.CriProfiler;
using static SoundManager;


/// <summary>
/// @brief SoundManager クラス
/// サウンドの文字列からの再生を管理するクラス
/// 2D,3Dの再生を管理する
/// @detail 3D再生のみ CriAtomSourceを必須とする(マネージャー側で追加する)
/// 2D再生では同一音源の同時再生が現在不可能だが、3D再生では可能
/// </summary>
public class SoundManager : IDisposable
{
    public static SoundManager Instance { get; } = new SoundManager();

    private CriAtomExPlayer m_bgmPlayer;
    public CriAtomExPlayer BGMPlayer { get => m_bgmPlayer; }

    private CriAtomExPlayback m_currentBGM;

    private bool m_disposedValue;

    //public static float CalcDspTime(float pitch) { return 1 / Mathf.Pow(2, pitch / 1200.0f); }

    /// <summary>
    /// @brief 2Dでのサウンド再生
    /// </summary>
    /// <param name="cueName">サウンドの識別名称</param>
    /// <param name="vol">音量設定(0~1)</param>
    /// <param name="pitch">音程設定(-2400~2400) 1200で1オクターブ変わる</param>
    /// <returns>CriAtomExPlayback</returns>
    public CriAtomExPlayback StartPlayback(string cueName, float vol = 1.0f, float pitch = 0)
    {
        var player = new CriAtomExPlayer(); // 新しいプレイヤーを生成
        player.SetCue(null, cueName);
        player.SetVolume(vol);
        player.SetPitch(pitch);
        // player.SetDspTimeStretchRatio(CalcDspTime(pitch));
        CriAtomExPlayback pb = player.Start(); // 再生

        return pb;
    }
    /// <summary>
    /// @brief 2Dでのサウンド再生
    /// </summary>
    /// <param name="data">サウンドデータクラス</param>
    /// <returns>CriAtomExPlayback</returns>
    public CriAtomExPlayback StartPlayback(SoundData data)
    {
        return StartPlayback(data.m_soundName, data.m_volume, data.m_pitch);
    }

    /// <summary>
    /// @brief 座標のみ指定での3D再生 指定された座標にオブジェクトを作成しCriAtomExPlayer,Instant3DSoundPlayerを紐づけ再生
    /// 再生終了時自動でオブジェクトが破棄される
    /// </summary>
    /// <param name="cueName">サウンドの識別名称</param>
    /// <param name="pos">座標</param>
    /// <param name="vol">音量設定(0~1)</param>
    /// <param name="pitch">音程設定(-2400~2400) 1200で1オクターブ変わる</param>
    /// <returns>CriAtomExPlayback</returns>
    public CriAtomExPlayback Start3DPlayback(string cueName, UnityEngine.Vector3 pos, float vol = 1.0f, float pitch = 0)
    {
        GameObject obj = new GameObject();
        obj.transform.position = pos;
        var source = obj.AddComponent<CriAtomSource>();
        var instantPlayer = obj.AddComponent<Instant3DSoundPlayer>();
        CriAtomEx3dSource source3D = new CriAtomEx3dSource();
        CriAtomExPlayer exPlayer = source.player;
        instantPlayer.CriSource = source;
        exPlayer.SetCue(null, cueName);
        exPlayer.SetVolume(vol);
        exPlayer.SetPitch(pitch);
        // exPlayer.SetDspTimeStretchRatio(CalcDspTime(pitch));
        exPlayer.Set3dSource(source3D); //3D音源の紐づけ
        exPlayer.SetPanType(CriAtomEx.PanType.Auto);
        CriAtomExPlayback pb = exPlayer.Start(); // 再生
        return pb;
    }

    /// <summary>
    /// @brief 座標のみ指定での3D再生 指定された座標にオブジェクトを作成しCriAtomExPlayer,Instant3DSoundPlayerを紐づけ再生
    /// 再生終了時自動でオブジェクトが破棄される
    /// </summary>
    /// <param name="data">サウンドデータクラス</param>
    /// <param name="pos">座標</param>
    /// <returns>CriAtomExPlayback</returns>
    public CriAtomExPlayback Start3DPlayback(SoundData data, UnityEngine.Vector3 pos)
    {
        return Start3DPlayback(data.m_soundName, pos, data.m_volume, data.m_pitch);
    }

    /// <summary>
    /// @brief 3D再生 指定されたオブジェクトに音源を設定し再生
    /// </summary>
    /// <param name="cueName">サウンドの識別名称</param>
    /// <param name="obj">オブジェクト</param>
    /// <param name="vol">音量設定(0~1)</param>
    /// <param name="pitch">音程設定(-2400~2400) 1200で1オクターブ変わる</param>
    /// <returns>CriAtomExPlayback</returns>
    public CriAtomExPlayback Start3DPlayback(string cueName, GameObject obj, float vol = 1.0f, float pitch = 0)
    {
        var atom = obj.AddComponent<CriAtomSource>();
        atom.cueName = cueName;
        atom.cueSheet = null;
        atom.volume = vol;
        atom.pitch = pitch;
        atom.use3dPositioning = true;
        return atom.Play();
    }

    /// <summary>
    /// @brief 3D再生 指定されたオブジェクトに音源を設定し再生
    /// </summary>
    /// <param name="cueName">サウンドの識別名称</param>
    /// <param name="obj">オブジェクト</param>
    /// <param name="vol">音量設定(0~1)</param>
    /// <param name="pitch">音程設定(-2400~2400) 1200で1オクターブ変わる</param>
    /// <returns>CriAtomExPlayback</returns>
    public CriAtomExPlayback Start3DPlayback(SoundData data, GameObject obj)
    {
        return Start3DPlayback(data.m_soundName, obj, data.m_volume, data.m_pitch);
    }

    /// <summary>
    /// @brief BGM再生
    /// </summary>
    /// <param name="cueName">サウンドの識別名称</param>
    /// <param name="vol">音量設定(0~1)</param>
    /// <param name="pitch">音程設定(-2400~2400) 1200で1オクターブ変わる</param>
    public void StartBGM(string cueName, float vol = 1.0f, float pitch = 0)
    {
        m_bgmPlayer.SetCue(null, cueName);
        m_bgmPlayer.SetVolume(vol);
        m_bgmPlayer.SetPitch(pitch);
        // m_bgmPlayer.SetDspTimeStretchRatio(CalcDspTime(pitch));
        m_bgmPlayer.Loop(true);
        m_currentBGM = m_bgmPlayer.Start();
    }

    //! @brief BGM再生停止
    public void StopBGM()
    {
        m_currentBGM.Stop();
    }

    //! @brief BGM一時停止
    public void PauseBGM()
    {
        m_currentBGM.Pause();
    }

    //! @brief BGM再開
    public void ResumeBGM()
    {
        m_bgmPlayer.Resume(CriAtomEx.ResumeMode.PausedPlayback);
    }

    /// <summary>
    /// @brief BGMの音量とピッチを設定
    /// </summary>
    /// <param name="vol">音量設定(0~1)</param>
    /// <param name="pitch">音程設定(-2400~2400) 1200で1オクターブ変わる</param>
    public void SetBGMVolumeAndPitch(float vol, float pitch)
    {
        m_bgmPlayer.SetVolume(vol);
        m_bgmPlayer.SetPitch(pitch);
        // m_bgmPlayer.SetDspTimeStretchRatio(CalcDspTime(pitch));
    }

    //! @brief BGM再生中かどうか
    public bool IsBGMPaused()
    {
        return m_currentBGM.IsPaused();
    }

    private SoundManager()
    {
        this.m_bgmPlayer = new CriAtomExPlayer();
    }

    //! @brief 破棄処理
    private void Dispose(bool disposing)
    {
        if (!m_disposedValue)
        {
            m_bgmPlayer?.Dispose();
            m_bgmPlayer = null;
            m_disposedValue = true;
        }
    }

    ~SoundManager()
    {
        Dispose(disposing: false);
    }

    //! @brief 破棄処理
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// @brief SoundManagerの拡張クラス Loop指定での再生を行う
/// </summary>
public static class ExtensionSoundManager
{
    /// <summary>
    /// @brief 2Dループ再生を行う
    /// </summary>
    /// <param name="manager">拡張元</param>
    /// <param name="cueName">サウンドの識別名称</param>
    /// <param name="isLoop">ループするか</param>
    /// <param name="vol">音量設定(0~1)</param>
    /// <param name="pitch">音程設定(-2400~2400) 1200で1オクターブ変わる</param>
    /// <returns>CriAtomExPlayback</returns>
    public static CriAtomExPlayback SetLoopPlayback(this SoundManager manager, string cueName, bool isLoop, float vol = 1.0f, float pitch = 0)
    {
        var player = new CriAtomExPlayer(); // 新しいプレイヤーを生成
        player.SetCue(null, cueName);
        player.SetVolume(vol);
        player.SetPitch(pitch);
        //  player.SetDspTimeStretchRatio(SoundManager.CalcDspTime(pitch));
        player.Loop(isLoop);

        var pb = manager.StartPlayback(cueName, vol, pitch);
        return pb;
    }
}

[Serializable]
/// <summary>
/// @brief SoundData クラス
/// @detail サウンドの再生に必要なデータを保有する
/// </summary>
public class SoundData
{
    public string m_soundName;
    [Range(0.0f, 1.0f)]
    public float m_volume = 1.0f;
    [Range(-2400, 2400)]
    public float m_pitch = 0.0f;
}