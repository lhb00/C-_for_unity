using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMissile : Bullet // MonoBehaviour를 Bullet으로 교체하여 상속하기
{
    public Transform target;
    NavMeshAgent nav;
    
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        nav.SetDestination(target.position);
    }
}
