using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/StartFindChairCustomer")]
[AddComponentMenu("")]
public class StartFindChairCustomer : BaseCustomerStateBehaviour
{

    // 椅子を探すステート
    // 制作者　田内

    //===========================================================
    // ステート

    [SerializeField]
    private StateLink m_successLink = null;

    [SerializeField]
    private StateLink m_failLink = null;

    //==========================================================


    // 椅子を探す(1度のみ)
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

        // 待機列コントローラー
        var queueController = QueueManager.instance;
        if (queueController == null) return;

        // 待機列ができていれば
        if (queueController.IsQueue())
        {
            // 失敗ステートへ移動
            SetTransition(m_failLink);
            return;
        }


        // テーブルセットのコントローラー
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
        else
        {
            // Todo:椅子が無かった場合ここに来る
            // 失敗ステートへ移動
            SetTransition(m_failLink);
        }
    }



    // Use this for enter state
    public override void OnStateBegin()
    {
        FindChair();
    }

}
