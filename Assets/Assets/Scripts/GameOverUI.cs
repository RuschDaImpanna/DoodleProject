using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{

    public TextMeshProUGUI scoreTxt;
    
    public void setup(int score)
    {

        gameObject.SetActive(true);
        scoreTxt.text = $"{score:D6}";
        
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
