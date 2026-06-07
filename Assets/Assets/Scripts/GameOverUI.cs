using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{

    public TextMeshProUGUI scoreTxt;
    public PlayMusic musicHandler;
    public AudioClip gameOverMusic;
    
    public void setup(int score)
    {

        gameObject.SetActive(true);
        scoreTxt.text = $"{score:D6}";

        musicHandler.playMusic(gameOverMusic);
        
    }

    public void restartBtn ()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void menuBtn ()
    {
        
        SceneManager.LoadScene("MainMenu");

    }

}
