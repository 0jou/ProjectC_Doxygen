using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/WaitQueueCustomer")]
[AddComponentMenu("")]
public class WaitQueueCustomer : BaseCustomerStateBehaviour
{

	// 行列を待つ、行列に更新があれば
	// 制作者　田内

	//====================================================================
	// ステート

	[SerializeField]
	private StateLink m_successLink = null;

	//====================================================================


	// 待機列が更新されたか
	private void UpdateQueue()
    {

		var data = GetCustomerData();
		if (data == null) return;


		var queueData = data.QueueData;
		if (queueData == null) return;


		// 更新されていれば
		if (queueData.IsUpdate)
		{
			queueData.IsUpdate = false;

			SetTransition(m_successLink);

		}
	}

	//==============================================


	// Use this for initialization
	void Start () {
	
	}

	// Use this for awake state
	public override void OnStateAwake() {
	}

	// Use this for enter state
	public override void OnStateBegin()
	{
	}

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// OnStateUpdate is called once per frame
	public override void OnStateUpdate() 
	{
		UpdateQueue();
	}

	// OnStateLateUpdate is called once per frame, after Update has finished.
	public override void OnStateLateUpdate() {
	}
}
