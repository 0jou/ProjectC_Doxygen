using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

public class NewItemWindow : BaseWindow
{
    // 新規獲得したアイテムを表示するウィンドウ
    // 制作者　田内

    [Header("スロット作成")]
    [SerializeField]
    private CreatePocketItemSlotList m_createItemSlotList = null;


    [Header("説明欄")]
    [SerializeField]
    private ChangeItemDescription m_changeItemSlotDescription = null;


    [Header("スロットコントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("スクロール")]
    [SerializeField]
    private ChangeScrollViewPosition m_changeScrollViewPosition = null;


    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_createItemSlotList == null)
        {
            Debug.LogError("CreateItemSlotListコンポーネントがアタッチされていません");
            return;
        }

        if (m_changeItemSlotDescription == null)
        {
            Debug.LogError("ChangeItemSlotDescriotionコンポーネントがアタッチされていません");
            return;
        }
        #endregion


        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

           
            // スロット作成
            m_createItemSlotList.OnInitialize();

            await UniTask.DelayFrame(1);
            cancelToken.ThrowIfCancellationRequested();

            // 説明文を初期化
            m_changeItemSlotDescription.OnInitialize();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }


    public override async UniTask OnUpdate()
    {

        #region nullチェック

        if (m_selectUIController == null)
        {
            Debug.LogError("SlotContorollerコンポーネントがアタッチされていません");
            return;
        }

        if (m_changeItemSlotDescription == null)
        {
            Debug.LogError("ChangeItemSlotDescriptionコンポーネントがアタッチされていません");
            return;
        }

        if (m_changeScrollViewPosition == null)
        {
            Debug.LogError("ChangeScrollViewPositionがシリアライズされていません");
            return;
        }

        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            while (cancelToken.IsCancellationRequested == false)
            {

                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();


                // UIコントローラーの更新
                m_selectUIController.OnUpdate();

                // 説明文の更新
                m_changeItemSlotDescription.OnUpdate();

                // スクロールの更新
                m_changeScrollViewPosition.OnUpdate();

                // キャンセル
                if (IsClose())
                {
                    return;
                }

                // UIコントローラーの後処理
                m_selectUIController.OnLateUpdate();


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
