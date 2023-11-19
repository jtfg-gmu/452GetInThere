using UnityEngine;

public class DoorSoliderHitDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public int health = 50;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("soldier"))
        {
            Debug.Log("door");
            health -= 10;
            Debug.Log(health);
        }
    }
}
