using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

// アニメーターのインスペクターからイベントから、アイテムを投げる際一時停止（吉田）
[System.Serializable]
public class AnimEventSpeedStop : AnimatorEvents.EventNodeBase
{
    // 時間が来たときに実行
    public override void OnEvent(Animator animator)
    {
        animator.SetFloat("Speed", 0);
    }

}

// bool の値をtrueにする（吉田）
// TODO:スクリプト別のファイルに分けた方がいいかも
[System.Serializable]
public class AnimatorEventBoolTrue : AnimatorEvents.EventNodeBase
{
    [Header("trueにするboolの名前")]
    [SerializeField] private string m_boolName;

    public override void OnEvent(Animator animator)
    {
        animator.SetBool(m_boolName, true);
    }

    public override void OnExit(Animator animator) 
    { 
        animator.SetBool(m_boolName, false);
    }

}
