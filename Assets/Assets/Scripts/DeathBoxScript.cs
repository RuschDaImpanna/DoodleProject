using UnityEngine;

public class DeathBoxScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("TRIGGER DETECTED");
        Debug.Log(other.name);

        Debug.Log("Entered trigger with: " + other.gameObject.name);

        // Optional: Filter by a specific tag
        if (other.CompareTag("Player"))
        {
            Debug.Log("The Player entered the zone!");
        }
    }
    
}
