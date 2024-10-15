using ItemInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldItemInstance : MonoBehaviour
{
    //作成者　山本
    //アイテム持ち状態時に選んだアイテムを生成する
    //参考→PutItemInstance.cs,ThrowItemInstance.cs

    //===================================================================================

    [Header("アイテムのインスタンスを作成するタグ")]
    [SerializeField]
    private string m_tag = "Player";

    private GameObject m_targetObject = null;
    private Transform m_holdPointTrans = null;
    //キャッシュ用
    private GameObject m_itemObj = null;


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
            return;
        }


        if (m_holdPointTrans)
        {
            var trans = m_holdPointTrans;

            // プレハブを作成
            m_itemObj = Instantiate(prefab, trans);

            foreach (Transform child in m_itemObj.transform)
            {
                if (child.TryGetComponent(out AssignItemID id))
                {
                    Debug.Log("見つかった");
                    //AssignItemIDを無効化（有効だと敵が食べてしまう）
                    id.enabled = false;
                }
            }

        }
        else
        {
            Debug.Log("ターゲットが存在しません");

            m_itemObj = Instantiate(prefab);
        }

    }

    public void DestroyPrefab()
    {
        if(m_itemObj)
        {
            Destroy(m_itemObj);
        }
    }

    private void OnDestroy()
    {
        //アイテムを破棄
        if(m_itemObj)
        {
            Destroy(m_itemObj);
        }
    }


}
