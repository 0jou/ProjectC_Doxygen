using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;
using FoodInfo;

public class CreateManagementProvideFoodControllerSlotList : BaseCreateSlotList
{
    // 制作者 田内
    // 経営提供料理選択コントローラーを基にスロットを作成


    [Header("経営提供料理コントローラー")]
    [SerializeField]
    protected SelectManagementProvideFoodController m_selectManagementProvideFoodController = null;

    //===========================================
    //               実行処理
    //===========================================

    override protected void CreateSlotInstance()
    {
        // まだ作成されていないスロットを作成する
        var FoodIDList = GetCreateList();

        // 作成
        foreach (var id in FoodIDList)
        {
            // 子オブジェクトにスロットを追加
            var slot = Instantiate(m_slot, transform);

            if (slot.TryGetComponent<ManagementProvideFoodControllerSlotData>(out var slotData))
            {
                // スロットのデータをセット
                var itemData = ItemDataBaseManager.instance.GetItemData(ItemTypeID.Food, (uint)id);
                slotData.SetSelectManagementProvideFoodController(m_selectManagementProvideFoodController);
                slotData.SetItemSlotData(itemData, ManagementProvideFoodManager.instance.PocketType);

                // リストに追加
                m_slotList.Add(slotData);

            }
            else
            {
                Debug.LogError("ProvideFoodSlotDataコンポーネントがアタッチされていません");
            }

            // UIcontrollerに追加
            AddSelectUIControler(slot);
        }
    }


    private List<FoodID> GetCreateList()
    {
        List<FoodID> creatSlotList = new();

        #region nullチェック
        if (m_selectManagementProvideFoodController == null)
        {
            Debug.LogError("ManagementProvideFoodControllerがシリアライズされていません");
            return creatSlotList;
        }
        #endregion

        foreach (var id in m_selectManagementProvideFoodController.ProvideFoodIDList)
        {
            // 作成済みか
            bool isCreate = false;

            // 既存のスロットに存在していれば作成しない
            foreach (var slot in m_slotList)
            {
                if (slot == null) continue;

                if (slot.TryGetComponent<ManagementProvideFoodControllerSlotData>(out var slotData))
                {
                    // 既に作成されていれば
                    if ((FoodID)slotData.ItemID == id)
                    {
                        isCreate = true;
                        break;
                    }
                }
            }

            // まだ作成されていなければ
            if (isCreate == false)
            {
                creatSlotList.Add(id);
            }
        }

        return creatSlotList;

    }




}
