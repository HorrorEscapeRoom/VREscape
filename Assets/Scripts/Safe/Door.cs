using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform hinge; // The door's hinge point.
    [SerializeField] float maxOpenAngle = 90f; // Maximum door open angle.
    [SerializeField] float rotationSpeed = 5f; // Smooth rotation speed.
    [SerializeField] SoundManager soundManager;
    [SerializeField] XRKnobWrapper knobWrapper; // Reference to the XRKnobWrapper.

    private Transform hand; // The hand object controlling the door.
    private Quaternion closedRotation; // Initial closed position of the door.

    void Start()
    {
        if (hinge == null)
        {
            Debug.LogWarning("[Door] Hinge not assigned! Using door's transform.");
            hinge = transform;
        }

        closedRotation = hinge.rotation;

        if (knobWrapper == null)
        {
            Debug.LogError("[Door] XRKnobWrapper not assigned! Door functionality will not work.");
            return;
        }

        // Register the knob wrapper's grabbed and released events.
        knobWrapper.OnGrabbed += HandleKnobGrabbed;
        knobWrapper.OnReleased += HandleKnobReleased;
    }

    void Update()
    {
        if (hand != null) UpdateDoorRotation();
    }

    private void HandleKnobGrabbed(Transform grabbedHand)
    {
        hand = grabbedHand;
        Debug.Log($"[Door] Hand assigned: {grabbedHand.name}");
    }

    private void HandleKnobReleased()
    {
        hand = null;
        Debug.Log("[Door] Hand released.");
    }

    void UpdateDoorRotation()
    {
        // Calculate the rotation based on the hand's rotation relative to the hinge.
        float handAngle = CalculateHandAngle();
        float clampedAngle = Mathf.Clamp(handAngle, 0f, maxOpenAngle); // Restrict within bounds.

        // Smoothly rotate the door to match the clamped angle.
        Quaternion targetRotation = closedRotation * Quaternion.Euler(0f, clampedAngle, 0f);
        hinge.rotation = Quaternion.Slerp(hinge.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        Debug.Log($"[Door] Hand Angle: {handAngle}, Clamped Angle: {clampedAngle}");
    }

    float CalculateHandAngle()
    {
        // Calculate the angle between the hand and the hinge's forward direction.
        Vector3 hingeToHand = hand.position - hinge.position;
        float angle = Vector3.SignedAngle(hinge.forward, hingeToHand, Vector3.up); // Rotate around the Y-axis.

        return angle;
    }
}