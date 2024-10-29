using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

using NaughtyAttributes;

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


    //================================
    // ボタン

    [BoxGroup("Button")]
    [Header("Next")]
    [SerializeField]
    private InputActionButton m_nextInputActionButton = null;


    [BoxGroup("Button")]
    [Header("Back")]
    [SerializeField]
    private InputActionButton m_backInputActionButton = null;


    //=======================================
    //               実行処理
    //=======================================


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
        if (Keyboard.current?.anyKey.wasPressedThisFrame == false && Gamepad.current?.wasUpdatedThisFrame == false) return;

        #region nullチェック
        if (m_nextInputActionButton == null)
        {
            Debug.LogError("NextInputActionButtonがシリアライズされていません");
            return;
        }
        if (m_backInputActionButton == null)
        {
            Debug.LogError("BackInputActionButtonがシリアライズされていません");
            return;
        }
        #endregion


        if (m_baseWindowUIList.Count <= 0) return;

        // 後ろに進む
        if (m_backInputActionButton.IsInputActionTrriger())
        {
            if (Back())
            {
                await SetUI();
            }
        }

        // 前に進む
        if (m_nextInputActionButton.IsInputActionTrriger())
        {
            if (Next())
            {
                await SetUI();
            }
        }
    }


    protected bool Next()
    {
        if (IsGoNext())
        {
            m_currentBaseWindowUICount++;
            return true;
        }
        return false;
    }


    protected bool Back()
    {
        if (IsGoBack())
        {
            m_currentBaseWindowUICount--;
            return true;
        }
        return false;
    }


    public bool IsGoNext()
    {
        if (m_currentBaseWindowUICount < m_baseWindowUIList.Count - 1)
        {
            return true;
        }
        return false;
    }


    public bool IsGoBack()
    {
        if (0 < m_currentBaseWindowUICount)
        {
            return true;
        }
        return false;
    }

}
