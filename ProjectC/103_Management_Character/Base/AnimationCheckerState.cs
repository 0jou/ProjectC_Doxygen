/*!
 * @file AnimationCheckerState.cs
 * @brief アニメーションが終了したか/前のアニメーションから変更されたかを確認し遷移させるステート
 * @author 田内
 * @par 変更
 *      前のアニメーションから変更されたタイミングを追加 上甲
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

/// <summary>
/// @brief アニメーションが終了したか/前のアニメーションから変更されたかを確認するステート
/// @details
/// - アニメーションが終了し指定のアニメーションに変更されたらリンク先に遷移
/// - アニメーションが指定のものから変更されたタイミングでリンク先に遷移 上甲
/// </summary>
[AddComponentMenu("Animator/EndAnimationCheckerState")]
public class EndAnimationCheckerState : StateBehaviour
{
    [Header("アニメーター")]
    [SerializeField, SlotType(typeof(Animator))]
    private FlexibleComponent m_animator = new FlexibleComponent(FlexibleHierarchyType.Self);

    [SerializeField]
    private string m_animationName = "";

    [SerializeField]
    private StateLink m_successLink = null;


    private int m_backAnimationHash = -1;
    private int m_successAnimationHash = -2;
    private Animator GetAnimator()
    {
        if (m_animator == null) return null;

        if (m_animator.value is Animator)
        {
            var animator = m_animator.value as Animator;
            return animator;
        }
        else
        {
            return null;
        }

    }

    private void Check()
    {
        var animator = GetAnimator();
        if (animator != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName(m_animationName))
            {
                // アニメーションの終了を確認
                if (stateInfo.normalizedTime >= 1.0f)
                {
                    SetTransition();
                }
                m_successAnimationHash = stateInfo.shortNameHash;
            }
            else
            {
                // アニメーションが指定のものから変更されたタイミング 上甲
                if (m_successAnimationHash == m_backAnimationHash)
                {
                    SetTransition();
                }
            }
            m_backAnimationHash = stateInfo.shortNameHash;
        }
    }

    private void SetTransition()
    {
        if (m_successLink == null)
        {
            Debug.LogError("リンク先が存在しません");
            return;
        }

        Transition(m_successLink);
    }

    //=========================================================================

    void Start()
    {

    }

    // Use this for awake state
    public override void OnStateAwake()
    {
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
    }

    // Use this for exit state
    public override void OnStateEnd()
    {
    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        Check();
    }

    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
    }
}
