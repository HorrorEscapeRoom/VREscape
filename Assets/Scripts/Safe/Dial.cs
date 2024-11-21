using System.Collections.Generic;
using UnityEngine;

public class Dial : MonoBehaviour
{
    [SerializeField] List<int> combination; // The correct combination sequence.
    [SerializeField] public int AmountDialNumbers;  // Total numbers on the dial.
    [SerializeField] float rotationThreshold = 0.5f; // Time to wait before comparing numbers.
    [SerializeField] SoundManager soundManager; // Reference to SoundManager for dial sounds.

    Door door;                              // Reference to the door (parent object).
    int currentDigitIndex = 0;             // Tracks the current position in the combination.
    float lastAngle = 0f;                  // Tracks the last angle of the dial.
    bool isClockwise = true;               // Tracks the direction of rotation.
    bool hasPassedNumber = false;          // Tracks if the dial has passed a number when switching directions.
    XRKnob xRKnob;                         // Handles rotation of the dial.

    float StepAngle => 360f / AmountDialNumbers; // The angle step for each number.

    // Timer variables
    float rotationTimer = 0f;             // Tracks elapsed time since the last rotation.

    void Start()
    {
        if (!TryGetComponent<XRKnob>(out xRKnob))
        {
            Debug.LogError("[Dial] XRKnob component is missing! Please add it to the GameObject.");
            enabled = false;
            return;
        }

        door = GetComponentInParent<Door>();
        if (door == null)
        {
            Debug.LogWarning("[Dial] Parent object does not have a Door component. The dial will work, but the door won't open automatically.");
        }

        if (soundManager == null)
        {
            Debug.LogWarning("[Dial] SoundManager is not assigned! Sounds will not play.");
        }
    }

    void Update()
    {
        HandleRotation();
    }

    void HandleRotation()
    {
        float currentAngle = xRKnob.absAngle;
        float angleDifference = CalculateAngleDifference(currentAngle, lastAngle);

        // If the dial rotates, reset the timer and play move sound.
        if (Mathf.Abs(angleDifference) > Mathf.Epsilon)
        {
            rotationTimer = 0f;
            soundManager?.PlayDialMoveSound();
        }

        // Increment the timer.
        rotationTimer += Time.deltaTime;

        // If the timer hasn't reached the threshold, return early.
        if (rotationTimer < rotationThreshold) return;

        // Timer reached threshold; process the number and reset the timer.
        ProcessDialedNumber(currentAngle);
        rotationTimer = 0f; // Reset the timer after processing.
    }

    void ProcessDialedNumber(float currentAngle)
    {
        // Calculate the number corresponding to the current angle.
        int dialedNumber = Mathf.FloorToInt(currentAngle / StepAngle) % AmountDialNumbers;

        // Log detailed debugging information.
        Debug.Log($"[Dial] Current Angle: {currentAngle}");
        Debug.Log($"[Dial] Step Angle: {StepAngle}");
        Debug.Log($"[Dial] Dialed Number (calculated): {dialedNumber}");

        // Determine rotation direction (clockwise or counterclockwise).
        bool newDirection = CalculateAngleDifference(currentAngle, lastAngle) > 0;
        if (newDirection != isClockwise)
        {
            if (!hasPassedNumber)
            {
                Debug.Log("[Dial] Direction changed without passing the required number. Resetting.");
                ResetCombination();
                return;
            }

            isClockwise = newDirection;
            hasPassedNumber = false;
            Debug.Log($"[Dial] Direction changed to {(isClockwise ? "Clockwise" : "Counterclockwise")}");
        }

        // Check if the dialed number matches the current combination digit.
        if (IsNumberCorrect(dialedNumber))
        {
            hasPassedNumber = true;

            if (TryAdvance())
            {
                Debug.Log("[Dial] Combination complete. Safe unlocked!");
                OpenDoor();
            }
        }
        else
        {
            Debug.Log($"[Dial] Incorrect number {dialedNumber} dialed. Resetting.");
            ResetCombination();
        }

        lastAngle = currentAngle; // Update the last angle for the next frame.
    }

    float CalculateAngleDifference(float currentAngle, float lastAngle)
    {
        float difference = currentAngle - lastAngle;
        return Mathf.Abs(difference) > 180f ? -Mathf.Sign(difference) * (360f - Mathf.Abs(difference)) : difference;
    }

    bool IsNumberCorrect(int dialedNumber)
    {
        bool isCorrect = currentDigitIndex < combination.Count && dialedNumber == combination[currentDigitIndex];
        if (soundManager != null)
        {
            if (isCorrect)
                soundManager.PlayDialCorrectSound();
            else
                soundManager.PlayDialIncorrectSound();
        }
        Debug.Log($"[Dial] IsNumberCorrect({dialedNumber}) = {isCorrect}");
        return isCorrect;
    }

    bool TryAdvance()
    {
        currentDigitIndex++;
        Debug.Log($"[Dial] Advanced to combination index {currentDigitIndex}");
        return currentDigitIndex == combination.Count; // Return true if the combination is complete.
    }

    void ResetCombination()
    {
        currentDigitIndex = 0;
        hasPassedNumber = false;
        Debug.Log("[Dial] Combination reset.");
    }

    void OpenDoor()
    {
        if (door != null) door.Open();
        else Debug.LogWarning("[Dial] Door component is not assigned. Cannot open the door.");
    }
}