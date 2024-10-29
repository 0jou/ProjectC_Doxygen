using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SelectUseItemInfo;

using Cysharp.Threading.Tasks;
using ItemInfo;

public class CreateToUpdateSelectUseItemWindow : MonoBehaviour
{
    // アイテムの使用用途を選択するウィンドウを作成・結果確認を行うスクリプト
    // 制作者　田内

    [Header("SelectUseItemWindowコントローラー")]
    [SerializeField]
    private GameObject m_createSelectUseItemWindowController = null;


    public async UniTask<SelectUseItemID> Create<WindowType>(SelectUseItemWindowType _windowTypeID, ItemTypeID _typeID, uint _id, System.Func<WindowType, UniTask> onBeforeInitialize = null) where WindowType : SelectUseItemWindow
    {

        if (m_createSelectUseItemWindowController == null)
        {
            Debug.LogError("m_createSelectUseItemWindowControllerがシリアライズされていません");
            return SelectUseItemID.Exit;
        }


        // インスタンスを作成
        var window = Instantiate(m_createSelectUseItemWindowController);

        // コントローラーを取得
        var windowController = window.GetComponent<WindowController>();
        if (windowController == null)
        {
            Debug.LogError("WindowControllerコンポーネントが登録されていません");
            if (window) Destroy(window);
            return SelectUseItemID.Exit;
        }


        // デフォルト値を入れる
        if (onBeforeInitialize == null)
        {
            onBeforeInitialize = (async wnd =>
             {
                 // デフォルトの処理
                 wnd.SetData(_windowTypeID, _typeID, _id);

             });
        }

        //SE音追加（山本）
        SoundManager.Instance.StartPlayback("SceneChange");

        // 作成したウィンドウのアップデート
        var selectUseWindow = await windowController.CreateWindow<WindowType>(true, onBeforeInitialize);


        var cancelToken = selectUseWindow.GetCancellationTokenOnDestroy();


        SelectUseItemID id = SelectUseItemID.Exit;

        try
        {
            // ウィンドウの処理
            id = await selectUseWindow.OnUpdate();
            cancelToken.ThrowIfCancellationRequested();


            // 動作を決める
            // ウィンドウを閉じる
            await selectUseWindow.OnClose();
            cancelToken.ThrowIfCancellationRequested();


            // ウィンドウを削除
            await selectUseWindow.OnDestroy();
            cancelToken.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {

        }


        // ウィンドウGameObjectを削除
        if (window)
        {
            Destroy(window);
        }


        await UniTask.CompletedTask;

        return id;

    }




}
