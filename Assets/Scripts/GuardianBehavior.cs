using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardianBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshAgent agent;
    public int health;
    private float tauntSearchRange;
    private float tauntAttackRange;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = 230;
        tauntSearchRange = 8f;
        tauntAttackRange = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
