using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControladorNavMesh : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform target;
    Animator animator;
    
    void Start()
    {
        navMeshAgent.destination = target.position;
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        animator.SetFloat("Velocidad", navMeshAgent.speed);
        if(transform.position == target.position)
        {
            FinalRecorrido();
        }
    }
    void FinalRecorrido()
    {
        navMeshAgent.isStopped = true;
    }
}
