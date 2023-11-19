using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
	public static Main instance;
	public Transform castle;
	public Transform Soldier_Spawn;
	public int Number_of_allied_soldiers;

	void Awake()
	{
		Main.instance = this;
	}

	// Start is called before the first frame update
	void Start()
	{

		GameObject.Instantiate(Resources.Load<GameObject>("Castle"), castle.position, castle.rotation);
		InvokeRepeating("InstantiateEntities", 5f, 5f);
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void InstantiateEntities()
	{
		GameObject.Instantiate(Resources.Load<GameObject>("Soldier"), Soldier_Spawn.position, Soldier_Spawn.rotation);
	}
}