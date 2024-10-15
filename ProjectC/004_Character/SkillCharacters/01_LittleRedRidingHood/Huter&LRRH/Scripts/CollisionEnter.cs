using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnter : MonoBehaviour
{
    [SerializeField] GameObject m_effectObject;
    private Animator m_animator;

   
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("当たったよ");
        m_animator.SetTrigger("Idle");

        if(m_effectObject != null) 
        {
            //m_effectObject
            //エフェクトクローン
            GameObject.Instantiate(m_effectObject,transform);
        }

        

        Destroy(this);
    }

}
