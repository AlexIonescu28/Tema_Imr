using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Collider))]
public class ScoreTarget : MonoBehaviour
{
    [Header("Referinte")]
    public ScoreManager scoreManager;
    public ScorePopup popupPrefab;
    public ParticleSystem hitParticles;
    public AudioSource hitAudio;

    [Header("Feedback vizual")]
    public Renderer targetRenderer;
    public Color flashColor = Color.green;
    public float flashDuration = 0.5f;
    public float pulseScale = 1.25f;

    [Tooltip("Multiplicator de scor pentru aceasta tinta (ex. 1 = centru ratat, 3 = bullseye).")]
    public int multiplier = 1;

    Color _baseColor;
    Vector3 _baseScale;

    void Reset() => GetComponent<Collider>().isTrigger = true;

    void Awake()
    {
        if (targetRenderer == null) targetRenderer = GetComponentInChildren<Renderer>();
        if (targetRenderer != null) _baseColor = targetRenderer.material.color;
        _baseScale = transform.localScale;
    }

    void OnTriggerEnter(Collider other) => TryScore(other);
    void OnTriggerStay(Collider other) => TryScore(other); 
    void TryScore(Collider other)
    {
        var obj = other.GetComponentInParent<ScorableObject>();
        if (obj == null || obj.HasScored) return;
        if (obj.requireRelease && obj.IsHeld) return;

        obj.MarkScored();
        int total = obj.points * multiplier;

        if (scoreManager != null) scoreManager.AddScore(total);
        SpawnFeedback(total, obj.transform.position);
    }

    void SpawnFeedback(int points, Vector3 worldPos)
    {
        if (hitParticles != null) hitParticles.Play();
        if (hitAudio != null) hitAudio.Play();
        if (popupPrefab != null)
        {
            var popup = Instantiate(popupPrefab, worldPos + Vector3.up * 0.2f, Quaternion.identity);
            popup.Show($"+{points}");
        }
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            float k = 1f - Mathf.Clamp01(t / flashDuration);   
            if (targetRenderer != null)
                targetRenderer.material.color = Color.Lerp(_baseColor, flashColor, k);
            transform.localScale = Vector3.Lerp(_baseScale, _baseScale * pulseScale, Mathf.Sin(k * Mathf.PI));
            yield return null;
        }
        if (targetRenderer != null) targetRenderer.material.color = _baseColor;
        transform.localScale = _baseScale;
    }
}