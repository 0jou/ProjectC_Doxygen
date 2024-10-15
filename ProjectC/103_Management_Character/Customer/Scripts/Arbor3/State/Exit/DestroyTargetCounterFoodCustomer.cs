/*!
 * @file DestroyTargetCounterFoodCustomer.cs
 * @brief ターゲットにしていた料理のインスタンスを削除する
 * @author 田内
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

/// <summary>
/// @brief ターゲットにしていた料理のインスタンスを削除する
/// </summary>
[AddComponentMenu("Customer/DestroyTargetCounterFoodCustomer")]
public class DestroyTargetCounterFoodCustomer : BaseCustomerStateBehaviour
{
    //! @brief ターゲットの料理インスタンスを削除する
    private void DestroyTargetFood()
    {
		// ターゲットの料理を削除する

		var data = GetCustomerData();
		if (data == null) return;

		var targetFood = data.TargetOrderFoodData;
		if (targetFood == null) return;

		Destroy(targetFood.gameObject);

    }

	// Use this for enter state
	public override void OnStateBegin()
	{
		DestroyTargetFood();
	}

}
