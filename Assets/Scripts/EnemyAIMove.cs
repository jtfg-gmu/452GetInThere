using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class EnemyAIMove : MonoBehaviour
{
    // Start is called before the first frame update
    public NavMeshAgent navMesh;
    Main m;
    private Transform[] castleLocations;
    private Boolean search;
    void Start()
    {
        m = Main.instance;
        castleLocations = m.castleLocations;
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.stoppingDistance = 2f;
        search = true;

    }

    private Boolean reachAndStay()
    {
        return navMesh.remainingDistance <= navMesh.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (search)
        {
            randomlyAttack();
            search = false;
        }

        else if (search == false)
        {
            if (reachAndStay())
            {
                Invoke("attackThenChange",5f);
            }
        }

    }

    private void randomlyAttack()
    {
        Random rnd = new Random();
        int index = rnd.Next(castleLocations.Length);
        navMesh.SetDestination(castleLocations[index].position);
    }

    private void attackThenChange()
    {
        search = true;
    }
}
