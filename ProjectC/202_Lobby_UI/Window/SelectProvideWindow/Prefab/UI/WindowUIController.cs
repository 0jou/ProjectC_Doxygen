using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public class WindowUIController : MonoBehaviour
{
    // 制作者 田内
    // 同ウィンドウで操作を分けたいUIを操作するコントローラー

    [Header("WindowUIList\nElement0から順に選択")]
    [SerializeField]
    protected List<BaseWindowUI> m_baseWindowUIList = new();

    //================================
    // 現在実行中のWindowUI

    private BaseWindowUI m_currentUpdateBaseWindowUI = null;


    //================================
    // 選択中のカウント

    private int m_currentBaseWindowUICount = 0;


    //============================================================
    // 実行処理
    //============================================================

    


    /// <summary>
    /// 初期化処理
    /// </summary>
    public async UniTask OnInitialize()
    {
        if (m_baseWindowUIList.Count < 1)
        {
            Debug.LogError("UIが一つもセットされていません");
            return;
        }

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 一度全て初期化
            foreach (var ui in m_baseWindowUIList)
            {
                if (ui == null) continue;
                await ui.OnInitialize();

                cancelToken.ThrowIfCancellationRequested();

            }

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        // 選択UIをセット
        await SetUI();

        await UniTask.CompletedTask;
    }


    /// <summary>
    /// 実行処理
    /// </summary>
    public async UniTask OnUpdate()
    {
        // UIを選択
        await Select();

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 選択中のUIのUpdate処理
            if (m_currentUpdateBaseWindowUI != null)
            {
                await m_currentUpdateBaseWindowUI.OnSelectUpdate();

                cancelToken.ThrowIfCancellationRequested();
            }

            // 全てのUpdate処理
            foreach (var ui in m_baseWindowUIList)
            {
                if (ui == null) continue;
                await ui.OnUpdate();

                cancelToken.ThrowIfCancellationRequested();
            }


        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

    /// <summary>
    /// 後実行処理
    /// </summary>
    public async UniTask OnLateUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 全てのUpdate後処理
            foreach (var ui in m_baseWindowUIList)
            {
                if (ui == null) continue;
                await ui.OnLateUpdate();

                cancelToken.ThrowIfCancellationRequested();
            }

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    /// <summary>
    /// UIをセットする
    /// </summary>
    protected async UniTask SetUI()
    {

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 終了処理実行
            if (m_currentUpdateBaseWindowUI != null)
            {
                await m_currentUpdateBaseWindowUI.OnSelectExit();

                cancelToken.ThrowIfCancellationRequested();
            }

            // 実行するUIを変更
            m_currentUpdateBaseWindowUI = m_baseWindowUIList[m_currentBaseWindowUICount];

            // 透明度をセット
            SetAlpha();

            // 初期処理実行
            if (m_currentUpdateBaseWindowUI != null)
            {
                await m_currentUpdateBaseWindowUI.OnSelectInitialize();

                cancelToken.ThrowIfCancellationRequested();
            }

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }



    /// <summary>
    /// 色をセット
    /// </summary>
    protected void SetAlpha()
    {
        foreach (var ui in m_baseWindowUIList)
        {
            if (ui == null) continue;
            bool flg = false;

            // 一致していれば
            if (m_currentUpdateBaseWindowUI == ui)
            {
                flg = true;
            }

            ui.SetAlpha(flg);
        }
    }


    protected async UniTask Select()
    {
        // ToDO:選択キーassign修正


        if (Keyboard.current?.anyKey.wasPressedThisFrame == false && Gamepad.current?.wasUpdatedThisFrame == false) return;


        if (m_baseWindowUIList.Count <= 0)
        {
            return;
        }

        bool flg = false;

        // 後ろに進む
        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            flg = Down();
        }


        // 前に進む
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            flg = Up();
        }


        if (flg)
        {
            await SetUI();

            //SE音追加（山本）
            SoundManager.Instance.StartPlayback("Select");
        }

    }


    protected bool Up()
    {
        // 前へ移動
        m_currentBaseWindowUICount++;

        if (m_baseWindowUIList.Count <= m_currentBaseWindowUICount)
        {
            m_currentBaseWindowUICount = m_baseWindowUIList.Count - 1;
            return false;
        }

        // 変更が加われば
        return true;

    }

    protected bool Down()
    {
        // 後ろへ移動
        m_currentBaseWindowUICount--;

        if (m_currentBaseWindowUICount < 0)
        {
            m_currentBaseWindowUICount = 0;
            return false;
        }

        // 変更が加われば
        return true;

    }




}
