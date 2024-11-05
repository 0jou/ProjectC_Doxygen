using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerInputManager : BaseManager<PlayerInputManager>
{
    // 制作者 田内
    // PlayerInputActionの動作を処理する

    // 保存用
    private float m_keepTimeScale = 1.0f;


    //================================================
    //              実行処理
    //================================================

    private void SetActivePlayerActionMap()
    {
        // 変更が加われば
        if (Time.timeScale != m_keepTimeScale)
        {
            // 基が停止していれば
            if (m_keepTimeScale == 0.0f && Time.timeScale != 0.0f)
            {
                GetInputActionMap(InputActionMapTypes.Player).Enable();
            }

            // 保存
            m_keepTimeScale = Time.timeScale;


            // 停止していれば
            if (Time.timeScale == 0.0f)
            {
                GetInputActionMap(InputActionMapTypes.Player).Disable();
            }
        }
    }
}
