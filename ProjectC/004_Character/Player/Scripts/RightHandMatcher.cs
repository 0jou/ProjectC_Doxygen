using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandMatcher : MonoBehaviour
{
    [SerializeField] private Transform m_rightHandPoint;
    private AvatarIKGoal m_ikGoal = AvatarIKGoal.RightHand;
    private Animator m_animator;


    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }


    private void OnAnimatorIK(int layerIndex)
    {
        m_animator.SetIKPositionWeight(m_ikGoal, 1f);
        m_animator.SetIKRotationWeight(m_ikGoal, 1f);
        m_animator.SetIKPosition(m_ikGoal, m_rightHandPoint.position);
        m_animator.SetIKRotation(m_ikGoal, m_rightHandPoint.rotation);

    }
}
