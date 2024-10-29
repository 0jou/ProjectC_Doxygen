/*!
 * @file ToggleActiveAction.cs
 * @brief 指定のGameObjectのアクティブ状態を切り替えるスクリプト
 * @author 上甲
 */
using UnityEngine;
using Arbor;

/// <summary>
/// @brief 指定のGameObjectのアクティブ状態を切り替えるスクリプト
/// </summary>
public class ToggleActiveAction : StateBehaviour
{
    // Arborのパラメータを利用する
    [SerializeField] private ParameterReference<GameObject> targetObjectParam;
    public bool setActive; // アクティブにするかどうか

    public override void OnStateBegin()
    {
        GameObject targetObject = targetObjectParam.value; // パラメータからGameObjectを取得
        if (targetObject != null)
        {
            targetObject.SetActive(setActive);
            Debug.Log("アクティブ状態を切り替えました");
            return;
        }
        Debug.Log("ないよ！！！！！！！！！！！！！！！！！！！！！！！！！！");
    }
}
