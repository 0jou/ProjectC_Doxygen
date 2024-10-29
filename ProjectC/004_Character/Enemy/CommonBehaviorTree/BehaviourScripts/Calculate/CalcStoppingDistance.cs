/**
* @file CalcStoppoingDistance.cs
* @brief Colliderの大きさを考慮した最近接距離を計算するCalculator
*/
using UnityEngine;
using Arbor;
using UnityEngine.AI;


[AddComponentMenu("")]
[AddBehaviourMenu("Float/CalcStoppingDistance")]
[BehaviourTitle("CalcStoppingDistance")]
/**
* @brief Agentとターゲットの大きさに基づいて停止距離をだす（伊波）
* @details 自身とターゲットのコライダーの幅を足したfloatを出すCalculator
*/
public class CalcStoppingDistance : Calculator
{
    [SerializeField]
    [SlotType(typeof(NavMeshAgent))]
    private FlexibleComponent m_rootObject = new (FlexibleHierarchyType.RootGraph);

    [SerializeField] private FlexibleTransform m_targetTransform;

    [SerializeField] private OutputSlotFloat m_result = new ();


    public override void OnCalculate()
    {
        NavMeshAgent controller = m_rootObject.value as NavMeshAgent;
        if (m_targetTransform.value == null)
        {
            m_result.SetValue(controller.radius);
            return;
        }

        if (m_targetTransform.value.gameObject.TryGetComponent(out CapsuleCollider m_targetCollider))
        {
            float radius = transform.root.localScale.x * controller.radius;
            // REVIEW:敵同士で殴り合うようにしたい場合や、細長いコライダーに対応出来ないコードなので修正が必要
            m_result.SetValue(m_targetCollider.radius * (float)System.Math.Sqrt(2f) + radius);
        }
        else
        {
            m_result.SetValue(controller.radius);
        }
    }
}
