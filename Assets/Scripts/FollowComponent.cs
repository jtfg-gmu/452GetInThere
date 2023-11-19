using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowComponent : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject dest;
    

    // Start is called before the first frame update
    void Start()
    {
	    dest = GameObject.FindGameObjectWithTag("castle");
	    agent = GetComponent<NavMeshAgent>();
	    agent.SetDestination(dest.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
 //        double distance = Vector3.Distance(this.gameObject.transform.position, Main.instance.castle.position);
	// if(distance <= Follow_Distance)
	//     navMesh.SetDestination(Main.instance.transform.position);
	// is_moving = isMoving();
    }
}
