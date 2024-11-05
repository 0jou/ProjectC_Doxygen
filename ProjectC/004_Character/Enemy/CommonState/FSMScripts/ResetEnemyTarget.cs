using UnityEngine;
using Arbor;

[AddComponentMenu("")]
[AddBehaviourMenu("Enemy/EnemyResetTarget")]
public class EnemyResetTarget : StateBehaviour
{
    [SerializeField]
    [SlotType(typeof(EnemyParameters))]
    private FlexibleComponent _enemyParameters = new FlexibleComponent(FlexibleHierarchyType.RootGraph);

    private EnemyParameters enemyParameters;

    //public override void OnStateAwake()
    //{
    //}

    public override void OnStateBegin()
    {
        if (!enemyParameters)
        {
            enemyParameters = _enemyParameters.value as EnemyParameters;
            if (!enemyParameters) return;
        }
        enemyParameters.Target = null;
    }
}