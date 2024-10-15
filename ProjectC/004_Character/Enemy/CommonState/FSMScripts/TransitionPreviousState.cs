using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class TransitionPreviousState : StateBehaviour
{

    private float m_countDown;

    // Use this for initialization
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
        m_countDown = 0.1f;
    }

    // Use this for exit state
    public override void OnStateEnd()
    {
    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
    }

    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
        m_countDown -= Time.deltaTime;
        if (m_countDown <= 0f)
        {
            int a = 0;
            a++;
            stateMachine.Transition(stateMachine.prevTransitionState);
        }
    }
}
