using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

public class UpdateWindow
{
    // 作成したウィンドウの処理をまとめたクラス
    // 制作者　田内

    // 処理を行う
    // 引数のGameObjectにはWindowControllerコンポーネントが必要
    public async UniTask Update<WindowType>(GameObject _window, System.Func<WindowType, UniTask> onBeforeInitialize = null) where WindowType : BaseWindow
    {

        var cancelToken = _window.GetCancellationTokenOnDestroy();

        try
        {

            if (_window == null)
            {
                Debug.LogError("引数のWindowControllerがnullです");
                return;
            }

            // コントローラーを取得
            var windowController = _window.GetComponent<WindowController>();
            if (windowController == null)
            {
                Debug.LogError("WindowControllerコンポーネントが登録されていません");
                return;
            }

            // 作成したウィンドウのアップデート
            await windowController.CreateWindow<WindowType>(false, onBeforeInitialize);
            cancelToken.ThrowIfCancellationRequested();

        }
        catch(System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

}
