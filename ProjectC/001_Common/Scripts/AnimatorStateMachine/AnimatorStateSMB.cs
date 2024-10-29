using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アニメーターのステートを、オブジェクトのステートと連携させるためのクラス
public class AnimatorStateSMB : StateMachineBehaviour
{
    [SerializeReference, SubclassSelector] private AnimatorStateMachine.ActionStateBase m_state;

    private AnimatorStateMachine m_stateMachine;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_stateMachine == null)
        {
            m_stateMachine = animator.GetComponentInParent<AnimatorStateMachine>();

            // 初期設定
            m_state.Initialize(m_stateMachine);
        }
        // ステート変更
        m_stateMachine.ChangeState(m_state);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // m_state.OnExit();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
