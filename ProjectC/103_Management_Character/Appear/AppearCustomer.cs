using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearCustomer : AppearObject
{
    // 制作者 田内
    // 客を出現させる


    //================================
    //          実行処理
    //================================

    protected override void UpdateCreate()
    {
        // 料理を提供できなければ
        if (!CounterManager.instance.IsOrder()) return;

        // 待ち列に客が入れなければ
        if (!QueueManager.instance.IsInQueue()) return;

        // オブジェクト作成
        var obj = CreateObject();
        if (obj == null) return;


        // キャラクターモーターを更新する
        var core = obj.GetComponent<MyCharacterController>();
        if (core == null)
        {
            Debug.LogError("キャラクターコントローラーが存在しません");
            return;
        }

        // モーターの座標を更新
        core.SetPositionMotor(transform.position);

    }

}
