using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;
using IngredientInfo;
using PocketItemDataInfo;
using Cysharp.Threading.Tasks;


public class ConfirmationItemWindow : ConfirmationWindow
{

    // 制作者 田内
    // アイテムの説明文を表示する確認ウィンドウ


    [Header("説明文")]
    [SerializeField]
    private ChangeConfirmationItemDescription m_changeConfirmationItemDescription = null;

    // 表示アイテム種類ID
    private ItemTypeID m_itemTypeID = ItemTypeID.Ingredient;

    // 表示アイテムID
    private uint m_itemID = (uint)IngredientID.Egg;

    // ポケットタイプ
    private PocketType m_pocketType = PocketType.Inventory;

    //===============================================
    //              実行処理
    //===============================================

    /// <summary>
    /// 表示するアイテムのIDをセットする
    /// これをセットしないとタマゴしか表示できません
    /// </summary>
    public void SetDescription(PocketType _pocketType, ItemTypeID _typeID, uint _id)
    {
        m_pocketType = _pocketType;
        m_itemTypeID = _typeID;
        m_itemID = _id;
    }

    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_changeConfirmationItemDescription == null)
        {
            Debug.LogError("ChangeConfirmationItemDescriptionがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // 説明文更新
            m_changeConfirmationItemDescription.ChangeDescription(m_pocketType, m_itemTypeID, m_itemID);

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

}
