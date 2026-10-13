using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Collider))]
public class Basket : MonoBehaviour
{
    [System.Serializable]
    public struct Zone
    {
        [Tooltip("Distanta minima (m) de la jucator la cos pentru aceasta zona.")]
        public float minDistance;
        public int points;
    }

    [Header("Scor dupa distanta (ordoneaza crescator dupa distanta)")]
    public Zone[] zones =
    {
        new Zone { minDistance = 0f, points = 2 },   // aproape
        new Zone { minDistance = 3f, points = 3 },   // semi-departe
        new Zone { minDistance = 5f, points = 5 },   // foarte departe
    };

    [Header("Referinte")]
    public ScoreManager scoreManager;
    public ScorePopup popupPrefab;
    public ParticleSystem hitParticles;
    public AudioSource hitAudio;

    [Header("Feedback vizual")]
    public Renderer rimRenderer;
    public Color flashColor = Color.green;
    public float flashDuration = 0.6f;

    Color _baseColor;

    void Reset() => GetComponent<Collider>().isTrigger = true;

    void Awake()
    {
        if (rimRenderer != null) _baseColor = rimRenderer.material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        var obj = other.GetComponentInParent<ScorableObject>();
        if (obj == null || obj.HasScored || obj.IsHeld) return;
        var rb = obj.GetComponent<Rigidbody>();
        if (rb != null && rb.linearVelocity.y > 0f) return;

        obj.MarkScored();

        Vector3 a = Camera.main.transform.position; a.y = 0f;
        Vector3 b = transform.position; b.y = 0f;
        float distance = Vector3.Distance(a, b);

        int points = PointsFor(distance);
        if (scoreManager != null) scoreManager.AddScore(points);
        PlayFeedback(points, distance);
    }

    int PointsFor(float distance)
    {
        int result = zones.Length > 0 ? zones[0].points : 2;
        foreach (var z in zones)
            if (distance >= z.minDistance) result = z.points;
        return result;
    }

    void PlayFeedback(int points, float distance)
    {
        if (hitParticles != null) hitParticles.Play();
        if (hitAudio != null) hitAudio.Play();
        if (popupPrefab != null)
        {
            var p = Instantiate(popupPrefab, transform.position + Vector3.up * 0.3f, Quaternion.identity);
            p.Show($"+{points}  ({distance:0.0} m)");
        }
        StopAllCoroutines();
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        if (rimRenderer == null) yield break;
        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            rimRenderer.material.color = Color.Lerp(flashColor, _baseColor, t / flashDuration);
            yield return null;
        }
        rimRenderer.material.color = _baseColor;
    }
}