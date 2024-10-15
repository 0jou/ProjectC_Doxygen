using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// アニメーターのインスペクターからイベントから、攻撃判定を出す(倉田)
[System.Serializable]
public class AnimEventAttack : AnimatorEvents.EventNodeBase
{
    
    [Header("ステート切り替え時に削除するか")]
    [SerializeField] private bool m_doDeleteOnExitState = true;
    [Header("発生させるプレハブ")]
    [SerializeField] private GameObject m_assetAttack;
    [Header("親オブジェクト(空ならAnimator直下)")]
    [SerializeField] private string m_parent;

    private GameObject m_createdInstance;

    // 時間が来たときに実行
    public override void OnEvent(Animator animator)
    {
        var charaCore = animator.transform.GetComponentInParent<CharacterCore>();

        m_createdInstance = CreateObject(animator);

        // 作成者情報を記憶
        var ownerInfo = m_createdInstance.AddComponent<OwnerInfoTag>();
        ownerInfo.GroupNo = charaCore.GroupNo;
        ownerInfo.Characore = charaCore;
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
    private GameObject CreateObject(Animator animator)
    {
        if (m_parent.Length != 0)
        {
            if (animator.transform.root.TryGetComponent(out ShareNodes nodes))
            {
                if (nodes.Nodes.TryGetValue(m_parent, out Transform parent))
                {
                    SoundManager.Instance.Start3DPlayback("Swing", parent.transform.position);
                    return GameObject.Instantiate(m_assetAttack, parent);
                }
                else
                {
                    Debug.LogError("攻撃当たり判定の親オブジェクトが見つかりませんでした。" +
                        "登録名が間違っているか、ShareNodeに追加されていません。");
                }
            }
            else Debug.LogError("ShareNodesコンポーネントが見つかりませんでした");
        }
        SoundManager.Instance.Start3DPlayback("Swing", animator.transform.position);
        return GameObject.Instantiate(m_assetAttack, animator.transform);
    }
}