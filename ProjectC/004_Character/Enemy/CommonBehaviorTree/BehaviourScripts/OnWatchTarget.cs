using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

[AddComponentMenu("")]
[AddBehaviourMenu("OnWatchTarget")]
[BehaviourTitle("OnWatchTarget")]
public class OnWatchTarget : Decorator {
    [SerializeField] private FlexibleTransform m_targetTransform;
    [SerializeField] private FlexibleFloat m_stoppingDistance;
    [SerializeField] private FlexibleInt m_viewAngle;


	protected override bool OnConditionCheck()
    {
        if (Vector3.Distance(transform.position, m_targetTransform.value.position) <= m_stoppingDistance.value)
        {
            return true;
        }

        float angle;
        angle = Vector3.Angle(transform.forward, m_targetTransform.value.position - transform.position);
        if (angle > m_viewAngle.value * 0.5f) return false;

        if (!transform.root.gameObject.TryGetComponent(out Collider myCollider))
        {
            return false;
        }
        if (!m_targetTransform.value.gameObject.TryGetComponent(out Collider targetCollider))
        {
            return false;
        }
        Vector3 dir = targetCollider.bounds.center - myCollider.bounds.center;
        float dist = Vector3.Distance(targetCollider.bounds.center, myCollider.bounds.center);
        Ray ray = new Ray(myCollider.bounds.center, dir);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * dist, Color.red);
        if (Physics.Raycast(ray, out hit, dist))
        {
            if (hit.collider.gameObject.name == targetCollider.gameObject.name)
            {
                return true;
            }
        }

        return false;
    }

	protected override void OnEnd() {
	}
}
