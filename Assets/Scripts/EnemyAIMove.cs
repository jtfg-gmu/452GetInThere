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
    public int health;
    private NavMeshPath path;
    private float elapsed = 0.0f;
    void Start()
    {
        m = Main.instance;
        castleLocations = m.castleLocations;
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.stoppingDistance = 2f;
        search = true;
        health = 150;
        path = new NavMeshPath();
        elapsed = 0.0f;

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
        elapsed += Time.deltaTime;
        bool isPath = NavMesh.CalculatePath(transform.position, castleLocations[index].position, NavMesh.AllAreas, path);
        while (!isPath)
        {
            rnd = new Random();
            index = rnd.Next(castleLocations.Length);
            isPath = NavMesh.CalculatePath(transform.position, castleLocations[index].position, NavMesh.AllAreas, path);
        }
        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        navMesh.SetDestination(castleLocations[index].position);
    }

    private void attackThenChange()
    {
        search = true;
    }
}
