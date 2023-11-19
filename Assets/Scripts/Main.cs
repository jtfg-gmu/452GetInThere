using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
	public static Main instance;
	public Transform[] castles;
	public Transform Soldier_Spawn;
	public int Number_of_allied_soldiers;
	private int soldierCtr;

	void Awake()
	{
		Main.instance = this;
	}

	// Start is called before the first frame update
	void Start()
	{
		soldierCtr = 0;
		SpawnCastles();
		InvokeRepeating("SpawnSoilders", 1f, 3f);
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void SpawnSoilders()
	{
		if (soldierCtr <= Number_of_allied_soldiers)
		{
			GameObject.Instantiate(Resources.Load<GameObject>("Soldier"), Soldier_Spawn.position, Soldier_Spawn.rotation);
			soldierCtr += 1;
		}
		
	}

	private void SpawnCastles()
	{
		for (var i = 0; i < castles.Length; i++)
		{
			GameObject.Instantiate(Resources.Load<GameObject>("Castle"), castles[i].position, castles[i].rotation);
		}
		
		
	}
}