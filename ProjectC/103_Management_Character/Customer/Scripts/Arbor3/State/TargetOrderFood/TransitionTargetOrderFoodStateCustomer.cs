using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using OrderFoodInfo;

[AddComponentMenu("")]
public class TransitionTargetOrderFoodStateCustomer :BaseCustomerStateBehaviour
{
	// 制作者　田内
	// ターゲットの料理の状態で進むステートリンクを変更する

	//======================================================
	// ステートリンク

	[System.Serializable]
	private class TransitionTargetOrderFood
	{
		[SerializeField]
		public OrderFoodState State = OrderFoodState.FindStaff;

		[SerializeField]
		public StateLink StateLink = null;
	}

	[SerializeField]
	private List<TransitionTargetOrderFood> m_transitionTargetOrderFoodList = new();


	private void Check()
	{
		var data = GetCustomerData();
		if (data == null) return;

		var targetFood = data.TargetOrderFoodData;
		if (targetFood == null) return;


		foreach(var state in m_transitionTargetOrderFoodList)
        {
			if(targetFood.CurrentOrderFoodState==state.State)
            {
				SetTransition(state.StateLink);
				return ;
			}

        }
	}

	// Use this for initialization
	void Start () {
	
	}

	// Use this for awake state
	public override void OnStateAwake() {
	}

	// Use this for enter state
	public override void OnStateBegin() {
	}

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// OnStateUpdate is called once per frame
	public override void OnStateUpdate()
	{
		Check();
	}

	// OnStateLateUpdate is called once per frame, after Update has finished.
	public override void OnStateLateUpdate() {
	}
}
