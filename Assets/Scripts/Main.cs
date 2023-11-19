using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
	public static Main instance;
<<<<<<< Updated upstream
	public Transform[] castles;
	public Transform Soldier_Spawn;
	public int Number_of_allied_soldiers;
	private int soldierCtr;
=======
	[SerializeField] public Transform[] castleLocations;
    [SerializeField] Transform soldierSpawnLocation;
	
	[SerializeField] int soldierCountMax;
	private int soldierCount;

	//Something similar can be implemented for enemy soldiers.

    private float nextWaveDefault = 3.0f;
    private float nextWaveTimer;
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
		
=======
		if (!IsInvoking("CreateSoldier"))
		{
			//if failed spawn wave, make checking timer longer until a change in gamestate occurs
			nextWaveTimer += nextWaveDefault;
			SpawnWave(nextWaveTimer);
		}
	}
	

	//Spawns castles (at the start of the game).
	private void CreateCastles()
	{
		GameObject go;
		if(castleLocations.Length == 0) { Debug.LogError("No castle locations initialized!"); return;  }
		for (var i = 0; i < castleLocations.Length; i++)
		{
			go = Instantiate(Resources.Load<GameObject>("Castle"), castleLocations[i].position, castleLocations[i].rotation);
			go.name = "Castle " + i;
			//other modifications here.
		}
>>>>>>> Stashed changes
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