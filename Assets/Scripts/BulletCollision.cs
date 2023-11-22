using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("enemy"))
        {
            EnemyAIMove enemyAIMove = other.gameObject.GetComponent<EnemyAIMove>();
            if (enemyAIMove.health > 0)
            {
                enemyAIMove.health -= 2;
                if (enemyAIMove.health == 0)
                {
                    Destroy(other.gameObject);
                }
            }
            Debug.Log("hit enemy");
            Destroy(gameObject);
        }
        
    }

    // private void O(Collision other)
    // {
    //     
    // }
}
