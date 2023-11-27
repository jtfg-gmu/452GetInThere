using System.Collections;
using UnityEngine;


public enum AttackStatus { attacking, wait, ready }
public abstract class AttackType : MonoBehaviour
{
    protected GITBeing user;
    public Collider[] lastAttacked;
    public AttackStatus status;
    public float attackRange;
    public float dmg = 3.0f;
    public float attackTime = 1.0f;
    public float reloadTime = 1.0f;
    public int eliminated;
    protected virtual void Start()
    {
        user = transform.gameObject.GetComponent<GITBeing>();
        this.status = AttackStatus.ready;
    }

    protected void LateUpdate()
    {
        if (user.status == BeingStatus.attack && this.status == AttackStatus.ready)
        {
            StartCoroutine(atkLoop());
        }
    }

    private IEnumerator atkLoop()
    {
        Attack();
        status = AttackStatus.wait;
        yield return new WaitForSeconds(attackTime);
        Invoke("ResetAttack", reloadTime);
    }
    protected abstract void Attack();
    protected virtual void ResetAttack()
    {
        eliminated = 0;
        status = AttackStatus.ready;
    }
}

public class AttackAOE : AttackType
{
    public GameObject waveDisplay;
    protected override void Start()
    {
        attackRange = 1f; // half of what we want so that we get closer to do AOE damage
        dmg = 4.0f;
        reloadTime = 3f;
        attackTime = 0.5f;
        base.Start();
    }
    protected override void Attack()
    {
        waveDisplay.SetActive(true);
        Collider[] collides = Physics.OverlapSphere(transform.position, 2 * attackRange);
        lastAttacked = collides;
        foreach (Collider c in collides)
        {
            GITBeing gb;
            foreach (string s in user.enemyTag)
            {
                if (c.CompareTag(s) && (gb = c.GetComponent<GITBeing>()) != null)
                {
                    if (gb.TakeDamage(dmg)) //Deal damage//if it's destroyed
                    {
                        eliminated++;
                    }
                }
            }
        }
    }
    protected override void ResetAttack()
    {
        waveDisplay.SetActive(false);
        base.ResetAttack();
    }
}

public class AttackProjectile : AttackType
{
    private GameObject bulletPrefab;
    protected override void Start()
    {
        base.Start();
        bulletPrefab = Resources.Load<GameObject>("projectile");
        attackRange = 15f;
        reloadTime = 3f;
        attackTime = 0.5f;
    }

    protected override void Attack()
    {
        GameObject projectile = GameObject.Instantiate(bulletPrefab,transform.position,transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = transform.forward * 20f;
    }
}
