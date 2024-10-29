/*!
 * @file GetGameObjectByKey.cs
 * @brief GameObjectRegistry から指定のキーで GameObject を取得するスクリプト
 * @author 上甲
 */

using UnityEngine;
using Arbor;

/// <summary>
/// @brief GameObjectRegistry から指定のキーで GameObject を取得するスクリプト
/// 取得失敗時は警告を表示する
/// @Attention GameObjectRegistry コンポーネント必須
/// </summary>
[BehaviourTitle("文字列検索でgameObjectを取得しパラメーターに登録する")]
public class GetGameObjectByKey : StateBehaviour
{
    public string objectKey; // 取得したいオブジェクトのキー
    public ParameterReference<GameObject> m_targetObjectParam; // 取得結果を格納するパラメータ
    [Tooltip("登録されていなければ最親から取得する")]
    public GameObjectRegistry m_registry; // 親オブジェクトにアタッチされたGameObjectRegistry

    private void Awake()
    {
        SetRegistryFromParent();
    }
    public override void OnStateBegin()
    {
        SetRegistryFromParent();
        if (m_registry != null)
        {
            // キーに基づいてGameObjectを取得
            GameObject targetObject = m_registry.GetObjectByKey(objectKey);
            if (targetObject != null)
            {
                // Arborのパラメータに取得したオブジェクトを設定
                m_targetObjectParam.value = targetObject;
            }
            else
            {
                Debug.LogWarning($"Object with key '{objectKey}' not found.");
            }
        }
        else
        {
            Debug.LogWarning("Registry is not assigned.");
        }
    }

    //! @brief 親オブジェクトから GameObjectRegistry を取得する (未設定時のみ)
    private void SetRegistryFromParent()
    {
        if (m_registry != null)
        {
            return;
        }
        m_registry = GetComponentInParent<GameObjectRegistry>();
    }
}