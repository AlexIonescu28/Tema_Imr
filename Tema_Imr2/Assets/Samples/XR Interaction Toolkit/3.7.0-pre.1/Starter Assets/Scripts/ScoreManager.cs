using UnityEngine;
using TMPro; // Obligatoriu pentru TextMeshPro

public class ScoreManager : MonoBehaviour
{
    private int Score = 0;

    
    public TextMeshProUGUI scoreText;

    public void addScore(int scoreToAdd)
    {
        Score = Score + scoreToAdd;

       
        scoreText.text = "Scor: " + Score;
    }
}