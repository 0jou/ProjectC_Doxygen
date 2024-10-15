/*!
 * @file CurrentAnimatorCheckerState.cs
 * @brief 現在のアニメーターを確認し、指定のアニメーションであればステートを遷移する
 * @author 田内
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

/// <summary>
/// @brief 現在のアニメーターを確認し、指定のアニメーションであればステートを遷移するステート
/// </summary>
[AddComponentMenu("Animator/CurrentAnimatorCheckerState")]
public class CurrentAnimatorCheckerState : StateBehaviour
{
    //! @brief アニメーターをエディタからセットする
    [Header("アニメーター")]
    [SerializeField, SlotType(typeof(Animator))]
    private FlexibleComponent m_animator = new FlexibleComponent(FlexibleHierarchyType.Self);

    private Animator m_currentAnimator = null;

    //! @brief 識別するアニメーションの名前
    [SerializeField]
    private string m_animationName = "";

    //! @brief 遷移先のリンク
    [SerializeField]
    private StateLink m_successLink = null;

    //! @brief アニメーターのセット 初期化
    private void SetAnimator()
    {
        if (m_animator == null) return;

        if (m_animator.value is Animator)
        {
            m_currentAnimator = m_animator.value as Animator;
        }
    }

    //! @brief アニメーションが指定のものであれば遷移する
    private void Check()
    {

        if (m_currentAnimator != null)
        {
            AnimatorStateInfo stateInfo = m_currentAnimator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName(m_animationName))
            {
                SetTransition();
            }

        }
    }

    //! @brief 遷移先のセット
    private void SetTransition()
    {
        if (m_successLink == null)
        {
            Debug.LogError("リンク先が存在しません");
            return;
        }

        Transition(m_successLink);
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
        SetAnimator();
    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        Check();
    }
}
