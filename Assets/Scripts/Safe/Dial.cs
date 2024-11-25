using System.Collections.Generic;
using UnityEngine;

public class Dial : MonoBehaviour
{
    [SerializeField] List<int> combination; // Combination sequence
    [SerializeField] int AmountDialNumbers = 40; // Number of steps on the dial
    [SerializeField] SoundManager soundManager; // Sound manager reference
    [SerializeField] XRKnob xrKnob; // XR Knob controlling the dial
    [SerializeField] Door door; // Reference to the Door object

    int currentDigitIndex = 0; // Tracks the current combination digit index
    float StepAngle => 360f / AmountDialNumbers; // Angle per dial step
    bool combinationComplete = false; // Tracks if combination is complete

    void Start()
    {
        if (!ValidateConfiguration()) return;
        xrKnob.OnValueChanged.AddListener(OnKnobRotated);
    }

    bool ValidateConfiguration()
    {
        if (combination == null || combination.Count == 0)
        {
            Debug.LogError("[Dial] Combination not set! Please configure the combination numbers.");
            enabled = false;
            return false;
        }
        if (xrKnob == null)
        {
            Debug.LogError("[Dial] XRKnob not assigned! Dial functionality will not work.");
            enabled = false;
            return false;
        }
        if (door == null)
        {
            Debug.LogError("[Dial] Door not assigned! Cannot control the door.");
            enabled = false;
            return false;
        }
        return true;
    }

    void OnKnobRotated(float currentAngle)
    {
        if (combinationComplete) door.SetRotationFromKnob(currentAngle);
        else HandleCombinationLogic(currentAngle);
    }

    void HandleCombinationLogic(float currentAngle)
    {
        int dialedNumber = Mathf.FloorToInt(currentAngle / StepAngle);

        if (dialedNumber == combination[currentDigitIndex])
        {
            soundManager.PlayDialCorrectSound(); // Play sound for correct input
            currentDigitIndex++;
            if (currentDigitIndex >= combination.Count)
            {
                combinationComplete = true;
                Debug.Log("[Dial] Combination complete! Safe unlocked.");
                soundManager.PlayDoorUnlockSound(); // Play unlock sound
                door.OnCombinationComplete(); // Notify the door that the combination is complete
            }
        }
        else
        {
            soundManager.PlayDialIncorrectSound(); // Play incorrect input sound
            currentDigitIndex = 0; // Reset on incorrect input
            Debug.LogWarning("[Dial] Incorrect input! Combination reset.");
        }
    }
}