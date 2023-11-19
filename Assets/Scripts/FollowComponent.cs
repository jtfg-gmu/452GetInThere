using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FollowComponent : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject dest;
    private Boolean is_moving;
    private float distance_limit = 10f;

    // Start is called before the first frame update
    void Start()
    {
	    is_moving = false;
	    dest = Main.instance.gameObject;
	    agent = GetComponent<NavMeshAgent>();
	    agent.stoppingDistance = 10f;
	    agent.SetDestination(dest.transform.position);
	    
    }

    // Update is called once per frame
    void Update()
    {
	    double distance = Vector3.Distance(this.gameObject.transform.position, dest.transform.position);
	    Debug.Log(distance);
	    if (agent.remainingDistance > agent.stoppingDistance)
	    {
		    agent.SetDestination(dest.transform.position);
		    gameObject.GetComponent<Animator>().SetBool("is_moving", true);
	    }
	    else
	    {
		    gameObject.GetComponent<Animator>().SetBool("is_moving", false);
	    }
    }

    private void OnCollisionEnter(Collision other)
    {
	    if (other.gameObject.CompareTag("floor"))
	    {
		    gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
	    }
    }
}
