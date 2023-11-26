using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Main : MonoBehaviour
{


    public static Main instance;
    [SerializeField] Transform player;
    [SerializeField] public Transform[] castleLocations;
    [SerializeField] Transform soldierSpawnLocation;
    [SerializeField] Transform soldierTop;
    [SerializeField] int soldierCountMax;
    private int soldierCount;
    private GameObject soldierFab;
    
    private XRInteractionManager xrim;

    //Something similar can be implemented for enemy soldiers.

    private float nextWaveDefault = 3.0f;
    private float nextWaveTimer;

    void Awake()
    {
        soldierFab = Resources.Load<GameObject>("Soldier");
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        nextWaveTimer = nextWaveDefault;
        soldierCount = 0;
        if(!(xrim = FindObjectOfType<XRInteractionManager>())) Debug.LogError("MAIN: Can't find XRIM!");
        if (castleLocations.Length == 0) { Debug.LogError("MAIN: No castle locations initialized!"); return; }
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
        
        for (var i = 0; i < castleLocations.Length; i++)
        {
            go = Instantiate(Resources.Load<GameObject>("Castle"), castleLocations[i].position, castleLocations[i].rotation);
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
            
            GameObject soldier = GameObject.Instantiate(soldierFab, soldierSpawnLocation, true);
            soldier.transform.position += new Vector3(0, 2, 0);
            soldier.transform.parent = soldierTop;
            soldier.name = "Soldier " + soldierCount;
            soldier.GetComponent<XRGrabInteractable>().interactionManager = xrim;
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