using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIMove : MonoBehaviour
{
    // Start is called before the first frame update
    public NavMeshAgent navMesh;
    Main m;
    private Transform[] castleLocations;
    private Boolean search;
    public int health;
    private GameObject explosionAOE;
    private bool canDoDamage;
    private float curAttackTime;
    private float attackTime;
    private float soldierSearchRange;
    private float soldierAttackRange;
    private bool soldierInSearchRange;
    private bool soldierInAttackRange;
    public bool isTaunt;
    private bool canAttackGuardian;
    void Start()
    {
        m = Main.instance;
        castleLocations = m.castleLocations; 
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.stoppingDistance = 0.5f;
        explosionAOE = transform.GetChild(2).gameObject;
        explosionAOE.SetActive(false);
        search = true;
        health = 150;
        canDoDamage = false;
        curAttackTime = 0f;
        attackTime = 1.75f;
        soldierSearchRange = 15f;
        soldierAttackRange = 1f;
        soldierInSearchRange = false;
        soldierInAttackRange = false;
        canAttackGuardian = false;
        isTaunt = false;

    }

    private Boolean reachAndStay()
    {
        return navMesh.remainingDistance <= navMesh.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTaunt)
        {
            Debug.Log("enemy distance to gaurdian " + navMesh.remainingDistance);
            Transform guardian = tauntingGuardian();
            if (!canAttackGuardian)
            {
                float dist = Vector3.Distance(transform.position, guardian.position);
                navMesh.SetDestination(guardian.position);
                if (dist <= 1)
                {
                    canAttackGuardian = true;
                }
            }

            if (canAttackGuardian)
            {
               
                navMesh.SetDestination(transform.position);
                if (!canDoDamage)
                {
                    canDoDamage = true;
                    curAttackTime = 0f;
                    StartCoroutine(doAOEDmg());
                }
                Invoke("removeTauntEffect",6f);
            }
            attackCoolDown();
        }
        else
        {
            if (!attackNearbySoldiers())
            {
                if (search)
                {
                    randomlyAttack();
                    search = false;
                }

                else if (search == false)
                {
                    if (reachAndStay() && !canDoDamage)
                    {
                        canDoDamage = true;
                        curAttackTime = 0f;
                        StartCoroutine(doAOEDmg());
                        Invoke("attackThenChange",5f);
                    }
                }
        
                attackCoolDown();
            }
        }
        
        
    }

    private bool attackNearbySoldiers()
    {
        GameObject[] soldiers = GameObject.FindGameObjectsWithTag("soldier");
        if (soldiers.Length == 0)
        {
            return false;
        }
        float dist = 1000000;
        Transform closestSoldier = null;
        foreach (var soldier in soldiers)
        {
            float cur = Vector3.Distance(transform.position, soldier.transform.position);
            if (cur <= dist)
            {
                dist = cur;
                closestSoldier = soldier.transform;
            }
        }

        if (closestSoldier is null)
        {
            soldierInSearchRange = false;
            soldierInAttackRange = false;
            return false;
        }

        soldierInSearchRange = dist <= soldierSearchRange;
        soldierInAttackRange = dist <= soldierAttackRange;

        if (soldierInSearchRange && !soldierInAttackRange)
        {
            navMesh.SetDestination(closestSoldier.position);
            return true;
        }

        if (!soldierInAttackRange && !soldierInSearchRange)
        {
            soldierInSearchRange = false;
            soldierInAttackRange = false;
            return false;
        }

        if (soldierInAttackRange && soldierInSearchRange && !canDoDamage)
        {
            canDoDamage = true;
            navMesh.SetDestination(transform.position);
            curAttackTime = 0f;
            StartCoroutine(doAOEDmg());
            Invoke("attackThenChange",5f);
            attackCoolDown();
            return true;
        }
        return false;
    }

    private void attackCoolDown()
    {
        curAttackTime += Time.deltaTime;
        if (curAttackTime >= attackTime)
        {
            canDoDamage = false;
        }
    }

    private void removeTauntEffect()
    {
        isTaunt = false;
        canAttackGuardian = false;
    }
    

    private IEnumerator doAOEDmg()
    {
        yield return new WaitForSeconds(1f);
        explosionAOE.SetActive(true);
        Collider[] collides = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider c in collides)
        {
            if (c.gameObject.CompareTag("soldier"))
            {
                FollowComponent followComponent = c.gameObject.GetComponent<FollowComponent>();
                followComponent.health -= 3;
                if (followComponent.health <= 0)
                {
                    Destroy(c.gameObject);
                }
            }

            if (c.gameObject.CompareTag("guardian"))
            {
                GuardianBehavior guardianBehavior = c.gameObject.GetComponent<GuardianBehavior>();
                guardianBehavior.health -= 3;
                if (guardianBehavior.health <= 0)
                {
                    isTaunt = false;
                    attackThenChange();
                    Destroy(c.gameObject);
                }
            }
        }
        yield return new WaitForSeconds(1f);
        explosionAOE.SetActive(false);
    }

    private void randomlyAttack()
    { 
        navMesh.SetDestination(closestCastle().position);
    }

    private Transform closestCastle()
    {
        Transform closestTar = null;
        float dist = 1000000;
        foreach (var tower in castleLocations)
        {
            float cur = Vector3.Distance(transform.position, tower.transform.position);
            if (cur <= dist)
            {
                dist = cur;
                closestTar = tower.transform;
            }
        }

        return closestTar;
    }

    private Transform tauntingGuardian()
    {
        Transform closestTar = null;
        GameObject[] guardians = GameObject.FindGameObjectsWithTag("guardian");
        float dist = 1000000;
        foreach (var guardian in guardians)
        {
            float cur = Vector3.Distance(transform.position, guardian.transform.position);
            if (cur <= dist)
            {
                dist = cur;
                closestTar = guardian.transform;
            }
        }

        return closestTar;
    }

    private void attackThenChange()
    {
        search = true;
        explosionAOE.SetActive(false);
        canDoDamage = false;
        curAttackTime = 0;
    }
}
