using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodInfo;
using Cysharp.Threading.Tasks;

public class SelectProvideFoodUI : BaseProvideFoodUI
{

    // 制作者　田内
    // 提供する料理を選択するUI

    [Header("スロット作成")]
    [SerializeField]
    private CreateRecipeSlotList m_createRecipeSlotList = null;

    [Header("スクロール")]
    [SerializeField]
    private ChangeScrollViewPosition m_changeScrollViewPosition = null;


    public override async UniTask OnInitialize()
    {
        if (m_createRecipeSlotList == null)
        {
            Debug.LogError("スロット作成が存在しません");
        }

        m_createRecipeSlotList.OnInitialize();

        await UniTask.CompletedTask;
    }


    override public async UniTask OnSelectUpdate()
    {
        #region nullチェック

        if (m_selectUIController == null)
        {
            Debug.LogError("選択コントローラーがシリアライズされていません");
            return;
        }
        if (m_changeScrollViewPosition == null)
        {
            Debug.LogError("ChangeScrollViewPosコンポーネントがアタッチされていません");
            return;
        }
        #endregion

        //=================================================================================

        // UI選択更新
        m_selectUIController.OnUpdate();

        // スクロールビューの更新
        m_changeScrollViewPosition.OnUpdate();

        // 料理を選択
        SetProvideFood();

        await UniTask.CompletedTask;
    }



    // 提供料理をセットする
    private void SetProvideFood()
    {
        // 選択されれば
        if (m_selectUIController.IsPress == false) return;


        var itemSlotData = GetCurrentSelectItemSlotData<RecipeItemSlotData>();
        if (itemSlotData == null) return;

        // 作成出来ない状態であれば
        if (itemSlotData.IsCreate == false)
        {
            return;
        }

        // 既に追加されていれば
        if (ManagementProvideFoodManager.instance.IsAddedProvideFood((FoodID)itemSlotData.ItemID))
        {
            // 取り除く
            ManagementProvideFoodManager.instance.RemoveProvideFoodList((FoodID)itemSlotData.ItemID);
        }
        // まだ追加されていなければ
        else
        {
            // 追加する
            ManagementProvideFoodManager.instance.AddProvideFoodList((FoodID)itemSlotData.ItemID);
        }
    }




}
