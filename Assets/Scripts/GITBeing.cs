using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BeingStatus { idle, regroup, chase, attack }

public class GITBeing : MonoBehaviour
{
    protected Main m;
    [SerializeField] public float health;//Current health for this being.
    public int maxHealth = 50;//Maximum health for this being.
    [HideInInspector] public BeingStatus status;//Being's status (used for animation).
    [HideInInspector] public string[] allyTag;//Tag considered an ally by this being.
    [HideInInspector] public string[] enemyTag;//Tag considered an enemy by this being.
    protected Transform closestTarget;
    [HideInInspector] public AttackType attackType; //AttackTypes for this being.
    [HideInInspector] public NavMeshAgent navMesh;//NavMeshAgent for this being.
    protected float pursueRange = 100f; //Pursuit range for enemies of this being.

    protected void Start()
    {
        m = Main.instance;
        navMesh = GetComponentInChildren<NavMeshAgent>();
        health = maxHealth;
        closestTarget = transform; //targets self just so early updates don't null out
    }

    protected virtual void Update()
    {
        if (health > 0)
        {
            if (findClosest(enemyTag, pursueRange))
            {
                //PATH TO ENEMIES
                float distToEnemy = Vector3.Distance(transform.position, closestTarget.position);
                status = (distToEnemy <= attackType.attackRange) ? BeingStatus.attack :
                    ((distToEnemy <= pursueRange) ? BeingStatus.chase : BeingStatus.regroup);
                if (InHostileAction())
                {
                    navMesh.SetDestination(closestTarget.position);
                    if (status == BeingStatus.attack) PrepareToAttack();
                }
            }
            else
            {
                //PATH TO ALLIES
                status = BeingStatus.regroup;
                findClosest(allyTag, pursueRange * 10);
            }
            //Debug.Log("closest object is " + closestTarget.gameObject);
            navMesh.SetDestination(closestTarget.position);
            if (ShouldStop()) status = BeingStatus.idle;
        }
    }
    protected bool InHostileAction() { return status >= BeingStatus.chase; }
    protected bool ShouldStop() { return navMesh.remainingDistance <= navMesh.stoppingDistance; }

    bool findClosest(string[] strings, float maxDist)
    {
        if (strings != null) return false;//if the tag doesn't exist then chill out
        Transform oldCT = closestTarget;
        double distance, minDist;
        minDist = maxDist;
        foreach (string s in strings)
        {
            if (aliveOfTag(s))
            {
                Transform g = closestOfTag(s);
                if (g != null)
                {
                    distance = Vector3.Distance(transform.position, g.position);
                    if (distance < minDist) { minDist = distance; closestTarget = g; }//update target
                }
            }
        }
        if (oldCT != closestTarget) { return true; }
        return false;
    }
    /// <summary>
    /// Deals damage to this being.
    /// </summary>
    /// <param name="dmg">Damage to take.</param>
    /// <returns>True if this being dies, false otherwise.</returns>
    public bool TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0.0f)
        {
            Destroy(gameObject, 1f);
            return true;
        }
        return false;
    }
    /// <summary>
    /// Checks if any of this tag exist.
    /// </summary>
    /// <param name="s">Tag to check.</param>
    /// <returns>True if any exist, false otherwise.</returns>
    protected bool aliveOfTag(string s)
    {
        return GameObject.FindGameObjectsWithTag(s).Length > 0;
    }
    protected Transform closestOfTag(string s, float maxDist = 100f)
    {
        GameObject[] tagged = GameObject.FindGameObjectsWithTag(s);
        GameObject chosen_one = null;
        float minDist = maxDist;
        foreach (var e in tagged)
        {
            float dist = Vector3.Distance(e.transform.position, transform.position);
            if (dist <= minDist)
            {
                minDist = dist;
                chosen_one = e;
            }
        }

        return chosen_one.transform;
    }
    /// <summary>
    /// Causes character to stand still and point towards target
    /// </summary>
    protected void PrepareToAttack()
    {
        navMesh.SetDestination(transform.position);
        transform.LookAt(closestTarget.transform);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }


}