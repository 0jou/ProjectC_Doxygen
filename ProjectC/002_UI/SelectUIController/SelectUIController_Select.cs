using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SelectUIInfo;

namespace SelectUIInfo
{
    // UIの選択種類列挙型
    public enum SelectUIType
    {
        Press,
        Hold,
    }
}


public partial class SelectUIController : MonoBehaviour
{
    // 制作者 田内
    // UI選択の処理

    private string m_pressInputAction = "Select";

    public string PressInputAction
    {
        get { return m_pressInputAction; }
    }

    private string m_holdInputAction = "HoldSelect";

    public string HoldInputAction
    {
        get { return m_holdInputAction; }
    }


    //==========================================
    //                 実行処理
    //==========================================

    // 選択した場合
    private void OnUpdateInput()
    {

        try
        {
            if (m_currentSelectUIData == null) return;

            SelectUIType type = m_currentSelectUIData.SelectUIType;
            switch (type)
            {
                // 単押し
                case SelectUIType.Press:
                    {
                        if (PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.UI, m_pressInputAction) == true)
                        {
                            PressUpdate();
                        }
                        break;
                    }
                // 長押し
                case SelectUIType.Hold:
                    {
                        if (PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.UI, m_holdInputAction) == true)
                        {
                            PressUpdate();
                        }
                        break;
                    }
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    private void PressUpdate()
    {
        PlayPressSound();

        // サイズを変更
        DoScale();

        m_isPress = true;
    }

}
