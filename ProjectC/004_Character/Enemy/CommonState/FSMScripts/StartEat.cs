using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
[AddBehaviourMenu("Enemy/StartEat")]
public class StartEat : StateBehaviour {
	[SerializeField] private FlexibleTransform m_target;

	// Use this for initialization
	void Start () 
	{
		//if(m_target.value.gameObject==null)
		//{
		//	return;
		//}

		//if(m_target.value.gameObject.TryGetComponent<EatingEffectContoroll>(out var effectContoroll))
		//{
		//	effectContoroll.OnPlayEffect();
		//}

	}

	// Use this for awake state
	public override void OnStateAwake() {
	}

	// Use this for enter state
	public override void OnStateBegin() 
	{
        if (m_target.value == null)
        {
            return;
        }

        if (m_target.value.gameObject.TryGetComponent<EatingEffectContoroll>(out var effectContoroll))
        {
			effectContoroll.SetEffectPosition(gameObject.transform.position);
            effectContoroll.OnPlayEffect();
        }
    }

	// Use this for exit state
	public override void OnStateEnd() 
	{
		if (m_target.value == null) return;
        if (m_target.value.gameObject.TryGetComponent<EatingEffectContoroll>(out var effectContoroll))
        {
            effectContoroll.OnStopEffect();
        }

    }
	
	// OnStateUpdate is called once per frame
	public override void OnStateUpdate() {
	}

	// OnStateLateUpdate is called once per frame, after Update has finished.
	public override void OnStateLateUpdate() {
	}
}
