/*!
 * @file GameObjectRegistry.cs
 * @brief GameObjectを文字キーで管理し外部から取得しやすくするクラス
 * @author 上甲
 */

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// @brief GameObjectを文字キーで管理し外部から取得しやすくするクラス
/// @details エディタから最初に設定されることを想定しているため、動的に追加は現在不可
/// 基本的に一番親の階層に配置して使用することを推奨
/// </summary>
public class GameObjectRegistry : MonoBehaviour
{
    // GameObjectを文字キーで管理するDictionary
    private Dictionary<string, GameObject> m_objectDictionary = new Dictionary<string, GameObject>();

    // 登録したいオブジェクトのリスト
    [System.Serializable]
    public class GameObjectEntry
    {
        public string key;        // 文字キー
        public GameObject obj;    // 対応するGameObject
    }

    [Header("登録したいオブジェクトのリスト")]
    public List<GameObjectEntry> objectsToRegister = new List<GameObjectEntry>();

    private void Awake()
    {
        // リストからDictionaryに登録
        foreach (var entry in objectsToRegister)
        {
            if (!m_objectDictionary.ContainsKey(entry.key))
            {
                m_objectDictionary.Add(entry.key, entry.obj);
            }
        }
    }

    //! @brief 文字キーでGameObjectを取得する
    public GameObject GetObjectByKey(string key)
    {
        GameObject obj;
        if (m_objectDictionary.TryGetValue(key, out obj))
        {
            return obj;
        }
        return null;
    }
}
