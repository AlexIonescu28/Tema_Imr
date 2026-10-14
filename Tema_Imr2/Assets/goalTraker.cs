using UnityEngine;

public class goalTracker : MonoBehaviour
{
    
    public ScoreManager scoreManager;

    
    public int points = 1;

    private void OnTriggerEnter(Collider other)
    {
        scoreManager.addScore(points);
        Debug.Log("cos");

    }
}