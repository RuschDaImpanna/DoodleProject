using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoringScript : MonoBehaviour
{

    public TextMeshProUGUI text;

    public void updateScore(int score)
    {
        
        text.text = $"{score:D6}";

    }

}
