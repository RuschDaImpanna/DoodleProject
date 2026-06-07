using UnityEngine;

public class DeathBoxScript : MonoBehaviour
{

    public AudioClip fallSfx;
    public PlayerCharacter player;

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("TRIGGER DETECTED");
        Debug.Log(other.name);

        Debug.Log("Entered trigger with: " + other.gameObject.name);
        
        if (other.CompareTag("Enviroment")) return;

        Debug.Log($"ENTER: {other.name} | TAG: {other.tag} | FRAME: {Time.frameCount}");


        // Optional: Filter by a specific tag
        if (other.CompareTag("Player"))
        {
            player.healthAssign(-5);
            player.playSfx(fallSfx);
        }

        Destroy(other, 0.3f);

    }
    
}
