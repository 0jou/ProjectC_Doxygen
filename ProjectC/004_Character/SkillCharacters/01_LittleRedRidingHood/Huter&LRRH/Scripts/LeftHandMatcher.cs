using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandMatcher : MonoBehaviour
{
    [SerializeField] private Transform m_leftHandPoint;
    private AvatarIKGoal m_ikGoal = AvatarIKGoal.LeftHand;
    private Animator m_animator;


    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }


    private void OnAnimatorIK(int layerIndex)
    {
        m_animator.SetIKPositionWeight(m_ikGoal,1f);
        m_animator.SetIKRotationWeight(m_ikGoal, 1f);
        m_animator.SetIKPosition(m_ikGoal, m_leftHandPoint.position);
        m_animator.SetIKRotation(m_ikGoal, m_leftHandPoint.rotation);

    }

}
