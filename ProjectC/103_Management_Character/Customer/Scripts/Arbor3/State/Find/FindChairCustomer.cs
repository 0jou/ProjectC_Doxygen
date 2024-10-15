using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/FindChairCustomer")]
[AddComponentMenu("")]
public class FindChairCustomer : BaseCustomerStateBehaviour
{

    // 椅子を探すステート
    // 制作者　田内

    private enum FindChairType
    {
        Once,   // 一度だけ確認する
        Always  // 常に確認する
    }



    [Header("確認タイプ")]
    [SerializeField]
    private FindChairType m_type = FindChairType.Once;


    //===========================================================
    // ステート

    [SerializeField]
    private StateLink m_successLink = null;

    [SerializeField]
    private StateLink m_failLink = null;

    //==========================================================


    // 椅子を探す(1度のみ)
    void FindChairOnce()
    {
        if (m_type != FindChairType.Once) return;

        // 客データ
        var data = GetCustomerData();
        if (data == null) return;

        // 既に所持していれば
        if(data.TargetTableSetData!=null)
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
        var tableSetController = TableSetManager.instance;
        if (tableSetController == null) return;

        // 空席を取得
        var table = tableSetController.GetVacantTableSet();
        if (table != null)
        {
            // 座るオブジェクトに自身をセット
            table.SitObject = data.gameObject;

            // ターゲットのテーブルセットをセット
            data.TargetTableSetData = table;

            // 成功ステートへ移動
            SetTransition(m_successLink);

            return;
        }

        // Todo:椅子が無かった場合ここに来る

        // 失敗ステートへ移動
        SetTransition(m_failLink);

    }

    // 椅子を探す(常に)
    void FindChairAlways()
    {
        if (m_type != FindChairType.Always) return;

        // 客データ
        var data = GetCustomerData();
        if (data == null) return;


        // すでにターゲットの椅子があれば
        if (data.TargetTableSetData != null) return;


        // 待機列データが存在すれば
        var queueData = data.QueueData;
        if (queueData != null)
        {
            // 自身が先頭じゃなければ確認しない
            if (queueData.HeadNumber > 0)
            {
                return;
            }
        }


        // テーブルセットのコントローラー
        var tableSetController = TableSetManager.instance;
        if (tableSetController == null) return;

        // 空席を取得
        var table = tableSetController.GetVacantTableSet();
        if (table != null)
        {
            // 座るオブジェクトに自身をセット
            table.SitObject = data.gameObject;

            // ターゲットのテーブルセットをセット
            data.TargetTableSetData = table;

            // 成功ステートへ移動
            SetTransition(m_successLink);

            return;
        }

    }




    // Use this for initialization
    void Start()
    {

    }

    // Use this for awake state
    public override void OnStateAwake()
    {
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
        FindChairOnce();
    }

    // Use this for exit state
    public override void OnStateEnd()
    {
    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        // 椅子を探す
        FindChairAlways();
    }

    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
    }



}
