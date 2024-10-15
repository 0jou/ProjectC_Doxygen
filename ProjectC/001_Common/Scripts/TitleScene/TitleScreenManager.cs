using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using nTools.PrefabPainter;


public class TitleScreenManager : MonoBehaviour
{

    public CanvasGroup m_titleLogo;
    public CanvasGroup m_pushAnyKey;
    public GameObject m_mainMenuPanel;
    public GameObject m_optionObject;

    private bool m_isAnyKeyPressed = false;

    private readonly InputAction m_pressAnyKeyAction =
                    new InputAction(type: InputActionType.PassThrough, binding: "*/<Button>", interactions: "Press");


    [SerializeField] SceneTransitionManager m_sceneChanger = null;

    private void OnEnable() => m_pressAnyKeyAction.Enable();
    private void OnDisable() => m_pressAnyKeyAction.Disable();
    // Start is called before the first frame update
    public void StartUp()
    {
        Debug.Log("StartUp");
        m_titleLogo.gameObject.SetActive(true);
        m_pushAnyKey.gameObject.SetActive(false);
        m_mainMenuPanel.SetActive(false);
        m_optionObject.SetActive(false);
        m_isAnyKeyPressed = false;

        _ = ShowTitleLogo();
    }

    async UniTask ShowTitleLogo()
    {
        if(m_titleLogo == null) { return; }
        await DOTween.To(() => m_titleLogo.alpha, x => m_titleLogo.alpha = x, 1, 1.0f);
        m_pushAnyKey.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // 見にくかったので、過去のものコメントして修正してます。（吉田）

        /*// 20240822以前 -------
        if (!m_isAnyKeyPressed && m_pressAnyKeyAction.triggered)
        {
            Debug.Log("Any Key Pressed");
            m_isAnyKeyPressed = true;
            m_pushAnyKey.gameObject.SetActive(false);
            if (m_nextActivateButton)
            {
                m_nextActivateButton.ActivateOrNotActivate(true);
            }
            m_mainMenuPanel.SetActive(true);
            //無理くりシーン移動（山本）
            // _ = m_sceneChanger.SceneChange();
        }
        */// -------

        if (!m_isAnyKeyPressed)
        {
            UpdateForWaitAnyKey();
            return;
        }

        // 以下 Push Any Key が非表示のときの処理



    }

    public void OnNewGame()
    {
        // New Game ボタンが押されたときの処理
        // 例えば、新しいシーンに遷移する
        if (m_sceneChanger != null)
        {
            _ = m_sceneChanger.SceneChange();
        }
    }
    public void OnLoadGame()
    {
        // Load Game ボタンが押されたときの処理
        // セーブデータのロードなど
        if (m_sceneChanger != null)
        {
            _ = m_sceneChanger.SceneChange();
        }

    }

    public void OnOption()
    {
        // Option ボタンが押されたときの処理
        // オプション画面の表示など
        m_optionObject.SetActive(true);
    }

    public void OnExit()
    {
        // Exit ボタンが押されたときの処理
        Application.Quit();
    }

    /// <summary>
    /// キーを押すまで の更新処理
    ///  pushAnyKeyが表示されているとき
    /// </summary>
    private void UpdateForWaitAnyKey()
    {
        if (m_pressAnyKeyAction == null) return;
        if (m_pushAnyKey == null) return;
        if (m_mainMenuPanel == null) return;

        if (m_isAnyKeyPressed) return;

        if (m_pressAnyKeyAction.triggered)
        {
            Debug.Log("Any Key Pressed");
            m_isAnyKeyPressed = true;

            // 表示切り替え
            m_pushAnyKey.gameObject.SetActive(false);
            m_mainMenuPanel.SetActive(true);


            //無理くりシーン移動（山本）
            // _ = m_sceneChanger.SceneChange();
        }
    }
}
