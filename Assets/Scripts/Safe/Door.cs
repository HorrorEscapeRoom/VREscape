using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform hinge; 
    [SerializeField] float maxOpenAngle = 90f; 
    [SerializeField] SoundManager soundManager; 

    Quaternion closedRotation; 
    float currentKnobAngle = 0f; 
    bool canControlDoor = false; 
    bool isLocked = true; 

    void Start()
    {
        if (hinge == null)
        {
            Debug.LogWarning("[Door] Hinge not assigned! Using door's transform.");
            hinge = transform;
        }

        closedRotation = hinge.rotation;
    }

    public void LockDoor()
    {
        isLocked = true;
        canControlDoor = false;
        soundManager.PlayDoorLockSound();
        Debug.Log("[Door] Door is now locked.");
    }

    public void UnlockDoor()
    {
        isLocked = false;
        canControlDoor = true;
        soundManager.PlayDoorUnlockSound();
        Debug.Log("[Door] Door is now unlocked.");
    }

    public void OnCombinationComplete()
    {
        if (isLocked) UnlockDoor();
        else Debug.LogWarning("[Door] Combination completed, but the door is already unlocked.");
    }

    public void SetRotationFromKnob(float knobAngle)
    {
        if (!canControlDoor)
        {
            Debug.LogWarning("[Door] Door cannot be controlled until combination is complete or unlocked.");
            return;
        }

        currentKnobAngle = Mathf.Clamp(knobAngle, 0f, maxOpenAngle);
        Quaternion targetRotation = closedRotation * Quaternion.Euler(0f, currentKnobAngle, 0f);
        hinge.rotation = targetRotation;
        Debug.Log($"[Door] Adjusting door to match knob angle: {currentKnobAngle}°.");
        if (Mathf.Approximately(currentKnobAngle, maxOpenAngle)) soundManager.PlayDoorMoveSound();
        else if (Mathf.Approximately(currentKnobAngle, 0f)) LockDoor();
    }
}