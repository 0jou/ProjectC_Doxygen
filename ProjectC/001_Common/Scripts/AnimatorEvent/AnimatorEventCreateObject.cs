using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アニメーターのインスペクターからイベントから、オブジェクトを出す(倉田)
[System.Serializable]
public class AnimatorEventCreateObject : AnimatorEvents.EventNodeBase
{

    [Header("ステート切り替え時に削除するか")]
    [SerializeField] private bool m_doDeleteOnExitState = true;
    [Header("発生させるプレハブ")]
    [SerializeField] private GameObject m_object;

    private GameObject m_createdInstance;

    // 時間が来たときに実行
    public override void OnEvent(Animator animator)
    {
        m_createdInstance = GameObject.Instantiate(m_object, animator.transform);
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