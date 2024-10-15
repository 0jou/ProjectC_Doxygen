using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// タイトル画面のメインメニューのコントローラー(吉田)
/// </summary>
public class TitleMainMenuController : MonoBehaviour
{

    [Header("選択中UI")]
    [SerializeField]
    private GameObject m_selectUI = null;

    [SerializeField]
    private List<TitleMainMenuButton> m_buttons = new();

    private int m_currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (m_selectUI == null)
        {
            Debug.LogError("選択中UIがInspecterで設定されていません。");
        }
        else
        {
            m_selectUI.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputUpdate();

        // ボタンの更新
        for (int i = 0; i < m_buttons.Count; i++)
        {
            if (i == m_currentIndex)
            {
                m_buttons[i].IsSelectedUpdate();
            }
            else
            {
                m_buttons[i].IsDeselectedUpdate();
            }
        }

    }

    private void InputUpdate()
    {
        if (Keyboard.current?.anyKey.wasPressedThisFrame == false && Gamepad.current?.wasUpdatedThisFrame == false) return;

        // 決定したら早期リターン
        if (PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.UI, "Select"))
        {
            m_buttons[m_currentIndex].OnClick();
            return;
        }

        int buttonMax = (m_buttons.Count);

        // WASDなどで選択
        bool switchFlg = false;

        if (PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.UI, "Down"))
        {
            m_currentIndex++;
            if (m_currentIndex >= buttonMax)
            {
                m_currentIndex = 0;
            }
            switchFlg = true;
        }

        if (PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.UI, "Up"))
        {
            m_currentIndex--;
            if (m_currentIndex < 0)
            {
                m_currentIndex = buttonMax - 1;
            }
            switchFlg = true;
        }

        if (switchFlg == false) return;

        if (m_selectUI)
        {
            // 選択中のUIを移動
            m_selectUI.SetActive(true);
            Vector3 selectUIPos = m_selectUI.transform.position;
            selectUIPos.y = m_buttons[m_currentIndex].transform.position.y;
            m_selectUI.transform.position = selectUIPos;
        }

    }

    private void OnEnable()
    {
        m_buttons[m_currentIndex].IsSelectedUpdate();
        m_selectUI.SetActive(true);
    }

    private void OnDisable()
    {
        m_currentIndex = 0;
        m_selectUI.SetActive(false);
    }
}
