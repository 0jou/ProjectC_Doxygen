using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using UnityEngine.Animations.Rigging;

[AddComponentMenu("")]
public class ResetLookTarget : StateBehaviour {

	//攻撃の際にリグに重みづけしたものを0に戻す
	//LookTargetをプレイヤーの原点に移動（山本）

    [SerializeField]
    [SlotType(typeof(Animator))]
    private FlexibleComponent m_animator = new FlexibleComponent(FlexibleHierarchyType.RootGraph);

	private Animator m_myAnimator;

    // Use this for initialization
    void Start () 
	{
		
	
	}

	// Use this for awake state
	public override void OnStateAwake() 
	{
       

	}

	// Use this for enter state
	public override void OnStateBegin() 
	{
        m_myAnimator = m_animator.value as Animator;


        if (m_myAnimator.TryGetComponent(out SetRigInfo rigInfo))
        {
            if (rigInfo == null)
            {
                return;
            }

            if (rigInfo.UseRig.weight != 0)
            {
                rigInfo.UseRig.weight = 0;
            }

            if (rigInfo.TargetTransform)
            {
                rigInfo.TargetTransform.position = m_myAnimator.transform.position;
            }


        }

    }

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// OnStateUpdate is called once per frame
	public override void OnStateUpdate() 
	{
        

    }

	// OnStateLateUpdate is called once per frame, after Update has finished.
	public override void OnStateLateUpdate() 
    {
       
    }
}
