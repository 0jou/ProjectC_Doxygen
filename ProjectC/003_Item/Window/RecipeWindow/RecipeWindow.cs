using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodInfo;

using Cysharp.Threading.Tasks;
using PocketItemDataInfo;

public class RecipeWindow : BaseWindow
{
    // 制作者 田内
    // レシピを表示するウィンドウ

    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("スロット作成")]
    [SerializeField]
    private CreateRecipeSlotList m_createRecipeSlotList = null;

    [Header("説明文")]
    [SerializeField]
    private ChangeItemDescription m_changeItemDescription = null;

    [Header("スクロール")]
    [SerializeField]
    private ChangeScrollViewPosition m_changeScrollViewPosition = null;

    [Header("料理を作成するウィンドウコントローラー")]
    [SerializeField]
    private WindowController m_createFoodWindowController = null;

    [Header("ポケットタイプ")]
    [SerializeField]
    private PocketType m_pocketType = PocketType.Inventory;

    //==========================================
    //              実行処理
    //==========================================

    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_createRecipeSlotList == null)
        {
            Debug.LogError("CreateItemSlotListコンポーネントがアタッチされていません");
            return;
        }
        if (m_changeItemDescription == null)
        {

            Debug.LogError("ChangeRecipeExplanationコンポーネントがアタッチされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // スロット作成
            m_createRecipeSlotList.OnInitialize();

            // 説明文の初期化
            m_changeItemDescription.OnInitialize();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

    public override async UniTask OnUpdate()
    {
        #region nullチェック
        if (m_changeItemDescription == null)
        {
            Debug.LogError("ChangeCreateItemListPositionコンポーネントがアタッチされていません");
            return;
        }
        if (m_selectUIController == null)
        {
            Debug.LogError("SlotContorollerコンポーネントがアタッチされていません");
            return;
        }
        if (m_changeScrollViewPosition == null)
        {
            Debug.LogError("ChangeScrollViewPosコンポーネントがアタッチされていません");
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


                // スクロールビューの更新
                m_changeScrollViewPosition.OnUpdate();


                // 説明文の更新
                m_changeItemDescription.OnUpdate();


                // 料理作成
                await CreateFood();
                cancelToken.ThrowIfCancellationRequested();


                // UI選択の後処理
                m_selectUIController.OnLateUpdate();


                // ウィンドウを閉じる
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


    private async UniTask CreateFood()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }
        if (m_createFoodWindowController == null)
        {
            Debug.LogError("m_createFoodWindowControllerがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            if (m_selectUIController.IsPress == false) return;

            var currentUI = m_selectUIController.CurrentSelectUI;
            if (currentUI == null || currentUI.TryGetComponent<RecipeItemSlotData>(out var slotData) == false) return;

            // 料理を作成できるか確認
            if (slotData.IsCreate == false) return;


            // 料理作成ウィンドウ
            var controller = Instantiate(m_createFoodWindowController);
            await controller.CreateWindow<CreateFoodWindow>(false, async _ =>
            {
                _.SetFoodData(m_pocketType, (FoodID)slotData.ItemID);
                await UniTask.CompletedTask;
            });
            if (controller != null) Destroy(controller.gameObject);


            // レシピ情報を初期化
            InitializeRecipeData();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }


    // レシピ情報を初期化
    private void InitializeRecipeData()
    {
        // 現在作成可能か確認
        m_createRecipeSlotList.CheckCreate();

        // 説明文を初期化
        m_changeItemDescription.OnInitialize();
    }

}
