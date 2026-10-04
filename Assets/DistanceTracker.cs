using UnityEngine;
using Vuforia;
public class DistanceTracker : MonoBehaviour
{
    public ObserverBehaviour target1;
    public ObserverBehaviour target2;
    float previousDistance = -1f;
    bool IsTracked(ObserverBehaviour t)
    {
        return t.TargetStatus.Status == Status.TRACKED ||
               t.TargetStatus.Status == Status.EXTENDED_TRACKED;
    }
    void Update()
    {
        if (!IsTracked(target1) || !IsTracked(target2))
        {
            previousDistance = -1f;
            return;
        }
        float distance = Vector3.Distance(target1.transform.position,
                                          target2.transform.position);

        if (previousDistance >= 0f)
        {
            float delta = distance - previousDistance;
            if (delta < -0.001f)
                Debug.Log("Se apropie: " + distance.ToString("F3") + " m");
            else if (delta > 0.001f)
                Debug.Log("Se depărtează: " + distance.ToString("F3") + " m");
        }
        previousDistance = distance;
    }
}