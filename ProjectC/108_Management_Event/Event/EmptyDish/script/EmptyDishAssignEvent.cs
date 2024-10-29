/* @file CleaningAssignEvent.cs
 * @brief 任意のTagのコライダーが接触した際に行われる空皿自体の挙動を制御するスクリプト
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// @brief 汚れ自体の接触時の挙動を制御するスクリプト
/// </summary>
public class EmptyDishAssignEvent : BaseAssignEventObject
{

    //! @brief 接触時のイベント 汚れを強調表示する
    protected override void OnCollisionTriggerEvent()
    {
    }

    //! @brief 接触終了時のイベント 汚れの強調表示を解除する
    protected override void OnCollisionTriggerExitEvent()
    {
    }

    //! @brief アクセスされたかどうかの定義 Gathering(Eキー)を押すとアクセスされる
    public override bool IsAccessed(ref IInputProvider input)
    {
        return input.Gathering;
    }

    //! @brief アクセス時のイベント 汚れを消す
    public override void OnCollisionAccesesEvent()
    {
        Destroy(gameObject);
    }



    private void Start()
    {
        base.Start();
    }

    private void OnDestroy()
    {
    }
}
