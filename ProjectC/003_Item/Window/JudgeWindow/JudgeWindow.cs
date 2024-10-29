using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SelectUseItemInfo;


public class JudgeWindow : BaseWindow
{

    // 制作者(田内)

    [Header("スロットコントローラー")]
    [SerializeField]
    protected SelectUIController m_selectUIController = null;


    [Header("trueを返すUI")]
    [SerializeField]
    protected GameObject m_yesUI = null;


    [Header("falseを返すUI")]
    [SerializeField]
    protected GameObject m_noUI = null;

    // 返す値
    protected bool m_judgeFlg = false;


    //==========================================
    //              実行処理
    //==========================================


    // 通常のUpdate処理はこっち
    public override async UniTask OnUpdate()
    {
        // 通常ウィンドウの処理
        // bool値は関係ないので無視
        await OnSelfUpdate();
    }


    // ウィンドウ内で選択ウィンドウを作成した場合はこのUpdateを使う
    public async UniTask<bool> OnSelfUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // ボタンを押すまで
            while (cancelToken.IsCancellationRequested == false)
            {
                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();


                if (m_selectUIController == null)
                {
                    Debug.LogError("SlotContorollerコンポーネントがアタッチされていません");
                    return false;
                }

                // UI選択の更新
                m_selectUIController.OnUpdate();

                // 結果を返信
                if (CheckPressSelectButton())
                {
                    // 処理を行う
                    await UpdateJudge();
                    cancelToken.ThrowIfCancellationRequested();

                    return m_judgeFlg;
                }

                // UI選択の後処理
                m_selectUIController.OnLateUpdate();


                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();

            }


        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        return false;
    }

    // 選択後に行う処理
    virtual protected async UniTask UpdateJudge()
    {
        await UniTask.CompletedTask;
    }

    // ボタンが選択されたかを確認するメソッド 
    private bool CheckPressSelectButton()
    {
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerが登録されていません");
            return false;
        }

        // 選択されれば
        if (!m_selectUIController.IsPress) return false;

        var ui = m_selectUIController.CurrentSelectUI;


        // 選択されたのがYesUIであれば
        if (ui == m_yesUI)
        {
            m_judgeFlg = true;
            return true;
        }

        // 選択されたのがNoUIであれば
        else if (ui == m_noUI)
        {
            m_judgeFlg = false;
            return true;
        }


        return false;

    }

}
