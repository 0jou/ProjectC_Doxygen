using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using UniRx.Triggers;
using Arbor.Calculators;

public class ThrowItemInstance : MonoBehaviour
{

    // 制作者 吉田　参考元 PutItemInstance.cs

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

    private Vector3 m_throwPower = new Vector3(0, 10, 0);
    public Vector3 ThrowPower { set { m_throwPower = value; } }

    private Vector3 m_throwTorque = new Vector3(0.3f, 0, 0.3f);
    public Vector3 ThrowTorque { set { m_throwTorque = value; } }

    private Rigidbody m_rigidbody = null;

    private Collider m_collider = null;

    private bool m_onece = true;

    private Transform m_handTrans = null;
    public void SetHandTrans(Transform _handTrans)
    {
        m_handTrans = _handTrans;
    }

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
    // ターゲットの位置からアイテムを投げる
    public void Throw()
    {
        if (m_onece == true)
        {
            if (m_rigidbody != null) return;
        }

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

        GameObject objct = null;

        if (m_handTrans)
        {
            var trans = m_handTrans;

            // プレハブを作成
            objct = Instantiate(prefab, trans.position, trans.rotation);
        }
        else
        {
            Debug.Log("ターゲットが存在しません");

            objct = Instantiate(prefab);
        }

        m_collider = objct.transform.GetComponentInChildren<Collider>();
        if (m_collider == null)
        {
            Debug.LogError("m_colliderが存在しません");
            return;
        }

        m_collider.gameObject.layer = LayerMask.NameToLayer("ThrowingFood");

        // コライダーの設定
        m_collider.isTrigger = false;
        m_collider.excludeLayers = LayerMask.GetMask("Player");

        // 作成したアイテムにRigidbodyを追加し、投げる
        m_rigidbody = objct.AddComponent<Rigidbody>();
        m_rigidbody.excludeLayers = LayerMask.GetMask("Player");
        m_rigidbody.AddForce(m_throwPower * m_rigidbody.mass, ForceMode.Impulse);
        m_rigidbody.AddTorque(new Vector3(0.3f, 0, 0.3f), ForceMode.Impulse);

        // 追加　上甲 効果音
        SoundManager.Instance.Start3DPlayback("ThrowItem", m_rigidbody.gameObject);
        //SoundManager.Instance.StartPlayback("ThrowItem");

        // 投げられている状態のコンポーネントを追加
        ThrowItemState state = objct.AddComponent<ThrowItemState>();
        state.Rigidbody = m_rigidbody;
        state.Collider = m_collider;

        // 作成したアイテムを手持ちから削除する
        InventoryManager.instance.RemoveItem(data.ItemTypeID, data.ItemID);

    }

}
