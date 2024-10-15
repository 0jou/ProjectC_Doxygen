using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimatorEventCreateObjectInParent : AnimatorEvents.EventNodeBase
{
    // 発生させるプレハブ
    //継承させるためにprivate->protectedに変更（山本）

    [Header("ステート切り替え時に削除するか")]
    [SerializeField] protected bool m_doDeleteOnExitState = true;
    [Header("発生させるプレハブ")]
    [SerializeField] protected GameObject m_object;
    [Header("親オブジェクト(空ならAnimator直下)")]
    [SerializeField] protected string m_parent;

    protected GameObject m_createdInstance;

    // 時間が来たときに実行
    public override void OnEvent(Animator animator)
    {
        if (m_parent.Length != 0)
        {
            if (animator.transform.root.TryGetComponent(out ShareNodes nodes))
            {
                if (nodes.Nodes.TryGetValue(m_parent, out Transform parent))
                {
                    m_createdInstance = GameObject.Instantiate(m_object, parent);
                }
                else
                {
                    Debug.LogError("攻撃当たり判定の親オブジェクトが見つかりませんでした。" +
                        "登録名が間違っているか、ShareNodeに追加されていません。");
                }
            }
            else Debug.LogError("ShareNodesコンポーネントが見つかりませんでした");
        }
        else
        {
            m_createdInstance = GameObject.Instantiate(m_object, animator.transform);
        }

    }

    public override void OnExit(Animator animator)
    {
        base.OnExit(animator);
        if (m_doDeleteOnExitState)
        {
            if (m_createdInstance == null) return;
            GameObject.Destroy(m_createdInstance);
        }
    }
}
