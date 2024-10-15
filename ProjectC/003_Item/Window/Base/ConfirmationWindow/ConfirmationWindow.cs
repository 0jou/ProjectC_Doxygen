using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ConfirmationWindow : BaseWindow
{
    // 制作者　田内
    // ただのウィンドウ、ボタンを押すと削除される

    //===========================================
    //              実行処理
    //===========================================

    public override async UniTask OnUpdate()
    {

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            while (cancelToken.IsCancellationRequested == false)
            {

                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // キャンセル
                if (IsClose())
                {
                    // 動作を行う
                    await UpdateConfirmation();

                    return;
                }

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    virtual protected async UniTask UpdateConfirmation()
    {
        // 派生クラスでの処理はこのメソッドをオーバーライドして行う
        await UniTask.CompletedTask;
    }


}
