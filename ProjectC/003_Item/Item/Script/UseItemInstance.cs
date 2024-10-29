using ItemInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemInstance : MonoBehaviour
{
    //作成者　山本
    //アイテム食事状態時に選んだアイテムのインスタンス生成とプレイヤーに食事効果を付与する
    //参考→PutItemInstance.cs,ThrowItemInstance.cs

    //===================================================================================

    [Header("アイテムのインスタンスを作成するタグ")]
    [SerializeField]
    private string m_tag = "Player";

    private GameObject m_targetObject = null;
    private Transform m_holdPointTrans = null;
    //キャッシュ用
    //private GameObject m_itemObj = null;


    //===================================================================================

    private ItemTypeID m_itemTypeID = new();

    public ItemTypeID ItemTypeID { set { m_itemTypeID = value; } get { return m_itemTypeID; } }


    private uint m_itemID = new();

    public uint ItemID { set { m_itemID = value; } get { return m_itemID; } }

    //===================================================================================

    private void Awake()
    {

        // Playerを探す
        GameObject[] getObj = GameObject.FindGameObjectsWithTag(m_tag);

        foreach (var obj in getObj)
        {
            m_targetObject = obj;
        }

    }

    public void SetItemID(ItemTypeID _itemTypeID, uint _itemID)
    {
        m_itemTypeID = _itemTypeID;
        m_itemID = _itemID;
    }

    public void SetHoldPoint(Transform _holdPointTrans)
    {
        m_holdPointTrans = _holdPointTrans;
    }

    // アイテムのインスタンスをHoldPointに作成する
    public void UseItem()
    {

        var data = ItemDataBaseManager.instance.GetItemData<FoodData>(m_itemTypeID, m_itemID);
        if (data == null)
        {
            Debug.LogError("この料理データは存在しません");
            return;
        }


        var prefab = data.ItemPrefab;
        if (prefab == null)
        {
            Debug.LogError("プレハブが存在しません");
            return;
        }

        //食べられないなら早期リターン
        if (!data.IsEat)
        {
            Debug.LogError("食べられません");
            return;
        }

        //プレイヤーのHPを回復
        if (data.HealingValue != 0.0f)
        {
            if (m_targetObject.TryGetComponent(out CharacterCore core))
            {
                var hp = core.Status.m_hp.Value += data.HealingValue;
                //最大値以上回復するなら最大値にする
                if (hp > core.Status.MaxHP)
                {
                    core.Status.m_hp.Value = core.Status.MaxHP;
                }

            }
        }

        // 使用したアイテムを手持ちから削除する
        InventoryManager.instance.RemoveItem(data.ItemTypeID, data.ItemID);

    }


}
