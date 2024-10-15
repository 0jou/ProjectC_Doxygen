using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshControl : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Transform target;

    private NavMeshAgent myAgent;

    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        myAgent.SetDestination(target.position);
    }
}
