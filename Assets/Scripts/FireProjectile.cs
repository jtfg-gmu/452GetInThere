using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    private int numBullet;
    void Start()
    {
        numBullet = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // InvokeRepeating("CreateProjectile",1f,6f);
        if (numBullet == 0)
        {
            CreateProjectile();
            numBullet += 1;
        }
    }

    private void CreateProjectile()
    {
        GameObject projectTile = GameObject.Instantiate(Resources.Load<GameObject>("projectile"),transform.position,transform.rotation);
        projectTile.GetComponent<Rigidbody>().velocity = transform.forward * 3;
    }
}
