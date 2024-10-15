/*!
 * @file DoChase.cs
 * @brief 敵がチェイスを継続するか判断するDecorator
 * @author 伊波
 */
using UnityEngine;
using Arbor;
using Arbor.BehaviourTree;

/**
* @brief 敵がチェイスを継続するか判断するDecorator　伊波
* @details 距離計算は高さを無視
*/
[AddComponentMenu("")]
[AddBehaviourMenu("Enemy/DoChase")]
[BehaviourTitle("Enemy/DoChase")]
public class DoChase : Decorator {

    [SerializeField]
    [SlotType(typeof(AgentController))]
    private FlexibleComponent _agent = new FlexibleComponent(FlexibleHierarchyType.RootGraph);
    [SerializeField] private FlexibleTransform m_target;
	[SerializeField] private FlexibleChaseParameters m_chaseParameter;


	//protected override void OnAwake() {
	//}

	//protected override void OnStart() {
	//}


    // プレイヤーとの距離が遠くなりすぎた or プレイヤーが自身のスポーン地から遠くに行ったら諦める
	protected override bool OnConditionCheck() {
		Vector2 myPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetPos = new Vector2(m_target.value.position.x, m_target.value.position.z);

        float distToTarget = Vector2.Distance(myPos, targetPos);
        float distToSpawn = Vector3.Distance(m_target.value.position, (_agent.value as AgentController).StartPosition);
        bool isChase = (distToTarget <= m_chaseParameter.value.ChaseDistFromTarget)
            && (distToSpawn <= m_chaseParameter.value.DistAwayFromSpawnPos);
        return isChase;
    }

 //   protected override void OnEnd() {
	//}
}
