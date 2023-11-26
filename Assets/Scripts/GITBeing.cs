using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    [SerializeField] protected Transform closestTarget;
    [HideInInspector] public AttackType attackType; //AttackTypes for this being.
    [HideInInspector] public NavMeshAgent navMesh;//NavMeshAgent for this being.
    protected float pursueRange; //Pursuit range for enemies of this being.

    protected void Start()
    {
        m = Main.instance;
        navMesh = GetComponentInChildren<NavMeshAgent>();
        health = maxHealth;
        closestTarget = null;
    }

    protected virtual void Update()
    {
        if (health > 0)
        {
            //Debug.Log("One");
            if (aliveOfTag(enemyTag[0]))
            {
                //Debug.Log("Two");
                if (closestTarget = closestOfTag(enemyTag, pursueRange))
                {
                    //Debug.Log("Three");
                    //PATH TO ENEMIES
                    //Debug.Log(gameObject.name + ": closest object is an enemy -- " + closestTarget.name);
                    float distToEnemy = Vector3.Distance(transform.position, closestTarget.position);
                    status = (distToEnemy <= attackType.attackRange) ? BeingStatus.attack :
                        ((distToEnemy <= pursueRange) ? BeingStatus.chase : BeingStatus.regroup);
                    if (InHostileAction())
                    {
                        navMesh.SetDestination(closestTarget.position);
                        if (status == BeingStatus.attack) PrepareToAttack();
                    }
                }
            }
            else
            {
                //PATH TO ALLIES
                status = BeingStatus.regroup;
                closestTarget = closestOfTag(allyTag, pursueRange * 10);
            }
            //Debug.Log("closest object is " + closestTarget.gameObject);
            if(closestTarget != null) navMesh.SetDestination(closestTarget.position);
            else navMesh.SetDestination(transform.position);
            if (ShouldStop()) status = BeingStatus.idle;
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);//reset tilt
        }
    }
    protected bool InHostileAction() { return status >= BeingStatus.chase; }
    protected bool ShouldStop() { return navMesh.remainingDistance <= navMesh.stoppingDistance; }
    /// <summary>
    /// Deals damage to this being.
    /// </summary>
    /// <param name="dmg">Damage to take.</param>
    /// <returns>True if this being dies, false otherwise.</returns>
    public bool TakeDamage(float dmg)
    {
        UnityEngine.Debug.Log(gameObject.name + ": took " + dmg + " damage!! -- " + health + " / " + maxHealth + "remaining");
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
    protected Transform closestOfTag(string[] s, float maxDist = 100f)
    {
        GameObject chosen_one = null;
        float minDist = maxDist;
        foreach(string s1 in s)
        {
            GameObject[] tagged = GameObject.FindGameObjectsWithTag(s1);
            foreach (var e in tagged)
            {
                //Debug.Log("e!");
                float dist = Vector3.Distance(e.transform.position, transform.position);
                if (dist <= minDist)
                {
                    minDist = dist;
                    chosen_one = e;
                }
            }
        }
        
        if(chosen_one != null) { return chosen_one.transform; }
        return null;
    }
    /// <summary>
    /// Causes character to stand still and point towards target
    /// </summary>
    protected void PrepareToAttack()
    {
        navMesh.SetDestination(transform.position);
        transform.LookAt(closestTarget.transform);
    }


}