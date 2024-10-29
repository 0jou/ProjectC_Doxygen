using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManagementInputActionButton : InputActionButton
{
    // 制作者 田内
    // 経営シーンへ移動するボタン

    [Header("経営提供料理コントローラー")]
    [SerializeField]
    private SelectManagementProvideFoodController m_selectManagementProvideFoodController;

    override protected bool IsPress()
    {
        if(m_selectManagementProvideFoodController==null)
        {
            Debug.LogError("SelectManagementProvideFoodControllerがシリアライズされていません");
            return false;
        }

        // 提供する料理が1つ以上存在すれば
        if (0<m_selectManagementProvideFoodController.ProvideFoodIDList.Count)
        {
            return true;
        }

        return false;
    }

}
