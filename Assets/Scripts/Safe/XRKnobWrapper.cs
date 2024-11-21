using UnityEngine;

public class XRKnobWrapper : MonoBehaviour
{
    public event System.Action<Transform> OnGrabbed;
    public event System.Action OnReleased;

    [SerializeField] XRKnob knob; // Reference to the XRKnob.

    void Start()
    {
        if (knob == null)
        {
            Debug.LogError("[XRKnobWrapper] XRKnob is not assigned.");
            enabled = false;
            return;
        }
    }

    public void Grabbed(Transform hand)
    {
        OnGrabbed?.Invoke(hand);
        Debug.Log($"[XRKnobWrapper] Hand grabbed: {hand.name}");
    }

    public void Released()
    {
        OnReleased?.Invoke();
        Debug.Log("[XRKnobWrapper] Hand released.");
    }
}
