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
    private GameObject enemy;

    public bool isMoving()
    {
        return navMesh.remainingDistance > navMesh.stoppingDistance;
    }

    // Start is called before the first frame update
    void Start()
    {
        isAttacking = false;
        isChasing = false;
        enemy = GameObject.FindGameObjectWithTag("enemy");
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
        float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
        // if (distToEnemy <= chaseEnemyRange && !(distToEnemy <= attackEnemyRange))
        // {
        //     isAttacking = false;
        //     navMesh.SetDestination(enemy.transform.position);
        // }
        // else if (distToEnemy <= chaseEnemyRange && distToEnemy <= attackkEnemyRange)
        // {
        //     navMesh.SetDestination(navMesh.transform.position);
        //     isAttacking = true;
        // }

        isAttacking = distToEnemy <= attackEnemyRange;
        isChasing = distToEnemy <= chaseEnemyRange; 
        
        if (!isAttacking && !isChasing)
        {
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
            navMesh.SetDestination(closestObject.position);
            is_moving = isMoving();
            my_animator.SetBool("is_moving", is_moving);
        }

        if (isChasing && !isAttacking)
        {
            navMesh.SetDestination(enemy.transform.position);
        }

        if (isAttacking && isChasing)
        {
            navMesh.SetDestination(transform.position);
            transform.LookAt(enemy.transform);
        }
        
        
    }
}