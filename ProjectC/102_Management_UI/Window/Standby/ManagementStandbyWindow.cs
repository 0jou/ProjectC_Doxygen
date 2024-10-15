using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementStandbyWindow : BaseWindow
{
    // 制作者 田内
    // 経営シーンの待機画面


    //=====================================
    //          実行処理
    //=====================================

    public override async UniTask OnUpdate()
    {
      
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // ボタンを押すまで
            while (cancelToken.IsCancellationRequested == false)
            {
                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // ボタンが押されたら
                if(IsClose()) return;

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }



}
