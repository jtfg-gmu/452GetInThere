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
        player = GameObject.FindGameObjectWithTag("Player");
        navMesh.stoppingDistance = 3f;
        is_moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("player " + player);
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
}