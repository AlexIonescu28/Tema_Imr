using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
public class ScorableObject : MonoBehaviour
{
    [Min(0)] public int points = 10;
    public bool requireRelease = true;

    public bool IsHeld { get; private set; }
    public bool HasScored { get; private set; }
    public Vector3 ThrowOrigin { get; private set; }

    XRGrabInteractable _interactable;
    Rigidbody _rb;
    Vector3 _startPos;
    Quaternion _startRot;

    void Awake()
    {
        _interactable = GetComponent<XRGrabInteractable>();
        _rb = GetComponent<Rigidbody>();
        _startPos = transform.position;
        _startRot = transform.rotation;
    }

    void OnEnable()
    {
        _interactable.selectEntered.AddListener(OnGrab);
        _interactable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        _interactable.selectEntered.RemoveListener(OnGrab);
        _interactable.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args) => IsHeld = true;

    void OnRelease(SelectExitEventArgs args)
    {
        IsHeld = false;
        var cam = Camera.main;
        ThrowOrigin = cam != null ? cam.transform.position : transform.position;
    }

    public void MarkScored() => HasScored = true;

    public void ResetObject()
    {
        HasScored = false;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        transform.SetPositionAndRotation(_startPos, _startRot);
    }
}