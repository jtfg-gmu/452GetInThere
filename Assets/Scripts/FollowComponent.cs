using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowComponent : MonoBehaviour
{

    Main m;
    public NavMeshAgent navMesh;
    public int Follow_Distance;
    private bool is_moving;
    private Animator my_animator;
    private GameObject player;
    private float chaseEnemyRange;
    private float attackEnemyRange;
    public bool isChasing;
    public bool isAttacking;

    public bool isMoving()
    {
        return navMesh.remainingDistance > navMesh.stoppingDistance;
    }

    // Start is called before the first frame update
    void Start()
    {
        isAttacking = false;
        isChasing = false;
        chaseEnemyRange = 50f;
        attackEnemyRange = 15f;
        m = Main.instance;
        navMesh = this.GetComponent<NavMeshAgent>();
        my_animator = this.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        navMesh.stoppingDistance = 3f;
        navMesh.avoidancePriority = 0;
        is_moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyStillAlive())
        {
            Transform closestEnemy = getClosestEnemy();

            float distToEnemy = Vector3.Distance(transform.position, closestEnemy.position);
            isAttacking = distToEnemy <= attackEnemyRange;
            isChasing = distToEnemy <= chaseEnemyRange; 
            if (isChasing && !isAttacking)
            {
                navMesh.SetDestination(closestEnemy.position);
            }

            if (isAttacking && isChasing)
            {
                navMesh.SetDestination(transform.position);
                transform.LookAt(closestEnemy.transform);
            }
        }

        if (!enemyStillAlive())
        {
            isAttacking = false;
            isChasing = false;
        }
        
        
        if (!isAttacking && !isChasing)
        {
            Debug.Log("pathing to allies");
            double shortestDistance = 2000000;
            Transform closestObject = null;
        
            double distance = Vector3.Distance(this.gameObject.transform.position, player.transform.position);
            if (distance <= shortestDistance)
            {
                shortestDistance = distance;
                closestObject = player.transform;
            }
            for (var i = 0; i < m.castleLocations.Length; i++)
            {
                distance = Vector3.Distance(this.gameObject.transform.position, m.castleLocations[i].transform.position);
                if (distance <= shortestDistance)
                {
                    shortestDistance = distance;
                    closestObject = m.castleLocations[i];
                }
            }
            Debug.Log("closest object is " + closestObject.gameObject);
            navMesh.SetDestination(closestObject.position);
            is_moving = isMoving();
            my_animator.SetBool("is_moving", is_moving);
        }
        
    }

    private bool enemyStillAlive()
    {
        return GameObject.FindGameObjectsWithTag("enemy").Length > 0;
    }
    private Transform getClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        GameObject enemy = null;
        double minEnemyDist = 1000000;
        foreach (var e in enemies)
        {
            double dist = Vector3.Distance(e.transform.position, transform.position);
            if (dist <= minEnemyDist)
            {
                minEnemyDist = dist;
                enemy = e;
            }
        }

        return enemy.transform;
    }
}