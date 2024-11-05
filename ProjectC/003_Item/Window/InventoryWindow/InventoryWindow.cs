using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;
using SelectUseItemInfo;

// 制作者(田内)

public class InventoryWindow : BaseWindow
{


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


    [Header("使用用途選択ウィンドウコントローラー")]
    [SerializeField]
    private CreateToUpdateSelectUseItemWindow m_createSelectUseItemWindow = null;


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

            // 説明分の初期化
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
        // Nullチェック
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
            Debug.Log("ChangeScrollViewPositionがシリアライズされていません");
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

                // UI選択の更新
                m_selectUIController.OnUpdate();

                // スクロールの更新
                m_changeScrollViewPosition.OnUpdate();

                // 説明文の更新
                m_changeItemSlotDescription.OnUpdate();

                // アイテム使用用途選択ウィンドウを作成する
                await CreateSelectUseCraftItemWindow();
                cancelToken.ThrowIfCancellationRequested();

                // UI選択の後処理
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



    // アイテムウィンドウを作成
    private async UniTask CreateSelectUseCraftItemWindow()
    {
        #region nullチェック
        if (m_createSelectUseItemWindow == null)
        {
            Debug.LogError("m_createJudgeWindowがシリアライズされていません");
            return;
        }
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIコントローラーが登録されていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 選択したか
            if (m_selectUIController.IsPress == false) return;

            // 選択しているUIがなければ
            if (m_selectUIController.CurrentSelectUI == null) return;

            // 選択しているUIがスロットであれば
            var slotData = m_selectUIController.CurrentSelectUI.GetComponent<ItemSlotData>();
            if (slotData == null) return;

            // ウィンドウを作成/動作
            SelectUseItemID selectUseItemID = await m_createSelectUseItemWindow.Create<SelectUseItemWindow>(SelectUseItemWindowType.Inventory, slotData.ItemTypeID, slotData.ItemID);
            cancelToken.ThrowIfCancellationRequested();

            // 動作を決める
            switch (selectUseItemID)
            {
                case SelectUseItemID.Put:
                    {
                        // アイテムを設置する
                        // プレイヤーへアイテム情報を送る(吉田)
                        GameObject player = GameObject.FindGameObjectWithTag("Player");
                        if (player != null)
                        {
                            player.GetComponent<CharacterCore>()?.PlayerParameters.
                                SetPutItemInfo(slotData.ItemTypeID, slotData.ItemID);
                        }

                        break;
                    }

                case SelectUseItemID.Eat:
                    {
                        // 何もせず戻る
                        break;
                    }

                case SelectUseItemID.Exit:
                    {
                        // 何もせず戻る
                        break;
                    }

                default:
                    {
                        Debug.LogError("IDが当てはまりません");
                        break;
                    }
            }

            return;

        }
        catch
        {
            return;
        }
    }

}
