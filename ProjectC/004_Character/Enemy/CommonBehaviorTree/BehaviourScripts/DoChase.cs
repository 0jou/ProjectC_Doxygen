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
	//[SerializeField] private FlexibleChaseParameters m_chaseParameter;

    [SerializeField]
    [SlotType(typeof(EnemyParameters))]
    private FlexibleComponent m_enemyParameters;

	//protected override void OnAwake() {
	//}

	//protected override void OnStart() {
	//}


    // プレイヤーとの距離が遠くなりすぎた or プレイヤーが自身のスポーン地から遠くに行ったら諦める
	protected override bool OnConditionCheck() {
        if (m_enemyParameters == null || m_enemyParameters.value == null) return false;
        EnemyParameters enemyParameter = m_enemyParameters.value as EnemyParameters;
        ChaseData chaseParameters = enemyParameter.GetChaseData();

        Vector2 myPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetPos = new Vector2(enemyParameter.Target.position.x, enemyParameter.Target.position.z);

        float distToTarget = Vector2.Distance(myPos, targetPos);
        float distToSpawn = Vector3.Distance(enemyParameter.Target.position, (_agent.value as AgentController).StartPosition);
        bool isChase = (distToTarget <= chaseParameters.ChaseDistFromTarget)
            && (distToSpawn <= chaseParameters.DistAwayFromSpawnPos);
        return isChase;
    }

 //   protected override void OnEnd() {
	//}
}
