using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore;
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
    private GameObject explosionAOE;
    private bool canDoDamage;
    private float curAttackTime;
    private float attackTime;
    private float soldierSearchRange;
    private float soldierAttackRange;
    private bool soldierInSearchRange;
    private bool soldierInAttackRange;
    void Start()
    {
        m = Main.instance;
        castleLocations = m.castleLocations;
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.stoppingDistance = 2f;
        explosionAOE = transform.GetChild(2).gameObject;
        explosionAOE.SetActive(false);
        search = true;
        health = 150;
        path = new NavMeshPath();
        elapsed = 0.0f;
        canDoDamage = false;
        curAttackTime = 0f;
        attackTime = 1.75f;
        soldierSearchRange = 30f;
        soldierAttackRange = 4f;
        soldierInSearchRange = false;
        soldierInAttackRange = false;

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
            if (reachAndStay() && !canDoDamage)
            {
                canDoDamage = true;
                curAttackTime = 0f;
                StartCoroutine(doAOEDmg());
                Invoke("attackThenChange",5f);
            }
        }
        attackCoolDown();
    }

    private void attackCoolDown()
    {
        curAttackTime += Time.deltaTime;
        if (curAttackTime >= attackTime)
        {
            canDoDamage = false;
        }
    }
    

    private IEnumerator doAOEDmg()
    {
        yield return new WaitForSeconds(0.25f);
        explosionAOE.SetActive(true);
        Collider[] collides = Physics.OverlapSphere(transform.position, 7f);
        foreach (Collider c in collides)
        {
            if (c.gameObject.CompareTag("soldier"))
            {
                FollowComponent followComponent = c.gameObject.GetComponent<FollowComponent>();
                followComponent.health -= 3;
                if (followComponent.health <= 0)
                {
                    Destroy(c.gameObject);
                }
            }
        }
        yield return new WaitForSeconds(1f);
        explosionAOE.SetActive(false);
    }

    private bool isSoldierStillAlive()
    {
        return GameObject.FindGameObjectsWithTag("soldier").Length > 0;
    }

    private void randomlyAttack()
    {
        if (isSoldierStillAlive())
        {
            float distToSoldier = Vector3.Distance(transform.position,closestSoldier().position);
            soldierInSearchRange = distToSoldier <= soldierSearchRange;
            soldierInAttackRange = distToSoldier <= soldierAttackRange;
            if (soldierInSearchRange && !soldierInAttackRange)
            {
                navMesh.SetDestination(closestSoldier().position);
            }

            if (soldierInAttackRange && soldierInSearchRange)
            {
                navMesh.SetDestination(transform.position);
            }
        }

        if (!isSoldierStillAlive())
        {
            soldierInAttackRange = false;
            soldierInSearchRange = false;
        }

        if (!soldierInAttackRange && !soldierInSearchRange)
        {
            Random rnd = new Random();
            int index = rnd.Next(castleLocations.Length);
            elapsed += Time.deltaTime;
            bool isPath = NavMesh.CalculatePath(transform.position, castleLocations[index].position, NavMesh.AllAreas, path);
            Debug.Log(gameObject + " path to " + castleLocations[index].gameObject + " " + isPath);
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
    }

    private Transform closestSoldier()
    {
        GameObject[] soldiers = GameObject.FindGameObjectsWithTag("soldier");
        Transform closestSoldier = null;
        float dist = 1000000;
        foreach (var soldier in soldiers)
        {
            float cur = Vector3.Distance(transform.position, soldier.transform.position);
            if (cur <= dist)
            {
                dist = cur;
                closestSoldier = soldier.transform;
            }
        }

        return closestSoldier;
    }

    private void attackThenChange()
    {
        search = true;
        explosionAOE.SetActive(false);
        canDoDamage = false;
        curAttackTime = 0;
    }
}
