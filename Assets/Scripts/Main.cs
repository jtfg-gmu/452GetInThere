using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
	

	public static Main instance;
	[SerializeField] Transform[] castleSpawnLocations;
    [SerializeField] Transform soldierSpawnLocation;
	
	[SerializeField] int soldierCountMax;
	private int soldierCount;

	//Something similar can be implemented for enemy soldiers.

    private float nextWaveDefault = 3.0f;
    private float nextWaveTimer;

	void Awake()
	{
		instance = this;
	}

	// Start is called before the first frame update
	void Start()
	{
		nextWaveTimer = nextWaveDefault;
		soldierCount = 0;
		CreateCastles();
		SpawnWave();
	}

	// Update is called once per frame
	void Update()
	{
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
		if(castleSpawnLocations.Length == 0) { Debug.LogError("No castle locations initialized!"); return;  }
		for (var i = 0; i < castleSpawnLocations.Length; i++)
		{
			go = Instantiate(Resources.Load<GameObject>("Castle"), castleSpawnLocations[i].position, castleSpawnLocations[i].rotation);
			go.name = "Castle " + i;
			//other modifications here.
		}
	}

	//Begins spawning singleton soldiers after a delay.
	//Spawns up to soldierCountMax and then stops.
	private void SpawnWave(float delay = 1.0f) { InvokeRepeating("CreateSoldier", delay, 0.5f); }

	//Spawns a soldier.
	private void CreateSoldier()
	{
		if (soldierCount <= soldierCountMax) //if we can still spawn another soldier
		{
			GameObject.Instantiate(Resources.Load<GameObject>("Soldier"), soldierSpawnLocation.position, soldierSpawnLocation.rotation);
			soldierCount += 1;
		}
		else
		{
			Debug.Log("Skipped soldier spawn due to max number of soldiers");
			this.CancelInvoke();
		}
		
	}

    /*
 * Extendable Calls
 */

    //Used by other classes to signal when a soldier despawns (so we can spawn another).
    public void SignalDespawnSoldier() { soldierCount--; }
    //Used by other classes to signal when spawntimer should reset to default (i.e. after a lot of time)
    public void SignalResetSpawns() { nextWaveTimer = nextWaveDefault; }
}