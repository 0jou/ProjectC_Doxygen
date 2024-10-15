using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/PayMoneyCustomer")]
[AddComponentMenu("")]
public class PayMoneyCustomer : BaseCustomerStateBehaviour
{

    // 頼んだ料理の金額を払う
    // 制作者　田内


    //===================================================================================================================

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



    public override void OnStateBegin()
    {
        Pay();
    }

}
