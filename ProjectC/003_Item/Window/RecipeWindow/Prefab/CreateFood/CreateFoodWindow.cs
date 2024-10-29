using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

using ItemInfo;
using FoodInfo;
using PocketItemDataInfo;

using NaughtyAttributes;

public class CreateFoodWindow : BaseWindow
{
    // 制作者 田内
    // 料理を作成するウィンドウ

    [Header("説明文")]
    [SerializeField]
    private ChangeItemDescription m_changeItemDescription = null;

    [Header("料理作成コントローラー")]
    [SerializeField]
    private CreateFoodController m_createFoodController = null;

    [Header("料理作成コントローラーの説明文")]
    [SerializeField]
    private ChangeCreateFoodControllerDescription m_changeCreateFoodControllerDescription = null;

    [Header("操作する料理ID")]
    [SerializeField]
    private FoodID m_foodID = FoodID.Omelette;

    [Header("ポケットタイプ")]
    [SerializeField]
    private PocketType m_pocketType = PocketType.Inventory;

    //==========================================
    //              実行処理
    //==========================================


    /// <summary>
    /// 作成する料理のデータをセットする
    /// </summary>
    public void SetFoodData(PocketType _pocketType, FoodID _id)
    {
        m_pocketType = _pocketType;
        m_foodID = _id;
    }


    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_changeItemDescription == null)
        {
            Debug.LogError("ChangeItemDescriptionがシリアライズされていません");
            return;
        }
        if (m_createFoodController == null)
        {
            Debug.LogError("CreateFoodControllerがシリアライズされていません");
            return;
        }
        if(m_changeCreateFoodControllerDescription==null)
        {
            Debug.LogError("ChangeCreateFoodControllerDescriptionがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // 説明文更新
            m_changeItemDescription.ChangeDescription(m_pocketType, ItemTypeID.Food, (uint)m_foodID);

            // コントローラー更新
            m_createFoodController.SetFoodData(m_pocketType, m_foodID);

            // コントローラーの説明文初期化
            m_changeCreateFoodControllerDescription.OnInitialize();

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
            Debug.LogError("ChangeItemDescriptionがシリアライズされていません");
            return;
        }
        if (m_createFoodController == null)
        {
            Debug.LogError("CreateFoodControllerがシリアライズされていません");
            return;
        }
        if (m_changeCreateFoodControllerDescription == null)
        {
            Debug.LogError("ChangeCreateFoodControllerDescriptionがシリアライズされていません");
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


                // コントローラーの更新
                await m_createFoodController.OnUpdate();

                // 説明文を更新
                if (m_createFoodController.IsSelectChangeFlg)
                {
                    m_changeItemDescription.ChangeDescription(m_pocketType, ItemTypeID.Food, (uint)m_foodID);
                    m_changeCreateFoodControllerDescription.OnUpdate();
                }

                // コントローラーの後処理
                m_createFoodController.OnLateUpdate();

                // ウィンドウを閉じる
                if (IsClose())
                {
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

}
