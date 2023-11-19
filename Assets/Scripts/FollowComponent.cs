using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowComponent : MonoBehaviour
{
    public NavMeshAgent navMesh;
    public int Follow_Distance;
    private bool is_moving; 
    private Animator my_animator;

    public bool isMoving()
    {
        return navMesh.remainingDistance <= navMesh.stoppingDistance;
    }

    // Start is called before the first frame update
    void Start()
    {
        navMesh = this.GetComponent<NavMeshAgent>();
    	my_animator = this.GetComponent<Animator>();
    	is_moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        double distance = Vector3.Distance(this.gameObject.transform.position, Main.instance.castle.position);
    	if(distance <= Follow_Distance)
    	    navMesh.SetDestination(Main.instance.castle.transform.position);
    	else
    	    //go to the player.
    	is_moving = isMoving();
    	my_animator.SetBool("is_moving", is_moving);
    }
}