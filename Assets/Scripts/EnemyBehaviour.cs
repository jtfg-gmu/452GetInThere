using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : GITBeing
{
    // Start is called before the first frame update
    private Boolean search = true;
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
    protected new void Start()
    {
        attackType = gameObject.AddComponent<AttackAOE>();
        explosionAOE = gameObject.transform.GetChild(2).gameObject;
        explosionAOE.SetActive(false);
        GetComponent<AttackAOE>().waveDisplay = explosionAOE;
        pursueRange = 15f;
        countdownmax = 1;
        allyTag = new string[] { "enemy", "Castle" };
        enemyTag = new string[] { "soldier" };
        base.Start();
        navMesh.stoppingDistance = 0.5f;
        search = true;
        canDoDamage = false;
        curAttackTime = 0f;
        attackTime = 1.75f;
        
        
        canAttackGuardian = false;
        isTaunt = false;

    }

    // Update is called once per frame
    protected void LateUpdate()
    {
        if(attackType.status == AttackStatus.wait)
        {
            if(attackType.lastAttacked.Any(x=> x != null && x.CompareTag("guardian"))){
                attackThenChange();
            }
        }
        /*if (isTaunt)
        {
            //Debug.Log("enemy distance to gaurdian " + navMesh.remainingDistance);
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

                else
                {
                    if (ShouldStop() && !canDoDamage)
                    {
                        canDoDamage = true;
                        curAttackTime = 0f;
                        StartCoroutine(doAOEDmg());
                        Invoke("attackThenChange",5f);
                    }
                }
        
                attackCoolDown();
            }
        }*/
        
        
    }

    private bool attackNearbySoldiers()
    {
        return false;
        /*Transform closestSoldier = closestOfTag("soldier",soldierSearchRange);

        if (closestSoldier is null)
        {
            soldierInSearchRange = false;
            soldierInAttackRange = false;
            return false;
        }
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
        return false;*/
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
        status = BeingStatus.attack;
        yield return new WaitForSeconds(1f);
        explosionAOE.SetActive(false);
    }

    /*private void randomlyAttack()
    { 
        navMesh.SetDestination(closestCastle().position);
    }

    private Transform closestCastle()
    {

        return closestOfTag("castle", 100000);
    }

    private Transform tauntingGuardian()
    {
        return closestOfTag("guardian", 1000000);
    }*/

    private void attackThenChange()
    {
        search = true;
        explosionAOE.SetActive(false);
        canDoDamage = false;
        curAttackTime = 0;
    }
}
