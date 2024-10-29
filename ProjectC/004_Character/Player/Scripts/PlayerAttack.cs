using Arbor.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// プレイヤーを攻撃状態に遷移する(倉田) 
public class PlayerAttack : MonoBehaviour
{

    [SerializeField] private PlayerInput m_input;
    private Animator m_animator;

    private void Awake()
    {
        
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_input.actions["Attack"].triggered)
        {
            m_animator.SetTrigger("Attack");
        }
    }

 


}
