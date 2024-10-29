using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SelectUIController;

/// <summary>
/// 石が投げられた状態・宙に浮いている状態
/// </summary>

public class ThrowItemState : MonoBehaviour
{

    private Rigidbody m_rigidbody = null;
    public Rigidbody Rigidbody { set { m_rigidbody = value; } }

    private Collider m_collider = null;
    public Collider Collider { set { m_collider = value; } }


    // Start is called before the first frame update
    void Start()
    {
        if (m_rigidbody == null)
        {
            m_rigidbody = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_rigidbody == null) return;

        if (m_rigidbody.IsSleeping())
        {
            //m_collider.isTrigger = true;
            //m_collider.excludeLayers = 0;
            //m_collider.gameObject.layer = LayerMask.NameToLayer("Food"); ;
            //Destroy(m_rigidbody);
            //Destroy(this);
            Destroy(transform.root.gameObject);
        }
    }

    // 投げたものがが敵に当たった時
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            AddCondition(collision.gameObject,collision);

            // エフェクト？

            // 破壊
            //Destroy(gameObject);
            Destroy(transform.root.gameObject);
        }
    }

    // フィールドに効果発生
    private void AddCondition(GameObject _enemy, Collision _collision)
    {
        if (_enemy == null) return;

        var itemID = this.GetComponentInChildren<AssignItemID>();
        if (itemID == null) return;

        var data = ItemDataBaseManager.instance.GetItemData<FoodData>(itemID.ItemTypeID, itemID.ItemID);
        if (data == null) return;

        var conditionData = ConditionDataBaseManager.instance.GetConditionData(data.ConditionID);
        if (conditionData == null) return;

        //if (conditionData.ConditionPrefab == null) return;
        if (conditionData.ThrowConditionPrefab == null) return;

        GameObject throwCondition =
            Instantiate(conditionData.ThrowConditionPrefab, _collision.contacts[0].point, Quaternion.identity);

        //範囲を設定
        throwCondition.transform.localScale = 
            new Vector3(data.ThrowRange, data.ThrowRange, data.ThrowRange);

        //Transform managerObj = _enemy.transform.Find("ConditionManager");
        //if (!managerObj.TryGetComponent(out ConditionManager manager)) return;

        //manager.AddCondition(conditionData.ConditionPrefab);

    }
}
