using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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

        if (other.gameObject.CompareTag("door"))
        {
            Debug.Log("door");
            // foreach (ContactPoint contactPoint in other.contacts)
            // {
            //     GameObject collideObj = other.gameObject;
            //     Renderer renderder = collideObj.GetComponent<Renderer>();
            //     List<Material> materials = renderder.materials.ToList();
            //     foreach (Material t in materials)
            //     {
            //         Debug.Log(t.name);
            //     }
            //     
            // }
            
            
        }
        else if (other.gameObject.CompareTag("castle"))
        {
            Debug.Log("castle");
        }
        
    }
}
