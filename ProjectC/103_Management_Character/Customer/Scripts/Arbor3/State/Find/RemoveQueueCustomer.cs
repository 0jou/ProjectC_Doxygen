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


		// 待ち列に追加
		QueueManager.instance.RemoveQueueObject(data.QueueData);

	}

	//=====================================================

	// Use this for enter state
	public override void OnStateBegin() 
	{
		RemoveQueue();
	}
}
