using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ManagementStateUpdateInfo;
namespace ManagementStateUpdateInfo
{
    public enum ManagementState
    {
        Start = 0,　  // 通常の状態
        Update = 1,   // アクションパートから帰ってきた状態
        End = 2,      // 体験会用、タイトルに戻る
        Result = 3,   // リザルト表示

        Standby = 4,     // ゲーム開始時の待機状態

        None = -1,    // 実行終了
    }

}

public class ManagementStateUpdateManager : BaseGameStateUpdateController
{
    #region シングルトン
    public static ManagementStateUpdateManager instance;

    [Header("シーン変更後も削除しない")]
    [SerializeField]
    bool m_dontDestroyOnLoad = true;

    protected virtual void Awake()
    {
        // インスタンスがなければ作成
        if (instance == null)
        {
            instance = (ManagementStateUpdateManager)FindObjectOfType(typeof(ManagementStateUpdateManager));
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

    // 経営パートの進行を管理するマネージャークラス
    // 制作者　田内

    [Header("デバッグ用")]
    [SerializeField]
    private ManagementState m_managementState = ManagementState.Start;


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
        private ManagementState m_managementState = ManagementState.None;

        public ManagementState ManagementState
        {
            get { return m_managementState; }
        }

    }

    [Header("初期ステートリスト")]
    [SerializeField]
    private List<InitilizeState> m_initilizeStateList = new();

    [Header("当てはまらなかった時の初期ステート")]
    [SerializeField]
    private ManagementState m_initilizeState = ManagementState.Standby;


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
            m_currentState = (int)m_managementState;
        }
        else
        {
            // 一致するシーン名を取得
            foreach (var state in m_initilizeStateList)
            {
                if (beforeSceneName == state.SceneName)
                {
                    // 更新
                    m_currentState = (int)state.ManagementState;
                    return;
                }
            }

            // 何もなければ
            m_currentState = (int)m_initilizeState;
        }

    }

}
