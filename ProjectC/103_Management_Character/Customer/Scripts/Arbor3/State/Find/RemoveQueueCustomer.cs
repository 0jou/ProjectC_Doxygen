using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class RemoveQueueCustomer : BaseCustomerStateBehaviour
{
	// 待機列から自分を取り除く
	// 制作者　田内

	private void RemoveQueue()
	{

		var data = GetCustomerData();
		if (data == null) return;

		var controller = QueueManager.instance;
		if (controller == null) return;


		var queueData = data.QueueData;
		if (queueData == null) return;

		// 待ち列に追加
		controller.RemoveQueueObject(queueData);

	}





	//=====================================================


	// Use this for initialization
	void Start () {
	
	}

	// Use this for awake state
	public override void OnStateAwake() {
	}

	// Use this for enter state
	public override void OnStateBegin() 
	{
		RemoveQueue();
	}

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// OnStateUpdate is called once per frame
	public override void OnStateUpdate() {
	}

	// OnStateLateUpdate is called once per frame, after Update has finished.
	public override void OnStateLateUpdate() {
	}
}
