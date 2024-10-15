using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.EventSystems;

public partial class SelectUIController : MonoBehaviour
{
    // 制作者 田内
    // UIを選択する処理


    // 既存のスロットを選択する
    private void SelectUI()
    {
        if (Keyboard.current?.anyKey.wasPressedThisFrame == false && Gamepad.current?.wasUpdatedThisFrame == false) return;


        if (m_uiList.Count <= 0)
        {
            return;
        }

        bool flg = false;

        // 左に進む
        if (PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.UI, "Left"))
        {
            flg = Left();
        }


        // 右に進む
        if (PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.UI, "Right"))
        {
            flg = Right();
        }


        // 上に進む
        if (PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.UI, "Up"))
        {
            flg = Up();
        }


        // 下に進む
        if (PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.UI, "Down"))
        {
            flg = Down();
        }

        if (flg)
        {
            m_currentSelectUIData = m_uiList[m_currentHeight].List[m_currentWidth];
            m_isSelectChangeFlg = true;

            PlaySelectSound();

        }
    }


    protected bool Left()
    {
        bool flg = true;

        // 左へ移動
        m_currentWidth--;

        if (!IsWidthBelow())
        {
            if (m_isLoop)
            {

                m_currentWidth = m_uiList[m_currentHeight].List.Count - 1;

            }
            else
            {

                m_currentWidth = 0;

                flg = false;

            }

        }

        return flg;
    }


    private bool Up()
    {
        bool flg = true;

        // 上へ移動
        m_currentHeight--;

        // 下回れば
        if (!IsHeightBelow())
        {
            // ループするのであれば
            if (m_isLoop)
            {

                m_currentHeight = m_uiList.Count - 1;
            }
            else
            {

                Down();

                flg = false;
            }
        }

        // 存在しなければ
        if (!IsWidthExceed())
        {
            switch (m_interpolationType)
            {

                // 超えて移動
                case SelectUIInterpolation.Exceed:
                    {

                        Up();

                        break;

                    }

                // 補間して移動
                case SelectUIInterpolation.Interpolation:
                    {

                        m_currentWidth = m_uiList[m_currentHeight].List.Count - 1;

                        break;

                    }
            }
        }

        return flg;

    }

    protected bool Right()
    {

        bool flg = true;

        // 左へ移動
        m_currentWidth++;

        // 上回ってぃれば
        if (!IsWidthExceed())
        {
            // ループする
            if (m_isLoop)
            {

                m_currentWidth = 0;

            }
            else
            {

                m_currentWidth = m_uiList[m_currentHeight].List.Count - 1;

                flg = false;

            }
        }

        return flg;
    }


    private bool Down()
    {
        bool flg = true;

        // 下へ移動
        m_currentHeight++;

        // 上回っていれば
        if (!IsHeightExceed())
        {
            // ループする
            if (m_isLoop)
            {

                m_currentHeight = 0;

            }
            else
            {
                Up();

                flg = false;
            }

        }


        // 上回っていれば
        if (!IsWidthExceed())
        {
            switch (m_interpolationType)
            {

                // 超えて移動
                case SelectUIInterpolation.Exceed:
                    {
                        Down();
                        break;
                    }

                // 補間して移動
                case SelectUIInterpolation.Interpolation:
                    {
                        m_currentWidth = m_uiList[m_currentHeight].List.Count - 1;
                        break;
                    }
            }
        }

        return flg;

    }


    //==================================================================================================================

    private bool IsWidthExceed()
    {
        if (m_uiList[m_currentHeight].List.Count - 1 < m_currentWidth)
        {
            return false;
        }

        return true;
    }

    private bool IsWidthBelow()
    {
        if (m_currentWidth < 0)
        {
            return false;
        }

        return true;
    }

    private bool IsHeightExceed()
    {
        if (m_uiList.Count - 1 < m_currentHeight)
        {
            return false;
        }

        return true;
    }

    private bool IsHeightBelow()
    {
        if (m_currentHeight < 0)
        {
            return false;
        }

        return true;
    }


}
