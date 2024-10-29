using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/QueueFindChairCustomer")]
[AddComponentMenu("")]
public class QueueFindChairCustomer : BaseCustomerStateBehaviour
{
    // 制作者 田内
    // 待機列で椅子を探す

    [SerializeField]
    private StateLink m_successLink = null;

    //===================================
    //  実行処理
    //===================================

    private void FindChair()
    {
        // 客データ
        var data = GetCustomerData();
        if (data == null) return;

        // 既に所持していれば
        if (data.TargetTableSetData != null)
        {
            // 成功ステートへ移動
            SetTransition(m_successLink);
        }

        // 待機列データが存在すれば
        var queueData = data.QueueData;
        if (queueData == null) return;

        // 自身が先頭じゃなければ確認しない
        if (queueData.HeadNumber != 0) return;

        // 空席を取得
        var table = TableSetManager.instance.GetVacantTableSet();
        if (table != null)
        {
            // 座るオブジェクトに自身をセット
            table.SitObject = data.gameObject;

            // ターゲットのテーブルセットをセット
            data.TargetTableSetData = table;

            // 成功ステートへ移動
            SetTransition(m_successLink);
        }

    }


    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        FindChair();
    }

}
