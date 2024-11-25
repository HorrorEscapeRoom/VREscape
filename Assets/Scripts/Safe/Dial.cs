using System.Collections.Generic;
using UnityEngine;

public class Dial : MonoBehaviour
{
    [SerializeField] List<int> combination; 
    [SerializeField] int AmountDialNumbers = 40; 
    [SerializeField] SoundManager soundManager; 
    [SerializeField] XRKnob xrKnob; 
    [SerializeField] Door door; 

    int currentDigitIndex = 0; 
    float StepAngle => 360f / AmountDialNumbers; 
    bool combinationComplete = false; 

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
            soundManager.PlayDialCorrectSound(); 
            currentDigitIndex++;
            if (currentDigitIndex >= combination.Count)
            {
                combinationComplete = true;
                Debug.Log("[Dial] Combination complete! Safe unlocked.");
                soundManager.PlayDoorUnlockSound(); 
                door.OnCombinationComplete(); 
            }
        }
        else
        {
            soundManager.PlayDialIncorrectSound(); 
            currentDigitIndex = 0;
            Debug.LogWarning("[Dial] Incorrect input! Combination reset.");
        }
    }
}