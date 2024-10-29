/*!
 * @file PayMoneyCustomer.cs
 * @brief 頼んだ料理の金額を払うステート
 * @author 田内
 * @date 10/16
 *        食い逃げ時にお金を払う処理を追加 上甲
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/PayMoneyCustomer")]
[AddComponentMenu("")]
public class PayMoneyCustomer : BaseCustomerStateBehaviour
{
    //! @brief 食い逃げ失敗時に殴られるたびお金を払う金額
    [SerializeField] FlexibleInt m_flexiblePayBack = new(10);
    // 制作者　田内

    //! @brief 通常支払いか食い逃げか
    [Header("true 通常支払い false 食い逃げ時支払い")]
    [SerializeField] bool m_isNormalPay = true;


    //===================================================================================================================

    //! @brief お金を払う処理
    private void Pay()
    {
        var data = GetCustomerData();
        if (data == null) return;

        var targetFood = data.TargetOrderFoodData;
        if (targetFood == null) return;

        var foodData = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemInfo.ItemTypeID.Food, (uint)targetFood.FoodID);
        if (foodData == null) return;

        // お金を払う
        ManagementGameManager.instance.AddEarnedMoney(foodData.Price);

        // 提供した数を足す
        ManagementProvideFoodManager.instance.SoldFood(targetFood.FoodID);

    }

    //! @brief 食い逃げ失敗時にお金を払う処理
    private void PayBack()
    {
        ManagementGameManager.instance.AddEarnedMoney((uint)m_flexiblePayBack.value);
    }



    public override void OnStateBegin()
    {
        if (m_isNormalPay)
        {
            Pay();
        }
        else
        {
            PayBack();
        }
    }

}
