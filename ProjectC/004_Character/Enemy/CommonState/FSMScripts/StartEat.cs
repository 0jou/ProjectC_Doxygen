using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
[AddBehaviourMenu("Enemy/StartEat")]
public class StartEat : StateBehaviour
{
    [SerializeField]
    [SlotType(typeof(EnemyParameters))]
    private FlexibleComponent m_enemyParameters = new FlexibleComponent(FlexibleHierarchyType.RootGraph);

	private EnemyParameters _enemyParameters;

    // Use this for initialization
    void Start () 
	{
		//if(m_target.value.gameObject==null)
		//{
		//	return;
		//}

		//if(m_target.value.gameObject.TryGetComponent<EatingEffectContoroll>(out var effectContoroll))
		//{
		//	effectContoroll.OnPlayEffect();
		//}

	}

	// Use this for awake state
	public override void OnStateAwake() {
	}

	// Use this for enter state
	public override void OnStateBegin() 
	{
		_enemyParameters = m_enemyParameters.value as EnemyParameters;
        if (_enemyParameters == null)
        {
            return;
        }
        
        if (_enemyParameters.gameObject.TryGetComponent<EatingEffectContoroll>(out var effectContoroll))
        {
			effectContoroll.SetEffectPosition(gameObject.transform.position);
            effectContoroll.OnPlayEffect();
        }
    }

	// Use this for exit state
	public override void OnStateEnd() 
	{
		if (_enemyParameters == null) return;
        if (_enemyParameters.gameObject.TryGetComponent<EatingEffectContoroll>(out var effectContoroll))
        {
            effectContoroll.OnStopEffect();
        }

    }
	
	//// OnStateUpdate is called once per frame
	//public override void OnStateUpdate() {
	//}

	//// OnStateLateUpdate is called once per frame, after Update has finished.
	//public override void OnStateLateUpdate() {
	//}
}
