using Cysharp.Threading.Tasks;
using MackySoft.Navigathena.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// @file   SceneTransitionManager
/// @brief シーン遷移を管理するクラス 遷移する瞬間にEventSystemを無効化する
/// </summary>
public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] public string m_sceneName; // プロパティからフィールドに変更

    [ContextMenu("SceneChange")]
    private UniTask Method()
    {
        return SceneChange();
    }

    async public UniTask SceneChange()
    {
        await SceneChange(m_sceneName);

    }

    async public UniTask SceneChange(string sceneName)
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.enabled = false;
        }

        ISceneIdentifier identifier = new BuiltInSceneIdentifier(sceneName);

        // シーン名を保存
        SceneNameManager.instance.ChangeSceneName(sceneName);

        // ロード画面を挟んでシーン遷移
        await GlobalSceneNavigator.Instance.Change(identifier, new NextPageTransitionDirector(new BuiltInSceneIdentifier("Loading")));
    }
}