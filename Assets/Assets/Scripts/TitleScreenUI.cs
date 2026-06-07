using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour
{
    
    public PlayMusic musicHandler;
    public AudioClip titleScreenMusic;
    
    void Awake()
    {

        musicHandler.playMusic(titleScreenMusic);
        
    }

    public void playGame ()
    {
        
        SceneManager.LoadScene("GameRun");

    }
    
    /*public void credits ()
    {
        
        SceneManager.LoadScene("Credits");

    }*/
    

    public void exitGame ()
    {
        
        Application.Quit();

    }

}
