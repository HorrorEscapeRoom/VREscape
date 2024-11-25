using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform hinge; // Hinge for door rotation
    [SerializeField] float maxOpenAngle = 90f; // Maximum opening angle
    [SerializeField] SoundManager soundManager; // Reference to SoundManager for audio

    Quaternion closedRotation; // Initial closed door rotation
    float currentKnobAngle = 0f; // Tracks current knob angle for interpolation
    bool canControlDoor = false; // Tracks if the door can be controlled (after combination is complete)

    void Start()
    {
        if (hinge == null)
        {
            Debug.LogWarning("[Door] Hinge not assigned! Using door's transform.");
            hinge = transform;
        }

        closedRotation = hinge.rotation;
    }

    public void OnCombinationComplete()
    {
        canControlDoor = true;
        Debug.Log("[Door] Combination completed. Door is now controllable.");
    }

    public void SetRotationFromKnob(float knobAngle)
    {
        if (!canControlDoor)
        {
            Debug.LogWarning("[Door] Door cannot be controlled until combination is complete.");
            return;
        }

        // Clamp the knob angle to ensure it stays within the valid range
        currentKnobAngle = Mathf.Clamp(knobAngle, 0f, maxOpenAngle);

        // Calculate the target rotation for the door
        Quaternion targetRotation = closedRotation * Quaternion.Euler(0f, currentKnobAngle, 0f);

        // Set the hinge's rotation to the target rotation
        hinge.rotation = targetRotation;

        Debug.Log($"[Door] Adjusting door to match knob angle: {currentKnobAngle}°.");

        // Play sound for door movement
        if (Mathf.Approximately(currentKnobAngle, maxOpenAngle))
        {
            soundManager.PlayDoorOpenSound();
        }
        else if (Mathf.Approximately(currentKnobAngle, 0f))
        {
            soundManager.PlayDoorCloseSound();
        }
    }
}