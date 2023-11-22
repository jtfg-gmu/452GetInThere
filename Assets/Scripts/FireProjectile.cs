using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    private int numBullet;
    private FollowComponent soldierRef;
    private bool alreadyShootBullet;
    private float fireRate;
    private float fireTime;
    void Start()
    {
        soldierRef = transform.parent.gameObject.GetComponent<FollowComponent>();
        numBullet = 0;
        fireRate = 0.5f;
        fireTime = -1f;
        alreadyShootBullet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (soldierRef.isAttacking)
        {
            Debug.Log("attack, fire bullet");
            if (!alreadyShootBullet)
            {
                alreadyShootBullet = true;
                CreateProjectile();
                Invoke(nameof(ResetAttack),0.5f);
            }
            
        } 
    }

    private void ResetAttack()
    {
        alreadyShootBullet = false;
    }

    private void CreateProjectile()
    {
        if (Time.time > fireTime)
        {
            fireTime = Time.time * fireRate;
            GameObject projectTile = GameObject.Instantiate(Resources.Load<GameObject>("projectile"),transform.position,transform.rotation);
            projectTile.GetComponent<Rigidbody>().velocity = transform.forward * 20f;
            
        }
        
    }
}
