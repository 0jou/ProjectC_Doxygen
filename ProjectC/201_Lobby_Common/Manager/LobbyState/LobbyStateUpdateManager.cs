using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using LobbyStateInfo;

namespace LobbyStateInfo
{
    public enum LobbyState
    {
        Normal,　       // 通常の状態
        ReturnAction,   // アクションパートから帰ってきた状態
        TrialSession,   // 体験会用、タイトルに戻る


        GoToActionTutorial = 100,         // アクションに行くのを促すチュートリアル
        GoToManagementTutorial = 101,     // 経営に行くのを促すチュートリアル
    }
}

public class LobbyStateUpdateManager : BaseGameStateUpdateController
{
    // 経営パートの進行を管理するマネージャークラス
    // 制作者　田内


    #region シングルトン
    public static LobbyStateUpdateManager instance;

    [Header("シーン変更後も削除しない")]
    [SerializeField]
    bool m_dontDestroyOnLoad = true;

    protected virtual void Awake()
    {
        // インスタンスがなければ作成
        if (instance == null)
        {
            instance = (LobbyStateUpdateManager)FindObjectOfType(typeof(LobbyStateUpdateManager));
            if (m_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        // あれば作成しない
        else
        {
            Destroy(gameObject);
        }
    }

    public void DeleteInstance()
    {
        if (instance != null)
        {
            Destroy(instance);
            instance = null;
        }
    }
    #endregion



    [Header("デバッグ用")]
    [SerializeField]
    private LobbyState m_lobbyState = LobbyState.Normal;


    [System.Serializable]
    private class InitilizeState
    {
        [Header("シーン名")]
        [SerializeField]
        private string m_sceneName = "";

        public string SceneName
        {
            get { return m_sceneName; }
        }

        [Header("変更ステート")]
        [SerializeField]
        private LobbyState m_lobbyState = LobbyState.Normal;

        public LobbyState LobbyState
        {
            get { return m_lobbyState; }
        }

    }

    [Header("初期ステートリスト")]
    [SerializeField]
    private List<InitilizeState> m_initilizeStateList = new();

    [Header("当てはまらなかった時の初期ステート")]
    [SerializeField]
    private LobbyState m_initilizeState = LobbyState.Normal;


    //=================================================================
    //                      実行処理
    //=================================================================


    // 初期ステートをセット
    override protected void SetInitializeState()
    {
        // どのシーンからきたか
        var beforeSceneName = SceneNameManager.instance.BeforeSceneName;
        if (beforeSceneName == "None")
        {
            // 初期の値をセット
            m_currentState = (int)m_lobbyState;
        }
        else
        {
            // 一致するシーン名を取得
            foreach (var state in m_initilizeStateList)
            {
                if (beforeSceneName == state.SceneName)
                {
                    // 更新
                    m_currentState = (int)state.LobbyState;
                    return;
                }
            }

            // 何もなければ
            m_currentState = (int)m_initilizeState;
        }

    }

}
