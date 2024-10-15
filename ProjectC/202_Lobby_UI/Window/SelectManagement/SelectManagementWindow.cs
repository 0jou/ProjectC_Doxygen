using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;
using ButtonInfo;

using Cysharp.Threading.Tasks;

public class SelectManagementWindow : BaseWindow
{
    // 経営シーンを操作する、移動するウィンドウ
    // 制作者 田内

    [Header("スロットコントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;


    [Foldout("経営ウィンドウ")]
    [Header("経営ウィンドウ")]
    [SerializeField]
    private WindowController m_provideFoodWindowController = null;

    [Foldout("経営用倉庫ウィンドウ")]
    [Header("経営用倉庫ウィンドウ")]
    [SerializeField]
    private WindowController m_managementStorageWindowController = null;


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

                // UI選択の更新
                m_selectUIController.OnUpdate();

                // ボタンを選択
                await SelectButton();
                cancelToken.ThrowIfCancellationRequested();

                // 選択UIの後処理
                m_selectUIController.OnLateUpdate();

                // 閉じる
                if (IsClose()) return;

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }


    private async UniTask SelectButton()
    {
        // 選択したボタン
        var id = m_selectUIController.IsPressButton();

        switch (id)
        {
            // 経営倉庫ウィンドウを作成
            case ButtonID.ManagementStorage:
                {
                    var controller = Instantiate(m_managementStorageWindowController);
                    await CreateToUpdateWindow<BaseWindow>(controller, false);
                    if (controller != null) Destroy(controller.gameObject);
                    break;
                }
            // 提供料理選択ウィンドウを作成
            case ButtonID.ProvideFood:
                {
                    var controller = Instantiate(m_provideFoodWindowController);
                    await CreateToUpdateWindow<BaseWindow>(controller, false);
                    if (controller != null) Destroy(controller.gameObject);
                    break;
                }
        }
    }


}
