using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    
    public void UpdateScoreText(int currentScore)
    {
        scoreText.text = "Scor: " + currentScore;
    }
}