using UnityEngine;
using UnityEngine.AI;

public class SoldierBehaviour : GITBeing
{
    private Animator my_animator;
    private GameObject player;

    // Start is called before the first frame update
    new void Start()
    {
        
        attackType = gameObject.AddComponent<AttackProjectile>();
        allyTag = new string[] { "soldier", "Castle", "Player" };
        enemyTag = new string[] { "enemy" };
        my_animator = this.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        maxHealth = 50;
        pursueRange = 50;
        base.Start();

    }
    void OnDestroy()
    {
        m.SignalDespawnSoldier();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update(); 
        my_animator.SetBool("is_moving", status == BeingStatus.idle);
    }

    protected override void PrepareToAttack()
    {
        navMesh.stoppingDistance = 3.2f;
        navMesh.SetDestination(transform.position);
        transform.LookAt(closestTarget.transform);
    }
}