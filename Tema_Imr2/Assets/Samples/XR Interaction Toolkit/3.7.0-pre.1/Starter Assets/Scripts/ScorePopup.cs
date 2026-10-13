using TMPro;
using UnityEngine;
public class ScorePopup : MonoBehaviour
{
    public TMP_Text label;
    public float lifetime = 1.2f;
    public float riseSpeed = 0.5f;

    float _t;
    Camera _cam;

    void Awake()
    {
        _cam = Camera.main;
        if (label == null) label = GetComponentInChildren<TMP_Text>();
    }

    public void Show(string text)
    {
        if (label != null) label.text = text;
    }

    void Update()
    {
        _t += Time.deltaTime;
        transform.position += Vector3.up * (riseSpeed * Time.deltaTime);

        if (_cam != null)
            transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);

        if (label != null)
        {
            var c = label.color;
            c.a = 1f - Mathf.Clamp01(_t / lifetime);
            label.color = c;
        }

        if (_t >= lifetime) Destroy(gameObject);
    }
}