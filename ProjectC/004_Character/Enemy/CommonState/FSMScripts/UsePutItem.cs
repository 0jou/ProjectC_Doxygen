/**
* @file UsePutItem.cs
* @brief 敵が料理を食べる処理
*/
using UnityEngine;
using Arbor;

[AddComponentMenu("")]
/**
* @brief 敵が料理を食べる処理　伊波
* @details 料理の状態異常効果をここで追加する
*/
public class UsePutItem : StateBehaviour
{
    [SerializeField] private StateLink m_stateLink = new ();
    [SerializeField] private FlexibleTransform m_targetTrans;

    //// Use this for initialization
    //void Start()
    //{

    //}

    //// Use this for awake state
    //public override void OnStateAwake()
    //{
    //}

    // Use this for enter state
    public override void OnStateBegin()
    {
        if (m_targetTrans.value == null)
        {
            Transition(m_stateLink);
            return;
        }

        CreateConditionObject();
        Destroy(m_targetTrans.value.gameObject);
        Transition(m_stateLink);
    }

    //// Use this for exit state
    //public override void OnStateEnd()
    //{
    //}

    //// OnStateUpdate is called once per frame
    //public override void OnStateUpdate()
    //{
    //}

    //// OnStateLateUpdate is called once per frame, after Update has finished.
    //public override void OnStateLateUpdate()
    //{
    //}

    private void CreateConditionObject()
    {
        if (m_targetTrans.value == null) return;
        if (!m_targetTrans.value.TryGetComponent(out AssignItemID itemID)) return;

        var data = ItemDataBaseManager.instance.GetItemData<FoodData>(itemID.ItemTypeID, itemID.ItemID);
        if (data == null) return;

        var conditionData = ConditionDataBaseManager.instance.GetConditionData(data.ConditionID);
        if (conditionData == null) return;

        if (conditionData.ConditionPrefab == null) return;
        Transform managerObj = transform.root.Find("ConditionManager");
        if (!managerObj.TryGetComponent(out ConditionManager manager)) return;

        manager.AddCondition(conditionData.ConditionPrefab);
        return;
    }
}
