using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowComponent : MonoBehaviour
{
<<<<<<< Updated upstream
    public NavMeshAgent navMesh;
    public int Follow_Distance;
    private bool is_moving; 
=======
    Main m;
    public NavMeshAgent navMesh;
    public int Follow_Distance;
    private bool is_moving;
>>>>>>> Stashed changes
    private Animator my_animator;

    public bool isMoving()
    {
        return navMesh.remainingDistance <= navMesh.stoppingDistance;
    }

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< Updated upstream
        navMesh = this.GetComponent<NavMeshAgent>();
    	my_animator = this.GetComponent<Animator>();
    	is_moving = false;
=======
        m = Main.instance;
        navMesh = this.GetComponent<NavMeshAgent>();
        my_animator = this.GetComponent<Animator>();
        is_moving = false;
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
        double shortestDistance = 2000000;
<<<<<<< Updated upstream
	Transform closestCastle = null;

	for(var i = 0; i < Main.instance.castles.Length; i++)
	{
		double distance = Vector3.Distance(this.gameObject.transform.position, Main.instance.castles[i].transform.position);
	    	if(distance <= shortestDistance)
		{
		    shortestDistance = distance;
		    closestCastle = Main.instance.castles[i];
		}
	}

	if(shortestDistance <= Follow_Distance)
    	    navMesh.SetDestination(closestCastle.position);
    	is_moving = isMoving();
    	my_animator.SetBool("is_moving", is_moving);
=======
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

        if (shortestDistance <= Follow_Distance)
            navMesh.SetDestination(closestCastle.position);
        is_moving = isMoving();
        my_animator.SetBool("is_moving", is_moving);
>>>>>>> Stashed changes
    }
}