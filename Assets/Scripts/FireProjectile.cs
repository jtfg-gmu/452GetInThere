using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    private int numBullet;
    private FollowComponent soldierRef;
    private bool alreadyShootBullet;
    void Start()
    {
        soldierRef = transform.parent.gameObject.GetComponent<FollowComponent>();
        numBullet = 0;
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
                Invoke(nameof(ResetAttack),3f);
            }
            
        } 
    }

    private void ResetAttack()
    {
        alreadyShootBullet = false;
    }

    private void CreateProjectile()
    {
        GameObject projectTile = GameObject.Instantiate(Resources.Load<GameObject>("projectile"),transform.position,transform.rotation);
        projectTile.GetComponent<Rigidbody>().velocity = transform.forward * 3f;
    }
}
