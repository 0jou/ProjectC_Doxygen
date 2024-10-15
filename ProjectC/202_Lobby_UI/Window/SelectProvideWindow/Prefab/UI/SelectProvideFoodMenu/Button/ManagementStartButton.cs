using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementStartButton : ButtonData
{
    // 制作者 田内
    // 経営を開始するボタン

    //===================================
    //          実行処理
    //===================================


    void Update()
    {
        Check();
    }


    private void Check()
    {
        // 提供料理が何かしらセットされていれば
        if (ManagementProvideFoodManager.instance.IsSetProvideFood())
        {
            m_isUse = true;
        }
        else
        {
            m_isUse = false;
        }
    }

}
