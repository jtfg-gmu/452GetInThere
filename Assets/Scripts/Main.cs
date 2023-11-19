using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
	public static Main instance;
	public Transform[] castles;
	public Transform Soldier_Spawn;
	public int Number_of_allied_soldiers;
	private int ctr;

	void Awake()
	{
		Main.instance = this;
	}

	// Start is called before the first frame update
	void Start()
	{
		ctr = 0;
		CreateCastles();
		InvokeRepeating("CreateSoldiers", 1f, 3f);
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void CreateCastles()
	{
		for (var i = 0; i < 4; i++)
		{
			GameObject.Instantiate(Resources.Load<GameObject>("Castle"), castle.position, castle.rotation);
		}
	}

private void CreateSoldiers()
	{
		if (ctr <= Number_of_allied_soldiers)
		{
			GameObject.Instantiate(Resources.Load<GameObject>("Soldier"), Soldier_Spawn.position, Soldier_Spawn.rotation);
			ctr += 1;
		}
		
	}
}