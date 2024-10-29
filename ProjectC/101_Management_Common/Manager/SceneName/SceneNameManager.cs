using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SceneNameManager : BaseManager<SceneNameManager>
{
    // シーンの名前を管理するマネージャー
    // 制作者　田内

    //====================================================
    // 現在のシーン名

    private string m_currentSceneName = "None";

    public string CurrentSceneName
    {
        get { return m_currentSceneName; }
    }

    //====================================================
    // 前回のシーン名
    private string m_beforeSceneName = "None";

    public string BeforeSceneName
    {
        get { return m_beforeSceneName; }
    }

    //=======================================================================

    private void Start()
    {
        // 最初は現在のシーンのみ保持
        m_currentSceneName = SceneManager.GetActiveScene().name;
    }

    // シーンの名前を保持する
    public void ChangeSceneName(string _sceneName)
    {
        // 以前のシーン名を保持
        m_beforeSceneName = m_currentSceneName;

        // 更新されたシーン名を保持
        m_currentSceneName = _sceneName;
    }

}
