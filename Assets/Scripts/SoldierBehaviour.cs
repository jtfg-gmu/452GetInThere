using UnityEngine;
using UnityEngine.AI;

public class SoldierBehaviour : GITBeing
{
    private Animator my_animator;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
        attackType = gameObject.AddComponent<AttackProjectile>();
        allyTag = new string[] { "soldier", "Castle", "Player" };
        enemyTag = new string[] { "enemy" };
        my_animator = this.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        maxHealth = 50;
        pursueRange = 15f;
        base.Start();

    }
    void OnDestroy()
    {
        m.SignalDespawnSoldier();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update(); 
        my_animator.SetBool("is_moving", status == BeingStatus.idle);
    }
}