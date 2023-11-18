using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minionHitDection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.collider);
        if (other.collider.material.name.Equals("door"))
        {
            Debug.Log("collide with door");
        }
    }
}
