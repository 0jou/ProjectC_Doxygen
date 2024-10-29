using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class RotateToTarget : StateBehaviour {

	[SerializeField] private FlexibleVector3 m_targetPos;
	[SerializeField]
	[SlotType(typeof(CharacterCore))]
	protected FlexibleComponent m_characterCore = new FlexibleComponent(FlexibleHierarchyType.RootGraph);

	// Use this for initialization
	void Start () {
	
	}

	//// Use this for awake state
	//public override void OnStateAwake() {
	//}

	//// Use this for enter state
	//public override void OnStateBegin() {
	//}

	//// Use this for exit state
	//public override void OnStateEnd() {
	//}
	
	// OnStateUpdate is called once per frame
	public override void OnStateUpdate()
	{
		if (!m_characterCore.value) return;
		var core = m_characterCore.value as CharacterCore;
		if (core)
        {
			core.SetRotateToTarget(m_targetPos.value - core.transform.position);
		}
	}

	//// OnStateLateUpdate is called once per frame, after Update has finished.
	//public override void OnStateLateUpdate() {
	//}
}
