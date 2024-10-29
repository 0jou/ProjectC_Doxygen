using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;

public class PutItemInstance : MonoBehaviour
{

    // 制作者 田内

    //===================================================================================


    [Header("アイテムのインスタンスを作成するタグ")]
    [SerializeField]
    private string m_tag = "Player";

    private GameObject m_targetObject = null;



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


    // アイテムのインスタンスを作成する
    // TODO:現在はアイテムをターゲットの位置に設置するだけなので、ターゲットに渡す
    public void Craft()
    {

        var data = ItemDataBaseManager.instance.GetItemData(m_itemTypeID, m_itemID);
        if (data == null)
        {
            Debug.LogError("このアイテムのデータは存在しません");
            return;
        }


        var prefab = data.ItemPrefab;
        if (prefab == null)
        {
            Debug.LogError("プレハブが存在しません");
            return ;
        }


        if (m_targetObject)
        {
            var trans = m_targetObject.transform;

            // プレハブを作成
            Instantiate(prefab, trans.position, trans.rotation);
        }
        else
        {
            Debug.Log("ターゲットが存在しません");

            Instantiate(prefab);
        }


        // 作成したアイテムを手持ちから削除する
        InventoryManager.instance.RemoveItem(data.ItemTypeID, data.ItemID);

        //ActionItemWindowの削除も行う（山本）
        GameObject Player = GameObject.FindWithTag("Player");
        if (Player)
        {
            Player = Player.transform.root.gameObject;

            if (Player.TryGetComponent(out PlayerParameters playerParameters))
            {
                GameObject actionItemWindow = playerParameters.ActionItemWindowController;

                ActionItemControllerWindow actionItemControllerWindow = actionItemWindow.GetComponentInChildren<ActionItemControllerWindow>();

                if(actionItemWindow)
                {
                    
                    if (actionItemControllerWindow.CreateSlotList)
                    {
                        var createSlotList = actionItemControllerWindow.CreateSlotList;
                        //actionItemControllerWindow.CreateSlotList.CreateSlotInstance(slotData.ItemTypeID, slotData.ItemID);
                        //初期化
                        createSlotList.OnInitialize();


                    }

                }
            }

        }

    }

}
