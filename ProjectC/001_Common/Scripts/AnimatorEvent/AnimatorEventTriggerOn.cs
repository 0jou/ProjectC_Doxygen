
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アニメーターのインスペクターからイベントから、アニメーターのトリガーをオンにする(倉田)
[System.Serializable]
public class AnimatorEventTrigger : AnimatorEvents.EventNodeBase
{
    // 発生させるプレハブ
    [Header("オンにするトリガーの名前")]
    [SerializeField] private string m_triggerName;

    // 時間が来たときに実行
    public override void OnEvent(Animator animator)
    {
        animator.SetTrigger(m_triggerName);
    }

    public override void OnExit(Animator animator)
    {
        base.OnExit(animator);
        animator.ResetTrigger(m_triggerName);
    }

}