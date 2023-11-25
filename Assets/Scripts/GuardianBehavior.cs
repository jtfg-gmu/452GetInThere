using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GuardianBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshAgent agent;
    public int health;
    private float tauntSearchRange;
    private float tauntAttackRange;
    private GameObject tauntAOEObj;
    private bool releaseAOE;
    private float skillCoolDown;
    private float curSkillTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 2f;
        health = 230;
        tauntSearchRange = 12f;
        tauntAttackRange = 3f;
        tauntAOEObj = transform.GetChild(2).gameObject;
        releaseAOE = false;
        skillCoolDown = 2f;
        curSkillTime = 0;
    ;
}

    // Update is called once per frame
    void Update()
    {
        if (isEnemyAlive())
        {
            Transform target = getClosestEnemy();
            float dist = Vector3.Distance(transform.position, target.position);
            if (dist <= tauntSearchRange && !(dist <= tauntAttackRange))
            {
                Debug.Log("guardian is moving toward " + target);
                agent.SetDestination(target.position);
            }

            if (dist <= tauntAttackRange && dist <= tauntSearchRange)
            {
                
                agent.SetDestination(transform.position);
                if (!releaseAOE)
                {
                    curSkillTime = 0;
                    releaseAOE = true;
                    StartCoroutine(tauntAOE());
                }
                
            }
            if (!(dist <= tauntAttackRange) && !(dist <= tauntSearchRange))
            {
                agent.SetDestination(transform.position);
            }
        }

        else
        {
            agent.SetDestination(transform.position);
        }
        AOECoolDown();
    }

    private bool isEnemyAlive()
    {
        return GameObject.FindGameObjectsWithTag("enemy").Length > 0;
    }

    private IEnumerator tauntAOE()
    {
        yield return new WaitForSeconds(0.75f);
        tauntAOEObj.SetActive(true);
        Collider[] collides = Physics.OverlapSphere(transform.position, 7f);
        foreach (Collider c in collides)
        {
            if (c.gameObject.CompareTag("enemy"))
            {
                EnemyAIMove enemy = c.gameObject.GetComponent<EnemyAIMove>();
                enemy.isTaunt = true; 
            }
        }
        yield return new WaitForSeconds(1f);
        tauntAOEObj.SetActive(false);
    }

    private void AOECoolDown()
    {
        curSkillTime += Time.deltaTime;
        if (curSkillTime >= skillCoolDown)
        {
            releaseAOE = false;
        }
    }

    private Transform getClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        Transform closestEnemy = null;
        float minDist = 1000000;
        foreach (var e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist <= minDist)
            {
                minDist = dist;
                closestEnemy = e.transform;
            }
        }

        return closestEnemy;
    }
}
