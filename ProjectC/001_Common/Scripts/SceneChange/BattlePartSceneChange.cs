using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BattlePartSceneChange : MonoBehaviour
{
    //バトルパート→経営パート用の仮シーンチェンジ（山本）
    [SerializeField] private SceneTransitionManager m_sceneTransitionManager = null;
    [SerializeField] PlayerInput m_input = null;

    private InputActionMap m_inputActionMap;

    private bool m_sceneChangeFlg = false;

    public bool SceneChangeFlg { get { return m_sceneChangeFlg; }set { m_sceneChangeFlg = value; } }

    private void Start()
    {
        if (!m_input)
        {
            return;
        }
        m_inputActionMap = m_input.actions.FindActionMap("Debug");

        if (m_inputActionMap == null)
        {
            Debug.LogError("アクションマップにDebugが存在しません。");
        }

        m_sceneChangeFlg = false;
    }

    private void Update()
    {
        if (m_inputActionMap.FindAction("SceneChange").triggered || m_sceneChangeFlg)
        {
            m_sceneTransitionManager?.SceneChange();

            //SceneManager.LoadScene("Management");

        }
        
    }


}
