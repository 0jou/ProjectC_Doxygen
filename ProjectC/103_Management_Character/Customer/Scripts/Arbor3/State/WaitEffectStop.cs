/*!
 * @file WaitEffectStop.cs
 * @brief 指定したエフェクトが終了した段階でStateを抜けるようにする
 * @author 山本
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using UnityEngine.VFX;

[AddComponentMenu("")]
public class WaitEffectStop : BaseCustomerStateBehaviour {

    //指定したエフェクトが終了した段階でStateを抜けるようにする（山本）
    //EffectObservationコンポーネントが存在し、そのコンポーネントにエフェクトがセットされている必要がある。

    [SerializeField] private StateLink m_nextState;
	private EffectObservation m_effectObserv = null;

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
	public override void OnStateUpdate() {

		if (m_effectObserv == null)
		{
			var obj = GetRootGameObject();
			if(obj.TryGetComponent(out EffectObservation _observ))
			{
				m_effectObserv = _observ;
			}
			else
			{
				//Debug.LogError("EffectObservationコンポーネントがない");
				return;
			}

			return;
		}
		//エフェクトが終了しているかをチェック
		else if(m_effectObserv.IsFinishedEffect())
		{
			Transition(m_nextState);
		}

	}

	// OnStateLateUpdate is called once per frame, after Update has finished.
	public override void OnStateLateUpdate() {
	}
}
