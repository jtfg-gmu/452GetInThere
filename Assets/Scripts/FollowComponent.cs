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

    public bool isMoving()
    {
        return navMesh.remainingDistance > navMesh.stoppingDistance;
    }

    // Start is called before the first frame update
    void Start()
    {
        m = Main.instance;
        navMesh = this.GetComponent<NavMeshAgent>();
        my_animator = this.GetComponent<Animator>();
        navMesh.stoppingDistance = 3f;
        is_moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        double shortestDistance = 2000000;
        Transform closestCastle = null;

        for (var i = 0; i < m.castleLocations.Length; i++)
        {
            double distance = Vector3.Distance(this.gameObject.transform.position, m.castleLocations[i].transform.position);
            if (distance <= shortestDistance)
            {
                shortestDistance = distance;
                closestCastle = m.castleLocations[i];
            }
        }
        navMesh.SetDestination(closestCastle.position);
        is_moving = isMoving();
        my_animator.SetBool("is_moving", is_moving);
    }
}