using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderMove : MonoBehaviour
{

    [SerializeField] private float m_moveSpeed = 2f;

    private Vector3 m_direction;
    private Transform m_transform;

    // Start is called before the first frame update
    void Start()
    {
        m_transform = GetComponent<Transform>();
      
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        m_transform.position += transform.forward * m_moveSpeed * Time.deltaTime;

    }
}
