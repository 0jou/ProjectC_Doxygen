using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class WaitEatCustomer : BaseCustomerStateBehaviour
{
    // 料理を食べきるまで待機
    // 制作者　田内

    //==================================
    // ステートリンク
    [SerializeField]
    private StateLink m_nextState = null;


    private void Wait()
    {
        var data = GetCustomerData();
        if (data == null) return;

        // 怒ったら
        if (data.CountEat())
        {
            data.EatCount = 0;
            SetTransition(m_nextState);
        }
    }


    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        Wait();
    }

}
