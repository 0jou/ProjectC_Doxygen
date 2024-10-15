using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

// 一定時間経過後ステートを進める　(伊波)

[AddComponentMenu("")]
public class Wait : StateBehaviour {
	[SerializeField] private StateLink m_nextState;
	[SerializeField] private FlexibleFloat m_maxDelay;
	private float m_remainingTime;

	// Use this for initialization
	void Start () {
	
	}

	// Use this for awake state
	public override void OnStateAwake() {
	}

	// Use this for enter state
	public override void OnStateBegin() {
		m_remainingTime = m_maxDelay.value;
    }

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// OnStateUpdate is called once per frame
	public override void OnStateUpdate() {
        m_remainingTime -= Time.deltaTime;
		if(m_remainingTime <= 0f)
		{
			Transition(m_nextState);
		}
	}

	// OnStateLateUpdate is called once per frame, after Update has finished.
	public override void OnStateLateUpdate() {
	}
}
