using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowComponent : MonoBehaviour
{
    public NavMeshAgent navMesh;
    public int Follow_Distance;
    private bool is_moving; 

    public bool isMoving()
    {
        return navMesh.remainingDistance <= navMesh.stoppingDistance;
    }

    // Start is called before the first frame update
    void Start()
    {
        navMesh = this.GetComponent<NavMeshAgent>();
	is_moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        double distance = Vector3.Distance(this.gameObject.transform.position, Main.instance.castle.position);
	if(distance <= Follow_Distance)
	    navMesh.SetDestination(Main.instance.transform.position);
	is_moving = isMoving();
    }
}
