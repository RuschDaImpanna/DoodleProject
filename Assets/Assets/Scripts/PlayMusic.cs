using UnityEngine;

public class PlayMusic : MonoBehaviour
{

    public AudioSource musicPlayer;
    
    public void playMusic(AudioClip music)
    {
    
        musicPlayer.clip = music;
        musicPlayer.Play();
        
    }

}
