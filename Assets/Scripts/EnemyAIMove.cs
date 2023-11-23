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
        curAttackTime = 0;
        attackTime = 1.75f;

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
                curAttackTime = 0;
                StartCoroutine(doAOEDmg());
                attackCoolDown();
                Invoke("attackThenChange",5f);
            }
            
        }
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
        Collider[] collides = Physics.OverlapSphere(transform.position, 5f);
        foreach (Collider c in collides)
        {
            Debug.Log(c.gameObject);
        }
        yield return new WaitForSeconds(1f);
        explosionAOE.SetActive(false);
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
        explosionAOE.SetActive(false);
        canDoDamage = false;
        curAttackTime = 0;
    }
}
