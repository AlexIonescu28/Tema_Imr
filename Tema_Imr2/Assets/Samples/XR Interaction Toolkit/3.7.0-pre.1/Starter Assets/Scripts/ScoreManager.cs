using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text scoreLabel;
    public string format = "Scor: {0}";

    [Header("Feedback UI")]
    public float punchScale = 1.3f;
    public float punchDuration = 0.25f;

    public int Score { get; private set; }

    float _punchT = 1f;
    Vector3 _labelBaseScale = Vector3.one;

    void Awake()
    {
        if (scoreLabel != null) _labelBaseScale = scoreLabel.transform.localScale;
        Refresh();
    }

    public void AddScore(int amount)
    {
        Score += amount;
        Refresh();
        _punchT = 0f;
    }

    public void ResetScore()
    {
        Score = 0;
        Refresh();
    }

    void Update()
    {
        if (scoreLabel == null || _punchT >= 1f) return;
        _punchT += Time.deltaTime / punchDuration;
        float k = Mathf.Sin(Mathf.Clamp01(_punchT) * Mathf.PI);
        scoreLabel.transform.localScale = _labelBaseScale * Mathf.Lerp(1f, punchScale, k);
    }

    void Refresh()
    {
        if (scoreLabel != null) scoreLabel.text = string.Format(format, Score);
    }
}